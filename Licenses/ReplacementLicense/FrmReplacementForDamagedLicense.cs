using System;
using System.ComponentModel;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmReplacementForDamagedLicense : Form
    {
        ClsLicenses NewclsLicenses = new ClsLicenses();
        int _LicenseID = -1;

        public FrmReplacementForDamagedLicense()
        {
            InitializeComponent();
        }

        private byte _GetApplicationTypeID()
        {
            if (RBDamagedLicense.Checked)
                return (byte)ClsApplicationBusiness.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (byte)ClsApplicationBusiness.enApplicationType.ReplaceLostDrivingLicense;
        }

        private ClsLicenses.enIssueReason _GetenIssueReason()
        {
            if (RBDamagedLicense.Checked)
                return ClsLicenses.enIssueReason.DamagedReplacement;
            else
                return ClsLicenses.enIssueReason.LostReplacement;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReplacementForDamagedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text =ClsFormat.DateToShort(DateTime.Now);
            lblCreatedBy.Text = ClsGlobal.CurrentUser.UserID.ToString();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue Replacement for The License?.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != 
                DialogResult.Yes)
            {
                return;
            }

            NewclsLicenses = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.Replace(_GetenIssueReason(), ClsGlobal.CurrentUser.UserID);

            if (NewclsLicenses!=null)
            {
                MessageBox.Show("Licensed Replaced SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _LicenseID = NewclsLicenses.ID;
                lnkLblShowLicenseInfo.Enabled = true;
                btnIssue.Enabled = false;
                GBRBReplacement.Enabled = false;
                ctrlLicenseInfoWithFilter1.FilterEnabeled = false;
                lblLRApplicationID.Text = NewclsLicenses.ApplicationID.ToString();
                LblReplacedLicenseID.Text = NewclsLicenses.ID.ToString();
            }
            else
            {
                MessageBox.Show("Licensed Replaced Not SUCESSEFULY.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.clsDriver.clsPerson.id);
            frmLicenseHistory.ShowDialog();
        }

        private void lnkLblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo(NewclsLicenses.ID);
            frmLicenseInfo.ShowDialog();
        }

        private void RBDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lbLApplicationfEES.Text = ClsApplicationTypeBusiness.GetRecored(_GetApplicationTypeID()).Fees.ToString();
        }

        private void RBLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lbLApplicationfEES.Text = ClsApplicationTypeBusiness.GetRecored(_GetApplicationTypeID()).Fees.ToString();
        }

        private void FrmReplacementForDamagedLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lblOldLicenseID.Text = _LicenseID.ToString();
            lnkLblShowLicenseInfo.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1)
            {
                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }
    }
}