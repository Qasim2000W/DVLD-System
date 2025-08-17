using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD.Licenses.ReleaseLicense
{
    public partial class FrmReleaseDetainedLicense : Form
    {
        int _LicenseID = -1;

        public FrmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public FrmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            ctrlLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            ctrlLicenseInfoWithFilter1.FilterEnabeled = false;
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;

            llShowLicenseHistory.Enabled = (_LicenseID != -1);
            llShowLicenseInfo.Enabled = llShowLicenseHistory.Enabled;

            if (_LicenseID==-1)
            {
                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License i is not detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationFees.Text = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.
                                      ToString();
            lblCreatedByUser.Text = ClsGlobal.CurrentUser.UserName;
            lblDetainID.Text = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.DetainedInfo.DetainID.ToString();
            lblLicenseID.Text = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.ID.ToString();
            lblDetainDate.Text = ClsFormat.DateToShort(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.DetainedInfo.DetainDate);
            lblFineFees.Text = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text= (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained  license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;

            bool IsRelease= ctrlLicenseInfoWithFilter1.SelectLicenseInfo.ReleaseDetainedLicense(ClsGlobal.CurrentUser.UserID, ref ApplicationID);

            lblApplicationID.Text = ApplicationID.ToString();

            if (!IsRelease)
            {
                MessageBox.Show("Faild to to release the Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Detained License released Successfully ", "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnRelease.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabeled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void FrmReleaseDetainedLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo(_LicenseID);
            frmLicenseInfo.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.clsDriver.PersonID);
            frmLicenseHistory.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}