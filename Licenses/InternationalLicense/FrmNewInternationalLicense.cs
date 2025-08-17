using System;
using System.ComponentModel;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmNewInternationalLicense : Form
    {
        int _SelectInternationalLicenseiD = -1;

        public FrmNewInternationalLicense()
        {
            InitializeComponent();
        }

        private void FrmNewInternationalLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text =ClsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lbLfEES.Text = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedBy.Text = ClsGlobal.CurrentUser.UserID.ToString();
            lblExpirationDate.Text = ClsFormat.DateToShort(DateTime.Now.AddYears(1));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want Issue The License.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return;
            }

            ClsInternationalLicenses clsInternational = new ClsInternationalLicenses();
            clsInternational.DriverID = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.DriverID;
            clsInternational.IssuedUsingLocalLicenseID = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.ID;
            clsInternational.IssueDate = DateTime.Now;
            clsInternational.ExpirationDate = DateTime.Now.AddYears(1);
            clsInternational.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
            clsInternational.IsActive = true;

            clsInternational.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.Completed;
            clsInternational.ApplicationDate = DateTime.Now;
            clsInternational.LastStatusDate = DateTime.Now;
            clsInternational.PaidFees = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.NewInternationalLicense).Fees;
            clsInternational.ApplicationTypeID = (int)ClsApplicationBusiness.enApplicationType.NewInternationalLicense;
            clsInternational.PersonID = ctrlLicenseInfoWithFilter1.SelectLicenseInfo.clsDriver.PersonID;
            
            if (!clsInternational.AddNew())
            {
                MessageBox.Show("ADDIND Not SUCESSEFULY.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("ADDIND SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lnkLblShowLicenseInfo.Enabled = true;
            lblILApplicationID.Text = clsInternational.ApplicationID.ToString();
            LblinternationalLicenseID.Text = clsInternational.ID.ToString();

            ctrlLicenseInfoWithFilter1.FilterEnabeled = false;
            btnIssue.Enabled = false;
            _SelectInternationalLicenseiD = clsInternational.ID;
        }

        private void lnkLblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmIntrnationalDriverInfo frmIntrnationalDriverInfo = new FrmIntrnationalDriverInfo(_SelectInternationalLicenseiD);
            frmIntrnationalDriverInfo.ShowDialog();
        }

        private void lnkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.clsDriver.PersonID);
            frmLicenseHistory.ShowDialog();
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectLicenseiD = obj;

            lblLocalLicenseID.Text = SelectLicenseiD.ToString();

            lnkLblShowLicenseInfo.Enabled = (SelectLicenseiD != -1);
            llblLicenseHistory.Enabled = lnkLblShowLicenseInfo.Enabled;

            if (SelectLicenseiD == -1)
            {
                return;
            }

            if (ctrlLicenseInfoWithFilter1.SelectLicenseInfo.LicenseClassID!=3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int ActiveInterNationalLicenseID = ClsInternationalLicenses.GetActiveInternationalLicenseIDByDriverID(ctrlLicenseInfoWithFilter1.SelectLicenseInfo.
                                                                                                                  DriverID);

            if (ActiveInterNationalLicenseID!=-1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInterNationalLicenseID.ToString(), "Not allowed", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }

        private void FrmNewInternationalLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
    }
}