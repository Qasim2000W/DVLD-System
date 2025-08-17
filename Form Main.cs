using System;
using System.Data;
using System.Windows.Forms;
using Business;
using DVLD.Licenses.ReleaseLicense;

namespace DVLD
{
    public partial class Form1 : Form
    {
        FrmLogin _frmLogin;
        public Form1(FrmLogin frmLogin)
        {
            InitializeComponent();
            _frmLogin = frmLogin;
        }

        private void pEOPLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPeopleManagement formPeopleManagement = new FormPeopleManagement();
            formPeopleManagement.ShowDialog();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manage_Users FormManage_Users = new Manage_Users();
            FormManage_Users.ShowDialog();
        }

        private void saignOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_User_Info frm_User_Info = new Frm_User_Info(ClsGlobal.CurrentUser.UserID);
            frm_User_Info.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangePassword frmChangePassword = new FrmChangePassword(ClsGlobal.CurrentUser.UserID);
            frmChangePassword.ShowDialog();
        }

        private void manageApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListApplicationTypes FrmListApplicationTypes = new FrmListApplicationTypes();
            FrmListApplicationTypes.ShowDialog();
        }

        private void manageTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmManageTestType frmManageTestType = new FrmManageTestType();
            frmManageTestType.ShowDialog();
        }

        private void replacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReplacementForDamagedLicense frmReplacement = new FrmReplacementForDamagedLicense();
            frmReplacement.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewLocalDrivingLicenseApplication frmNLDLAPP = new FrmNewLocalDrivingLicenseApplication();
            frmNLDLAPP.ShowDialog();
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocalDriving_LicenseApplication frm = new FrmLocalDriving_LicenseApplication();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDrivers frmDrivers = new FrmDrivers();
            frmDrivers.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewInternationalLicense frmNewInternationalLicense = new FrmNewInternationalLicense();
            frmNewInternationalLicense.ShowDialog();
        }

        private void interNationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInterNationalLicenseApplication frmInterNational = new FrmInterNationalLicenseApplication();
            frmInterNational.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRenewLicense frmRenewLicense = new FrmRenewLicense();
            frmRenewLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocalDriving_LicenseApplication frmLocalDriving_License = new FrmLocalDriving_LicenseApplication();
            frmLocalDriving_License.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmDetainLicense frmDetainLicense = new FrmDetainLicense();
            frmDetainLicense.ShowDialog();
        }

        private void releaseDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReleaseDetainedLicense frmReleaseDetain = new FrmReleaseDetainedLicense();
            frmReleaseDetain.ShowDialog();
        }

        private void manageDetainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListDetainedLicenses frmListDetained = new FrmListDetainedLicenses();
            frmListDetained.ShowDialog();
        }

        private void releaseDetianedDriviToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReleaseDetainedLicense frmReleaseDetain = new FrmReleaseDetainedLicense();
            frmReleaseDetain.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ClsGlobal.CurrentUser != null)
                _frmLogin.Close();
        }
    }
}