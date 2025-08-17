using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class CTRLTakeTest : UserControl
    {
        public CTRLTakeTest()
        {
            InitializeComponent();
        }

        ClsLocalDrivingLicenseApplicationBusiness LocalLic;
        ClsAppointment _ClsAppointment;
        ClsTestTypeBusiness.enTestType _TestTypeID;
        int _TestID = -1;

        public ClsTestTypeBusiness.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case ClsTestTypeBusiness.enTestType.VisionTest:
                        groupBox1.Text = "Vision Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Vision 512.png";
                        break;
                    case ClsTestTypeBusiness.enTestType.WrittenTest:
                        groupBox1.Text = "Written Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Written Test 512.png";
                        break;
                    case ClsTestTypeBusiness.enTestType.StreetTest:
                        groupBox1.Text = "Street Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\driving-test 512.png";
                        break;
                }
            }
        }

        public int TestID
        {
            get
            {
                return _TestID;
            }
        }

        public void GitFillData(ClsAppointment clsAppointment)
        {
            _ClsAppointment = clsAppointment;
            LocalLic = ClsLocalDrivingLicenseApplicationBusiness.Find(_ClsAppointment.LocalDrivingLicenseApplicationID);
        }

        public void AppeiranceData()
        {
            _TestID = _ClsAppointment.TestID;

            LBLDLAppID.Text = LocalLic.ID.ToString();
            lBLclass.Text = LocalLic.LicenseClassInfo.ClassName;
            LBlName.Text = LocalLic.FullName;
            lbLfEES.Text = _ClsAppointment.PaidFees.ToString();
            lbltrail.Text = LocalLic.GetTotalTrail(_TestTypeID).ToString();
            lblTestID.Text=(_ClsAppointment.TestID==-1)?"Not Taken Yet": _ClsAppointment.TestID.ToString();
            lblDate.Text = _ClsAppointment.AppointmentDate.ToShortDateString();
        }
    }
}