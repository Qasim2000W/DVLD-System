using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class CtrlTest : UserControl
    {
        ClsLocalDrivingLicenseApplicationBusiness _LocalDrivingLicenseApplication;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplication.ID; }
        }

        int _LicenseID = -1;
        string ClassName = string.Empty;

        public CtrlTest()
        {
            InitializeComponent();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            llShowLicenceInfo.Enabled = (_LicenseID != -1);

            LBLDLAppID.Text = _LocalDrivingLicenseApplication.ID.ToString();
            ClassName= ClsClassLicenseBusiness.GetRecored(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lBLAppliedForLicense.Text = ClassName;
            LBLPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTest().ToString()+"/3";
            ClsApplicationBusiness clsApplication = ClsApplicationBusiness.Find((int)_LocalDrivingLicenseApplication.ApplicationID);
            ctrlApplicationBasic1.CtrlApplicationBasic_Load(clsApplication);
        }

        public void GetFillDataByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplicationBusiness.Find(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        public void GetFillDataByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplicationBusiness.FindByApplicationID(ApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo(_LocalDrivingLicenseApplication.ID);
            frmLicenseInfo.ShowDialog();
        }
    }
}