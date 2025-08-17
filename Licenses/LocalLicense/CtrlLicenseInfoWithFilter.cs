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

namespace DVLD.Licenses.LocalLicense
{
    public partial class CtrlLicenseInfoWithFilter : UserControl
    {
        int _LicenseID = -1;

        public ClsLicenses SelectLicenseInfo
        { get { return ctrlInfoLicense1.SelectedLicenseInfo; } }

        public bool _FilterEnabeled = true;

        public bool FilterEnabeled
        {
            get
            {
                return _FilterEnabeled;
            }
            set
            {
                _FilterEnabeled = value;
                gbFilters.Enabled = _FilterEnabeled;
            }
        }

        public CtrlLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseIDSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;

            if (handler != null)
            {
                handler(LicenseID); 
            }
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text = LicenseID.ToString();
            ctrlInfoLicense1.Appeareance(LicenseID);
            _LicenseID = ctrlInfoLicense1.SelectLicenseID;
            if (OnLicenseSelected != null)
                LicenseIDSelected(_LicenseID);
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar)&&!char.IsDigit(e.KeyChar);

            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                return;
            }

            LoadLicenseInfo(int.Parse(txtLicenseID.Text));
        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }
    }
}
