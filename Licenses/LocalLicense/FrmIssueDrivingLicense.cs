using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmIssueDrivingLicense : Form
    {
        ClsLocalDrivingLicenseApplicationBusiness clsLocall = new ClsLocalDrivingLicenseApplicationBusiness();
        int LocalID = -1;
        
        public FrmIssueDrivingLicense(int IDLocal)
        {
            InitializeComponent();
            LocalID = IDLocal;
        }
        
        private void FrmIssueDrivingLicense_Load(object sender, EventArgs e)
        {
            clsLocall = ClsLocalDrivingLicenseApplicationBusiness.Find(LocalID);

            if (clsLocall == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + LocalID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!clsLocall.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = clsLocall.GetActiveLicenseID();

            if (LicenseID != -1)
            {
                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlDrivingLicenseApplicationInfo1.GetFillDataByLocalDrivingLicenseApplicationID(clsLocall.ID);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseID = clsLocall.IssueForTheFirstTime(txtNotes.Text.Trim(), ClsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show($"License Issued SUCESSEFULY With License ID = {LicenseID}", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("ADDING NOT SUCESSEFULY.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}