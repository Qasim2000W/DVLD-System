using System;
using System.Data;
using ClsDataAccess;
using static Business.ClsApplicationBusiness;

namespace Business
{
    public class ClsLocalDrivingLicenseApplicationBusiness : ClsApplicationBusiness
    {
        public int ID { get; set; }

        public int LicenseClassID { get; set; }
        public ClsClassLicenseBusiness LicenseClassInfo;

        public string FullName { get { return ClsBusinessPeople.Find(PersonID).fullname; } }

        public enum enMode
        {
            ADD,
            Ubdate
        }

        enMode Mode;

        public ClsLocalDrivingLicenseApplicationBusiness()
        {
            ID = -1;
            LicenseClassID = -1;

            Mode = enMode.ADD;
        }

        ClsLocalDrivingLicenseApplicationBusiness(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
                                                DateTime ApplicationDate, int ApplicationTypeID, EnApplicationStatus ApplicationStatus,
                                                DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.ID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = ClsClassLicenseBusiness.GetRecored(LicenseClassID);
            this.LastStatusDate = LastStatusDate;
            this.PersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Ubdate;
        }

        private bool _AddNew()
        {
            this.ID = ClsLocalDrivingLicenseApplicationData.AddNewRecored((int)this.ApplicationID, this.LicenseClassID);

            return (this.ID != -1);
        }

        public static ClsLocalDrivingLicenseApplicationBusiness Find(int ID)
        {
            int ApplicationID = -1;
            int LicenseClassID = -1;
            bool IsFound = ClsLocalDrivingLicenseApplicationData.GetRecored(ID, ref ApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                ClsApplicationBusiness Application = ClsApplicationBusiness.Find(ApplicationID);

                return new ClsLocalDrivingLicenseApplicationBusiness(ID, (int)Application.ApplicationID, Application.PersonID, Application.ApplicationDate, Application.
                                                                    ApplicationTypeID, (EnApplicationStatus)Application.ApplicationStatus, Application.
                                                                   LastStatusDate, Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsLocalDrivingLicenseApplicationBusiness FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1;
            int LicenseClassID = -1;
            bool IsFound = ClsLocalDrivingLicenseApplicationData.GetRecoredByApplicationID(ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                ClsApplicationBusiness Application = ClsApplicationBusiness.Find(ApplicationID);

                return new ClsLocalDrivingLicenseApplicationBusiness(LocalDrivingLicenseApplicationID, (int)Application.ApplicationID, Application.PersonID,
                                                                    Application.ApplicationDate, Application.ApplicationTypeID, (EnApplicationStatus)Application.
                                                                   ApplicationStatus, Application.LastStatusDate, Application.PaidFees, Application.
                                                                   CreatedByUserID, LicenseClassID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public bool Delete()
        {
            bool DeletePassed = false;
            DeletePassed = ClsLocalDrivingLicenseApplicationData.DeleteRecored(this.ID);

            if (!DeletePassed)
                return DeletePassed;

            return base.DeleteRecored();
        }

        public bool Save()
        {
            base.Mode = (ClsApplicationBusiness.enMode)Mode;

            if (!base.Save())
                return false;


            switch (Mode)
            {
                case enMode.ADD:

                    if (_AddNew())
                    {
                        Mode = enMode.Ubdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Ubdate:

                    return true;
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }

        public int GetActiveLicenseID()
        {
            return ClsLicenses.GetActiveLicense(this.PersonID, this.LicenseClassID);
        }

        public int GetPassedTest()
        {
            return ClsTests.GetPassedTestCount(this.ID);
        }

        public byte GetTotalTrail(ClsTestTypeBusiness.enTestType testTypeID)
        {
            return ClsAppointment.GetCountTrial(this.ID, (int)testTypeID);
        }

        public bool DoesThisTestType(ClsTestTypeBusiness.enTestType testTypeID)
        {
            return ClsLocalDrivingLicenseApplicationData.DoesThisTestType(this.ID, (int)testTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, ClsTestTypeBusiness.enTestType testTypeID)
        {
            return ClsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)testTypeID);
        }

        public bool IsThereAnActiveScheduledTest(ClsTestTypeBusiness.enTestType testTypeID)
        {
            return ClsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.ID, (int)testTypeID);
        }

        public ClsTests FindLastTestForTestType(ClsTestTypeBusiness.enTestType testTypeID)
        {
            return ClsTests.FindLastTestByTestType(this.PersonID, this.LicenseClassID, testTypeID);
        }

        public bool PassedAllTests()
        {
            return ClsTests.PassedAllTests(this.ID);
        }

        public static DataTable GetList()
        {
            return ClsLocalDrivingLicenseApplicationData.GetList();
        }

        public int IssueForTheFirstTime(string Notes, int CreatedByUserID)
        {
            ClsDrivers clsDrivers = ClsDrivers.GetRecored(this.PersonID);

            if (clsDrivers == null)
            {
                clsDrivers.CreatedUser = CreatedByUserID;
                clsDrivers.CreatedDate = DateTime.Now;
                clsDrivers.PersonID = this.PersonID;

                if (!clsDrivers.AddNew())
                {
                    ClsEventLog.EventLogger("New Data Not Saved, something wrong", ClsEventLog.ENTypeMessage.warning);
                    return -1; 
                }
            }

            ClsLicenses clsLicense = new ClsLicenses();
            clsLicense.Notes = Notes;
            clsLicense.ApplicationID = (int)this.ApplicationID;
            clsLicense.CreatedByUserID = CreatedByUserID;
            clsLicense.LicenseClassID = this.LicenseClassID;
            clsLicense.IssueReason = ClsLicenses.enIssueReason.FirstTime;
            clsLicense.DriverID = clsDrivers.ID;
            clsLicense.IssueDate = DateTime.Now;
            clsLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            clsLicense.IsActive = true;
            clsLicense.PaidFees = this.LicenseClassInfo.ClassFees;

            if (clsLicense.SAVE())
            {
                this.SetCompleted();

                return clsLicense.ID;
            }
            else
            {
                ClsEventLog.EventLogger("New Data Not Saved, something wrong", ClsEventLog.ENTypeMessage.warning);
                return -1;
            }
        }
    }
}