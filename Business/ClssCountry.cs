using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClssCountry
    {
        public int countryid { get; set; }
        public string countryname { get; set; }

        ClssCountry(int countryid, string countryname)
        {
            this.countryid = countryid;
            this.countryname = countryname;
        }

        public static ClssCountry FindByName(string countryname)
        {
            int countryid = -1;
            string Countryname = "";

            if (ClssDataAccessCountry.GetCountryByName(countryname, ref Countryname, ref countryid))
                return new ClssCountry(countryid, countryname);
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static DataTable GetList()
        {
            return ClssDataAccessCountry.GetListRecoreds();
        }

    }
}