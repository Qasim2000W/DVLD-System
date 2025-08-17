using System;
using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsDrivers
    {
        public int ID { get; set; }

        public int PersonID { get; set; }

        public ClsBusinessPeople clsPerson;

        public int CreatedUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public ClsDrivers()
        {
            this.ID = -1;
            this.PersonID = -1;
            this.CreatedUser = -1;
            this.CreatedDate = DateTime.MinValue;
        }

        ClsDrivers(int ID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.ID = ID;
            this.PersonID = PersonID;
            this.CreatedUser = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.clsPerson = ClsBusinessPeople.Find(PersonID);
        }

        public bool AddNew()
        {
            this.ID = ClsDriversData.AddNew(this.PersonID, this.CreatedUser, this.CreatedDate);

            return (this.ID != -1);
        }

        public static bool ExistsIs(int PersonID)
        {
            return ClsDriversData.ExistsIs(PersonID);
        }

        public static ClsDrivers GetRecored(int PersonID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.MinValue;

            if (ClsDriversData.GetRecored(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new ClsDrivers(DriverID, PersonID,  CreatedByUserID,  CreatedDate);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsDrivers GetRecoredByID(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.MinValue;

            if (ClsDriversData.GetRecoredByID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new ClsDrivers(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetList()
        {
            return ClsDriversData.GetList();
        }
    }
}