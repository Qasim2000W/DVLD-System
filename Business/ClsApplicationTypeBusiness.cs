using System.Data;
using ClsDataAccess;

namespace Business
{
    public class ClsApplicationTypeBusiness
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public decimal Fees { get; set; }

        public enum EnMODE
        {
            ADD,
            UBDATE
        }

        EnMODE _MODE;

        public ClsApplicationTypeBusiness()
        {
            ID = -1;
            Title = "";
            Fees = 0;

            _MODE = EnMODE.ADD;
        }

        ClsApplicationTypeBusiness(int ID, string Title, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;

            _MODE = EnMODE.UBDATE;
        }

        private bool _Update()
        {
            return ClsApplicationTypes.UbdateRecored(this.ID,this.Title,this.Fees);
        }

        public static DataTable GetListApplicationTypes()
        {
            return ClsApplicationTypes.GetListApplication();
        }

        static public ClsApplicationTypeBusiness GetRecored(int ID)
        {
            string Title = "";
            decimal Fees = 0;

            if (ClsApplicationTypes.GetrecoredByID(ID, ref Title, ref Fees))
                return new ClsApplicationTypeBusiness(ID, Title, Fees);
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning); 
                return null;
            }
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