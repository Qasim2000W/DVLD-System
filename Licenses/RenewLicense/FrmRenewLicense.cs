using System;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmRenewLicense : Form
    {
        public FrmRenewLicense()
        {
            InitializeComponent();
        }

        private void FrmRenewLicense_Load(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();

            lblApplicationDate.Text = ClsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            LblApplicationFees.Text = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedBy.Text = ClsGlobal.CurrentUser.UserName;
            lblExpirationDate.Text = "???";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        ClsLicenses NewclsLicenses = new ClsLicenses();

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want Renew The License.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No)
            {
                return;
            }

            NewclsLicenses = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.RenewLicense(txtNotes.Text.Trim(), ClsGlobal.CurrentUser.UserID);
            
            if (NewclsLicenses!=null)
            {
                MessageBox.Show("Licensed Renewed SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblRLApplicationID.Text = NewclsLicenses.ApplicationID.ToString();
                LblRenewLicenseID.Text = NewclsLicenses.ID.ToString();
                lnkLblShowLicenseInfo.Enabled = true;
                btnRenew.Enabled = false;
                ctrlLicenseInfoWithFilter1.FilterEnabeled = true;
            }
            else
            {
                MessageBox.Show("Licensed Renewed Not SUCESSEFULY.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lnkLblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo(NewclsLicenses.ID);
            frmLicenseInfo.ShowDialog();
        }

        private void lnkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(NewclsLicenses.clsDriver.PersonID);
            frmLicenseHistory.ShowDialog();
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;

            lblOldLicenseID.Text = LicenseID.ToString();

            lnkLblShowLicenseInfo.Enabled = (LicenseID!=-1);

            if (LicenseID==-1)
            {
                return;
            }

            lblExpirationDate.Text = ClsFormat.DateToShort(DateTime.Now.AddYears(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.ClsLicenseClass.
                                                                                                                                          DefaultValidityLength));
            lblLicenseFees.Text = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.ClsLicenseClass.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(LblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.Notes;

            if (!ctrlLicenseInfoWithFilter1.SelectLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expiared, it will expire on: " + ClsFormat.DateToShort(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.
                                ExpirationDate), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license.","Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            btnRenew.Enabled = true;
        }

        private void FrmRenewLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
    }
}