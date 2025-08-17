using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using ClsDataAccess;

namespace Business
{
    public class ClsDetainedLicense
    {
        public int DetainID { get; set; }

        public int LicenseID { get; set; }

        public DateTime DetainDate { get; set; }

        public decimal FineFees { get; set; }

        public int CreatedByUserID { get; set; }

        public bool IsReleased { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int ReleasedByUserID { get; set; }

        public int ReleaseApplicationID { get; set; }


        public ClsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.ReleaseApplicationID = -1;
            this.ReleasedByUserID = -1;
            this.ReleaseDate = DateTime.MaxValue;
            this.DetainDate = DateTime.MinValue;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;

            Mode = enMode.ADD;
        }

        public ClsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID,
                                  bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseDate = ReleaseDate;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;

            Mode = enMode.Ubdate;
        }

        enum enMode
        {
            ADD,
            Ubdate
        }

        enMode Mode { get; set; }

        private bool _AddNew()
        {
            this.DetainID = ClsDetainedLicenseData.AddNew(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
            return (this.DetainID != -1);
        }

        private bool _Ubdate()
        {
            return ClsDetainedLicenseData.UbdateRecored(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }

        public static ClsDetainedLicense GetRecored(int LicenseID)
        {
            int DetainID = -1;
            DateTime DetainDate = DateTime.MinValue;
            decimal FineFees = 0;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (ClsDetainedLicenseData.GetRecored(LicenseID, ref DetainID, ref DetainDate, ref FineFees, ref CreatedByUserID,
                                                  ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new ClsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate,
                                              ReleasedByUserID, ReleaseApplicationID);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetList()
        {
            return ClsDetainedLicenseData.GetList();
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

                    return _Ubdate();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return ClsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return ClsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        }
    }
}