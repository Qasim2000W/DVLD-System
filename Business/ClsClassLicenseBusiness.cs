using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsClassLicenseBusiness
    {
        public int ID { get; set; }

        public string ClassName { get; set; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public decimal ClassFees { get; set; }

        public ClsClassLicenseBusiness()
        {
            this.ID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
        }

        public ClsClassLicenseBusiness(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength,
                                       decimal ClassFees)
        {
            this.ID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static ClsClassLicenseBusiness GetRecored(string ClassName)
        {
            int ID = -1;
            decimal ClassFees = 0;
            string ClassDescription = "";
            byte MinimumAllowedAge = 18; 
            byte DefaultValidityLength = 10;

            if (ClsLicenseClassData.GetRecored(ClassName, ref ID, ref ClassFees, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength))
            {
                return new ClsClassLicenseBusiness(ID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static ClsClassLicenseBusiness GetRecored(int ID)
        {
            string ClassName = ""; 
            string ClassDescription = "";
            byte MinimumAllowedAge = 18; 
            byte DefaultValidityLength = 10; 
            decimal ClassFees = 0;

            if (ClsLicenseClassData.GetRecored(ID, ref ClassName, ref ClassFees, ref  ClassDescription, ref  MinimumAllowedAge, ref  DefaultValidityLength))
            {
                return new ClsClassLicenseBusiness(ID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetList()
        {
            return ClsLicenseClassData.GetList();
        }
    }
}