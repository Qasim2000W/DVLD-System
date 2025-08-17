using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmTest : Form
    {
        int _LocalDrivingLicenseApplicationID = -1;
        int _AppointmentID = -1;
        ClsTestTypeBusiness.enTestType _TestTypeID;

        public FrmTest(int LocalDrivingLicenseApplicationID, ClsTestTypeBusiness.enTestType TestTypeID, int AppointmentID= -1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _AppointmentID = AppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
        private void FrmVisionTest_Load(object sender, EventArgs e)
        {
            ctrlSechduleTest1.TestTypeID = _TestTypeID;
            ctrlSechduleTest1.GitFillData(_LocalDrivingLicenseApplicationID,_AppointmentID);
        }
    }
}