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

namespace DVLD
{
    public partial class FrmDetainLicense : Form
    {
        int _Detain = -1;
        int _SelectLicenseID = -1;

        public FrmDetainLicense()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToString();
            lblCreatedBy.Text = ClsGlobal.CurrentUser.UserID.ToString();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Detain This License?.", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _Detain=ctrlLicenseInfoWithFilter1.SelectLicenseInfo.Detain(Convert.ToDecimal(txtFineFees.Text), ClsGlobal.CurrentUser.UserID);

            if (_Detain==-1)
            {
                MessageBox.Show("License Not Detained SUCESSEFULY.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainID.Text = _Detain.ToString();
            MessageBox.Show($"License Detained SUCESSEFULY With ID = {_Detain} .", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            lnkLblShowLicenseInfo.Enabled = true;
            btnDetain.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabeled = false;
            txtFineFees.Enabled = false;
        }


        private void lnkShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.clsDriver.PersonID);
            frmLicenseHistory.ShowDialog();
        }

        private void lnkLblShowLicenseInfo_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo(_SelectLicenseID);
            frmLicenseInfo.ShowDialog();
        }

        private void txtFineFees_Validating_1(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            };

            if (!ClsValidition.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid, Enter Number.");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            };
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected_1(int obj)
        {
            _SelectLicenseID = obj;

            lnkLblShowLicenseInfo.Enabled = (_SelectLicenseID != -1);
            lblLicenseID.Text = _SelectLicenseID.ToString();

            if (_SelectLicenseID == -1)
            {
                return;
            }

            if (ctrlLicenseInfoWithFilter1.SelectLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License i already detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDetain.Enabled = true;
        }
    }
}