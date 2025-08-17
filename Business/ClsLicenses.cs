using System;
using System.Data;
using ClsDataAccess;
using static System.Net.Mime.MediaTypeNames;

namespace Business
{
    public class ClsLicenses
    {
        public int ID { get; set; }

        public int ApplicationID { get; set; }

        public int DriverID { get; set; }

        public ClsDrivers clsDriver;

        public int LicenseClassID { get; set; }

        public ClsClassLicenseBusiness ClsLicenseClass;

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Notes { get; set; }

        public decimal PaidFees { get; set; }

        public bool IsActive { get; set; }

        public enIssueReason IssueReason { get; set; }

        public string IssueReasonText
        {
            get 
            { 
                return GetIssueReasonText(this.IssueReason); 
            } 
        }

        public int CreatedByUserID { get; set; }

        public bool IsDetained { get { return ClsDetainedLicense.IsLicenseDetained(this.ID); } }

        public ClsDetainedLicense DetainedInfo { get; set; }

        public enum enMode{ADD, Ubdate}

        enMode Mode;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public ClsLicenses()
        {
            this.ID = -1;
            this.DriverID = -1;
            this.IssueReason = 0;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MaxValue;
            this.CreatedByUserID = -1;
            this.Notes = string.Empty;
            this.IsActive = false;
            this.LicenseClassID = -1;
            this.PaidFees = 0;
            this.ApplicationID = -1;

            Mode = enMode.ADD;
        }

        public ClsLicenses(int ID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate,
                           string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            this.ID = ID;
            this.DriverID = DriverID;
            this.clsDriver = ClsDrivers.GetRecoredByID(DriverID);
            this.IssueReason = (enIssueReason)IssueReason;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.CreatedByUserID = CreatedByUserID;
            this.Notes = Notes;
            this.IsActive = IsActive;
            this.LicenseClassID = LicenseClassID;
            this.ClsLicenseClass = ClsClassLicenseBusiness.GetRecored(LicenseClassID);
            this.PaidFees = PaidFees;
            this.ApplicationID = ApplicationID;
            this.DetainedInfo = ClsDetainedLicense.GetRecored(ID);

            Mode = enMode.Ubdate;
        }

        private bool _AddNew()
        {
            this.ID = ClsLicensesData.AddNew(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate, 
                                            this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason,
                                            this.CreatedByUserID);

            return (this.ID != -1);
        }

        private bool _Ubdate()
        {
            return ClsLicensesData.UbdateRecored(this.ID,this.ApplicationID, this.DriverID,this.LicenseClassID, this.IssueDate, 
                                                 this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, 
                                                 this.CreatedByUserID);
        }

        public static ClsLicenses Find(int ApplicationID)
        {
            int ID = -1;
            int DriverID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate= DateTime.MinValue;
            DateTime ExpirationDate=DateTime.MaxValue;
            string Notes= string.Empty;
            decimal PaidFees=0; 
            bool IsActive= false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;

            if (ClsLicensesData.GetRecored(ApplicationID, ref ID, ref DriverID, ref LicenseClassID, ref IssueDate, 
                ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new ClsLicenses(ID,ApplicationID,DriverID,LicenseClassID,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,
                                       IssueReason,CreatedByUserID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsLicenses FindByID(int ID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MaxValue;
            string Notes = string.Empty;
            decimal PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;

            if (ClsLicensesData.GetRecoredByID(ID, ref ApplicationID, ref DriverID, ref LicenseClassID, ref IssueDate,
                ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new ClsLicenses(ID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive,
                                       IssueReason, CreatedByUserID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetListLicenseForPersonToShowing(int DriverID)
        {
            return ClsLicensesData.GetListLicenseForPersonToShowing(DriverID);
        }

        public static DataTable GetListForDriver(int DriverID)
        {
            return ClsLicensesData.GetListForDriver(DriverID);
        }

        public bool SAVE()
        {
            switch(Mode)
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
                    return _Ubdate();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }

        public static int GetActiveLicense(int PersonID, int LicenseClassID)
        {
            return ClsLicensesData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        public static bool IsLicenseIDExistsByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicense(PersonID, LicenseClassID) != -1);
        }

        public static string GetIssueReasonText(enIssueReason issueReason)
        {
            switch (issueReason)
            {
                case enIssueReason.FirstTime:
                    return "First time";

                case enIssueReason.Renew:
                    return"Renew";

                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                default:
                    return "First Time";
            }
        }

        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        public ClsLicenses RenewLicense(string Notes, int CreatedByUserID)
        {
            ClsApplicationBusiness clsApplication = new ClsApplicationBusiness();
            clsApplication.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.Completed;
            clsApplication.ApplicationDate = DateTime.Now;
            clsApplication.LastStatusDate = DateTime.Now;
            clsApplication.ApplicationTypeID = (int)ClsApplicationBusiness.enApplicationType.RenewDrivingLicense;
            clsApplication.CreatedByUserID = CreatedByUserID;
            clsApplication.PersonID = this.clsDriver.PersonID;
            clsApplication.PaidFees = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.RenewDrivingLicense).Fees;
            
            if (!clsApplication.Save())
            {
                return null;
            }

            ClsLicenses NewclsLicenses = new ClsLicenses();
            NewclsLicenses.IsActive = true;
            NewclsLicenses.Notes = Notes;
            NewclsLicenses.PaidFees = this.ClsLicenseClass.ClassFees;
            NewclsLicenses.LicenseClassID = this.LicenseClassID;
            NewclsLicenses.ApplicationID = (int)clsApplication.ApplicationID;
            NewclsLicenses.CreatedByUserID = CreatedByUserID;
            NewclsLicenses.DriverID = this.DriverID;
            NewclsLicenses.IssueDate = DateTime.Now;
            NewclsLicenses.ExpirationDate = DateTime.Now.AddYears(this.ClsLicenseClass.DefaultValidityLength);
            NewclsLicenses.IssueReason = ClsLicenses.enIssueReason.Renew;

            if (!NewclsLicenses.SAVE())
            {
                return null;
            }

            DeactivateCurrentLicense();

            return NewclsLicenses;
        }

        public bool DeactivateCurrentLicense()
        {
            return ClsLicensesData.DeActiveLicense(this.ID);
        }

        public ClsLicenses Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            ClsApplicationBusiness clsApplication = new ClsApplicationBusiness();
            clsApplication.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.Completed;
            clsApplication.ApplicationDate = DateTime.Now;
            clsApplication.LastStatusDate = DateTime.Now;
            clsApplication.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ? (int)ClsApplicationBusiness.enApplicationType.ReplaceDamagedDrivingLicense
                                             : (int)ClsApplicationBusiness.enApplicationType.ReplaceLostDrivingLicense;
            
            clsApplication.CreatedByUserID = CreatedByUserID;
            clsApplication.PersonID = this.clsDriver.PersonID;
            clsApplication.PaidFees = ClsApplicationTypeBusiness.GetRecored(clsApplication.ApplicationTypeID).Fees;

            if (!clsApplication.Save())
            {
                return null;
            }

            ClsLicenses NewclsLicenses = new ClsLicenses();
            NewclsLicenses.IsActive = true;
            NewclsLicenses.PaidFees = 0;
            NewclsLicenses.LicenseClassID = this.LicenseClassID;
            NewclsLicenses.ApplicationID = (int)clsApplication.ApplicationID;
            NewclsLicenses.CreatedByUserID = CreatedByUserID;
            NewclsLicenses.DriverID = this.DriverID;
            NewclsLicenses.IssueDate = DateTime.Now;
            NewclsLicenses.ExpirationDate = this.ExpirationDate;
            NewclsLicenses.IssueReason = IssueReason;

            if (!NewclsLicenses.SAVE())
            {
                return null;
            }

            DeactivateCurrentLicense();

            return NewclsLicenses;
        }

        public int Detain(decimal Fees, int CreatedByUser)
        {
            ClsDetainedLicense clsDetained = new ClsDetainedLicense();
            clsDetained.LicenseID = this.ID;
            clsDetained.CreatedByUserID = CreatedByUser;
            clsDetained.DetainDate = DateTime.Now;
            clsDetained.FineFees = Fees;

            if (!clsDetained.Save())
            {
                return -1;
            }

            return clsDetained.DetainID;
        }

        public bool ReleaseDetainedLicense(int ReleaseByUser,ref int  ApplicationID)
        {
            ClsApplicationBusiness clsApplication = new ClsApplicationBusiness();
            clsApplication.PaidFees = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.ReleaseDetainedDrivingLicsense).Fees;
            clsApplication.ApplicationDate = DateTime.Now;
            clsApplication.LastStatusDate = DateTime.Now;
            clsApplication.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.Completed;
            clsApplication.ApplicationTypeID = (int)ClsApplicationBusiness.enApplicationType.ReleaseDetainedDrivingLicsense;
            clsApplication.CreatedByUserID = ReleaseByUser;
            clsApplication.PersonID = this.clsDriver.PersonID;

            if (!clsApplication.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = (int)clsApplication.ApplicationID;

            return this.DetainedInfo.ReleaseDetainedLicense(ReleaseByUser, (int)clsApplication.ApplicationID);
        }
    }
}