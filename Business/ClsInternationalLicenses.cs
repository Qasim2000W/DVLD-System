using System;
using System.Data;
using ClsDataAccess;
using static Business.ClsApplicationBusiness;

namespace Business
{
    public class ClsInternationalLicenses:ClsApplicationBusiness
    {
        public int ID { get; set; }

        public int DriverID { get; set; }

        public int IssuedUsingLocalLicenseID { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsActive { get; set; }

        public ClsDrivers DriverInfo { get; set; }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public ClsInternationalLicenses()
        {
            this.ID = -1;
            this.ApplicationID = (int)ClsApplicationBusiness.enApplicationType.NewInternationalLicense;
            this.DriverID = -1;
            this.IsActive = false;
            this.CreatedByUserID = -1;
            this.ExpirationDate = DateTime.Now;
            this.IssueDate = DateTime.Now;
            this.IssuedUsingLocalLicenseID = -1;
        }

        ClsInternationalLicenses(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, EnApplicationStatus ApplicationStatus, DateTime 
                                LastStatusDate, decimal PaidFees, int CreatedByUserID, int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
                               DateTime IssueDate, DateTime ExpirationDate, bool IsActive)
        {
            base.ApplicationID = ApplicationID;
            base.PersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)ClsApplicationBusiness.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.ID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = ClsDrivers.GetRecoredByID(this.DriverID);

            Mode = enMode.Update;
        }

        public bool AddNew()
        {
            base.Mode = (ClsApplicationBusiness.enMode)Mode;
            if (!base.Save())
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return false;
            }

            this.ID = ClsInternationalLicensesData.AddNew((int)this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate,
                                                          this.IsActive, this.CreatedByUserID);

            return (this.ID != -1);
        }

        public static bool ExistsIs(int LicenseID)
        {
            return ClsInternationalLicensesData.ExistsIs(LicenseID);
        }

        public static DataTable GetListForDriver(int DriverID)
        {
            return ClsInternationalLicensesData.GetListForDriver(DriverID);
        }

        public static ClsInternationalLicenses GetRecored(int IssuedUsingLocalLicenseID)
        {
            int InternationalLicenseID = -1;
            int DriverID = -1;
            int ApplicationID = -1;
            bool IsActive = false;
            int CreatedByUserID = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MaxValue;


            if (ClsInternationalLicensesData.GetRecored(IssuedUsingLocalLicenseID, ref InternationalLicenseID, ref ApplicationID, ref DriverID, 
                                                        ref IssueDate ,ref ExpirationDate,ref IsActive , ref CreatedByUserID))
            {
                ClsApplicationBusiness Application = ClsApplicationBusiness.Find(ApplicationID);

                return new ClsInternationalLicenses((int)Application.ApplicationID, Application.PersonID, Application.ApplicationDate, (EnApplicationStatus)
                                                   Application.ApplicationStatus, Application.LastStatusDate,  Application.PaidFees, Application.CreatedByUserID,
                                                  InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsInternationalLicenses GetRecoredByID(int InternationalLicenseID)
        {
            int IssuedUsingLocalLicenseID = -1;
            int DriverID = -1;
            int ApplicationID = -1;
            bool IsActive = false;
            int CreatedByUserID = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MaxValue;


            if (ClsInternationalLicensesData.GetRecoredByID(InternationalLicenseID, ref IssuedUsingLocalLicenseID, ref ApplicationID, ref DriverID,
                                                        ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                ClsApplicationBusiness Application = ClsApplicationBusiness.Find(ApplicationID);

                return new ClsInternationalLicenses((int)Application.ApplicationID, Application.PersonID, Application.ApplicationDate, (EnApplicationStatus)
                                                   Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID,
                                                  InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetList()
        {
            return ClsInternationalLicensesData.GetList();
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return ClsInternationalLicensesData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
    }
}