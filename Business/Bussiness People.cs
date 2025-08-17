using System;
using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsBusinessPeople
    {
        public int id { get; set; }

        public string NationalNo { get; set; }

        public string firstname { get; set; }
       
        public string Secondname { get; set; }

        public string thirdname { get; set; }

        public string lastname { get; set; }

        public string fullname
        {
            get { return firstname + " " + Secondname + " " + thirdname + " " + lastname; }
        }

        public DateTime DateOfbirth { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }

        public byte Gendor { get; set; }

        public string ImagePath { get; set; }

        public int countryid { get; set; }

        public string countryName { get; set; }

        public enum enMode
        {
            ADD,
            Ubdate
        }

        enMode Mode;

        public ClsBusinessPeople()
        {
            this.id = -1;
            this.firstname = "";
            this.Secondname = "";
            this.thirdname = "";
            this.lastname = "";
            this.email = "";
            this.phone = "";
            this.address = "";
            this.DateOfbirth = DateTime.Now;
            this.ImagePath = "";
            this.countryid = -1;
            this.Gendor = 0;
            this.countryName = "";

            Mode = enMode.ADD;
        }

        private ClsBusinessPeople(int id,string NationalNo, string firstname, string secondName, string thirdname, 
                                  string lastname,string email, string phone, string address, string countryName,
                                  DateTime DateOfbirth, int NationalityCountryID, byte Gender, string ImagePath)
        {
            this.id = id;
            this.NationalNo = NationalNo;
            this.firstname = firstname;
            this.Secondname = secondName;
            this.thirdname = thirdname;
            this.lastname = lastname;
            this.email = email;
            this.phone = phone;
            this.address = address;
            this.countryid = NationalityCountryID;
            this.DateOfbirth = DateOfbirth;
            this.ImagePath = ImagePath;
            this.Gendor = Gender;
            this.countryName = countryName;

            Mode = enMode.Ubdate;
        }

        private bool _AddNew()
        {
            this.id = ClsPeoplleData.AddNewRecored(this.NationalNo, this.firstname, this.Secondname, this.thirdname,
                      this.lastname, this.email,this.phone, this.address, this.DateOfbirth, this.countryid,
                      this.Gendor,this.ImagePath);

            return (this.id != -1);
        }

        private bool _Ubdate()
        {
            return ClsPeoplleData.UbdateRecored(this.id, this.NationalNo, this.firstname, this.Secondname, this.thirdname,
                      this.lastname, this.email, this.phone, this.address, this.DateOfbirth, this.countryid,
                      this.Gendor, this.ImagePath);
        }

        public static ClsBusinessPeople Find(int PersonID)
        {
            int id = -1;
            string NationalNo = "";
            string firstname = "", secondName = "", thirdname = "", lastname = "";
            string email = "";
            string phone= "";
            string address = "";
            DateTime DateOfbirth = new DateTime();
            int NationalityCountryID = -1;
            byte Gender = 2;
            string ImagePath = "";
            string countryName = "";


            if (ClsPeoplleData.GetrecoredByID(PersonID,ref id, ref NationalNo, ref firstname, ref secondName, ref thirdname,
                                              ref lastname, ref email, ref phone, ref address, ref DateOfbirth,
                                              ref NationalityCountryID, ref Gender, ref ImagePath,ref countryName))
            {
                return new ClsBusinessPeople(id, NationalNo, firstname, secondName, thirdname,
                                              lastname, email, phone, address, countryName, DateOfbirth,
                                              NationalityCountryID, Gender, ImagePath);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsBusinessPeople FindByNationalNo(string NationalNum)
        {
            int id = -1;
            string NationalNo = "";
            string firstname = "", secondName = "", thirdname = "", lastname = "";
            string email = "";
            string phone = "";
            string address = "";
            DateTime DateOfbirth = new DateTime();
            int NationalityCountryID = -1;
            byte Gender = 2;
            string ImagePath = "";
            string countryName = "";


            if (ClsPeoplleData.GetrecoredByNationalNo(NationalNum, ref id, ref NationalNo, ref firstname, ref secondName, 
                                                     ref thirdname, ref lastname, ref email, ref phone, ref address, 
                                                     ref DateOfbirth, ref NationalityCountryID, ref Gender, ref ImagePath,
                                                     ref countryName))
            {
                return new ClsBusinessPeople(id, NationalNo, firstname, secondName, thirdname,
                                              lastname, email, phone, address, countryName, DateOfbirth,
                                              NationalityCountryID, Gender, ImagePath);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static bool IsExists(int PersonID)
        {
            return ClsPeoplleData.ExistsIs(PersonID);
        }

        public static bool IsExists(string NationalNo)
        {
            return ClsPeoplleData.ExistsIs(NationalNo);
        }

        public static bool Delete(int PersonID)
        {
            return ClsPeoplleData.DeleteRecored(PersonID);
        }

        public static DataTable GetList()
        {
            return ClsPeoplleData.GetListRecoredsPeople();
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
    }
}