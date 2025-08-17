using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsUsersBussiness
    {
        public int UserID { get; set; }

        public int PersonID { get; set; }

        public ClsBusinessPeople ClsPerson;

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public enum enMode
        {
            ADD,
            Ubdate
        }

        enMode Mode;

        public ClsUsersBussiness()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.ADD;
        }

        private ClsUsersBussiness(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.ClsPerson = ClsBusinessPeople.Find(PersonID);
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Ubdate;
        }

        static public DataTable GetListUsers()
        {
            return ClsDataUsers.GetListRecoredsUsers();
        }

        private bool _AddNew()
        {
            this.UserID = ClsDataUsers.AddNewUser(this.PersonID,this.UserName,this.Password, this.IsActive);

            return (this.UserID != -1);
        }

        private bool _Ubdate()
        {
            return ClsDataUsers.UbdateRecored(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public static ClsUsersBussiness Find(int UserID)
        {
            int PersonID = -1;
            string UserName = "";
            string Password = "";
            bool IsActive = false;


            if (ClsDataUsers.GetrecoredByID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                return new ClsUsersBussiness(UserID, PersonID, UserName,  Password,  IsActive);
            }
            else
            {
                return null;
            }
        }

        public static ClsUsersBussiness Find(string UserName)
        {
            int PersonID = -1;
            int UserID = -1;
            string Password = "";
            bool IsActive = false;

            if (ClsDataUsers.GetrecoredByUserName(UserName, ref UserID, ref PersonID , ref Password, ref IsActive))
            {
                return new ClsUsersBussiness(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static bool Delete(int UserID)
        {
            return ClsDataUsers.DeleteRecored(UserID);
        }

        public static bool IsExists(int UserID)
        {
            return ClsDataUsers.ExistsIs(UserID);
        }

        public static bool IsExists(string UserName)
        {
            return ClsDataUsers.ExistsIs(UserName);
        }

        public static bool IsExistsPersonID(int PersonID)
        {
            return ClsDataUsers.ExistsIsByPersonID(PersonID);
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

        public static bool ChangePassword(int UserID, string NewPassword)
        {
           return ClsDataUsers.ChangePassword(UserID, NewPassword);
        }
    }
}