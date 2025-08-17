using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmLicenseInfo : Form
    {
        int _LicenseID = -1;

        public FrmLicenseInfo(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }
      
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlInfoLicense1.Appeareance(_LicenseID);
        }
    }
}