using System;
using ClsDataAccess;


namespace Business
{
    public class ClsApplicationBusiness
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 8
        };

        public enum EnApplicationStatus
        {
            New = 1, Cancelled = 2, Completed = 3
        }

        public EnApplicationStatus ApplicationStatus { get; set; }

        public int? ApplicationID { get; set; } 

        public int PersonID { get; set; }

        public ClsBusinessPeople clsPerson;

        public decimal PaidFees { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime LastStatusDate { get; set; }

        public int ApplicationTypeID { get; set; }

        public string ApplicationStatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case EnApplicationStatus.New:
                        return "New";
                    case EnApplicationStatus.Cancelled:
                        return "Cancelled";
                    case EnApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }

        public int CreatedByUserID { get; set; }

        public ClsUsersBussiness CreatedByUserInfo;

        public ClsApplicationTypeBusiness ApplicationTypeInfo;

        public enum enMode
        {
            ADD,
            UPdate
        }

        public enMode Mode;

        public ClsApplicationBusiness()
        {
            this.ApplicationID = -1;
            PaidFees = 0;
            PersonID = -1;
            ApplicationDate = DateTime.Now;
            LastStatusDate = DateTime.Now;
            ApplicationTypeID = -1;
            ApplicationStatus = EnApplicationStatus.New ;
            CreatedByUserID = -1;

            Mode = enMode.ADD;
        }

        ClsApplicationBusiness(int ID, int PersonID, decimal PaidFees, DateTime ApplicationDate, DateTime LastStatusDate,
                               int ApplicationTypeID, EnApplicationStatus ApplicationStatus, int CreatedByUserID)
        {
            this.ApplicationID = ID;
            this.PersonID = PersonID;
            this.clsPerson = ClsBusinessPeople.Find(PersonID);
            this.PaidFees = PaidFees;
            this.ApplicationDate = ApplicationDate;
            this.LastStatusDate = LastStatusDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = ClsApplicationTypeBusiness.GetRecored(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = ClsUsersBussiness.Find(CreatedByUserID);

            Mode = enMode.UPdate;
        }

        private bool _AddNew()
        {
            this.ApplicationID = ClsApplicationData.AddNewRecored(this.PersonID,this.ApplicationDate, this.ApplicationTypeID,
                                                       (byte)this.ApplicationStatus, this.LastStatusDate,this.PaidFees,
                                                       this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _Ubdate()
        {
            return ClsApplicationData.UbdateRecored((int)this.ApplicationID,this.PersonID, this.ApplicationDate, this.ApplicationTypeID,
                                                    (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees,
                                                    this.CreatedByUserID);
        }

        public static ClsApplicationBusiness Find(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = new DateTime();
            int ApplicationTypeID = -1;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = new DateTime();
            decimal PaidFees = 0;
            int CreatedByUserID = 0;

            if (ClsApplicationData.GetRecored(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID, 
                                              ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new ClsApplicationBusiness(ApplicationID, ApplicantPersonID, PaidFees, ApplicationDate, LastStatusDate, 
                                                  ApplicationTypeID, (EnApplicationStatus)ApplicationStatus , CreatedByUserID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public bool DeleteRecored()
        {
            return ClsApplicationData.DeleteRecored((int)this.ApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.ADD:

                    if (_AddNew())
                    {
                        Mode = enMode.UPdate;
                        return true;
                    }
                    else
                    { 
                        return false;
                    }

                case enMode.UPdate:
                    return _Ubdate();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);

            return false;
        }

        public bool Cancel()
        {
            return ClsApplicationData.UpdateStatus((int)this.ApplicationID, 2);
        }

        public bool SetCompleted()
        {
            return ClsApplicationData.UpdateStatus((int)this.ApplicationID, 3);
        }

        public static int? GetActiveApplicationID(int PersonID, int LicenseClassID, EnApplicationStatus ApplicationStatus)
        {
            return ClsApplicationData.GetActiveApplicationID(PersonID, LicenseClassID, (int)ApplicationStatus);
        }
    }
}