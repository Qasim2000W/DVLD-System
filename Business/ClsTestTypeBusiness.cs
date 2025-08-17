using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsTestTypeBusiness
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public enTestType ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Fees { get; set; }

        public enum EnMODE
        {
            ADD,
            UBDATE
        }

        EnMODE _MODE;

        public ClsTestTypeBusiness()
        {
            ID = (enTestType)1;
            Title = "";
            Description = "";
            Fees = 0;

            _MODE = EnMODE.ADD;
        }

        ClsTestTypeBusiness(enTestType ID, string Title, string Description, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
            this.Description = Description;

            _MODE = EnMODE.UBDATE;
        }

        private bool _Update()
        {
            return ClsTestTypeData.UbdateRecored((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public static DataTable GetListTestTypes()
        {
            return ClsTestTypeData.GetList();
        }

        static public ClsTestTypeBusiness GetRecored(enTestType ID)
        {
            string Title = "";
            string Description = "";
            decimal Fees = 0;

            if (ClsTestTypeData.GetrecoredByID((int)ID, ref Title, ref Description, ref Fees))
                return new ClsTestTypeBusiness(ID, Title,  Description, Fees);
            else
                return null;
        }


        public bool SAVE()
        {
            switch (_MODE)
            {
                case EnMODE.ADD:
                    return true;

                case EnMODE.UBDATE:
                    return _Update();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }
    }
}