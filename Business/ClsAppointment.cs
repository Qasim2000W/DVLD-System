using System;
using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsAppointment
    {
        public int ID { get; set; }

        public ClsTestTypeBusiness.enTestType TestTypeID { get; set; }

        public decimal PaidFees { get; set; }

        public DateTime AppointmentDate { get; set; }

        public bool IsLocked { get; set; }

        public int LocalDrivingLicenseApplicationID { get; set; }

        public int CreatedByUserID { get; set; }

        public int RetakeTestApplicationID { get; set; }
        public ClsApplicationBusiness RetakeTestAppInfo { set; get; }

        public enum enMode
        {
            ADD,
            Ubdate
        }

        public enMode Mode { get; set; }

        public int TestID
        {
            get { return _GetTestID(); }
        }

        public ClsAppointment()
        {
            ID = -1;
            PaidFees = 0;
            TestTypeID = (ClsTestTypeBusiness.enTestType.VisionTest);
            AppointmentDate = DateTime.Now;
            IsLocked = false;
            LocalDrivingLicenseApplicationID = -1;
            CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.ADD;
        }

        ClsAppointment(ClsTestTypeBusiness.enTestType TestTypeID, int LocalDrivingLicenseApplicationID, int AppointmentID, DateTime AppointmentDate,
                                        decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this.ID = AppointmentID;
            this.TestTypeID = TestTypeID;
            this.PaidFees = PaidFees;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.IsLocked = IsLocked;
            this.CreatedByUserID = CreatedByUserID;
            this.RetakeTestApplicationID = RetakeTestApplicationID;

            if (RetakeTestApplicationID!=-1)
                RetakeTestAppInfo = ClsApplicationBusiness.Find(RetakeTestApplicationID);
            

            Mode = enMode.Ubdate;
        }

        private bool _AddNew()
        {
            this.ID = ClsAppointmentData.AddNewRecored((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, 
                                                       this.AppointmentDate,this.PaidFees, this.CreatedByUserID, 
                                                       this.IsLocked, this.RetakeTestApplicationID);

            return (this.ID != -1);
        }

        private bool _Update()
        {
            return ClsAppointmentData.UbdateRecored(this.ID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                                                    this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked,
                                                    this.RetakeTestApplicationID);
        }

        public static ClsAppointment GetRecored(int LDLAppID, int TestTypeID)
        {
            DateTime AppointmentDate = DateTime.MinValue;
            decimal PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int AppointmentID = -1;
            int RetakeTestApplicationID = -1;

            if (ClsAppointmentData.GetRecored(LDLAppID, TestTypeID, ref AppointmentID, ref AppointmentDate, ref PaidFees,
                                                 ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new ClsAppointment((ClsTestTypeBusiness.enTestType)TestTypeID, LDLAppID, AppointmentID, AppointmentDate, PaidFees, CreatedByUserID, 
                                          IsLocked, RetakeTestApplicationID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }

        }

        public static byte GetCountTrial(int LDLAppID, int TestTypeID)
        {
            return ClsAppointmentData.GetTotalTrail(LDLAppID, TestTypeID);
        }

        public static ClsAppointment GetRecored(int AppointmentID)
        {
            DateTime AppointmentDate = DateTime.MinValue;
            decimal PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int LDLAppID = -1;
            int TestTypeID = -1;
            int RetakeTestApplicationID = -1;

            if (ClsAppointmentData.GetRecored(AppointmentID, ref LDLAppID, ref TestTypeID, ref AppointmentDate, ref PaidFees,
                                              ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new ClsAppointment((ClsTestTypeBusiness.enTestType)TestTypeID, LDLAppID, AppointmentID, AppointmentDate, PaidFees, CreatedByUserID,
                                          IsLocked, RetakeTestApplicationID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }

        }

        public static DataTable GetRecoredDT(int LDLAppID, int TestTypeID)
        {
           return ClsAppointmentData.GetRecored(LDLAppID, TestTypeID);
        }

        public static DataTable GetList()
        {
            return ClsAppointmentData.GetList();
        }

        public bool Save()
        {
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

                    return _Update();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }

        private int _GetTestID()
        {
            return ClsAppointmentData.GetTestID(this.ID);
        }
    }
}