using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Applications;

namespace DVLD.Licenses.LocalLicense
{
    public partial class FrmLocalDrivingLicenseApplicationInfo : Form
    {
        int _LocalDrivingLicenseID = -1;

        public FrmLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseID)
        {
            InitializeComponent();
            _LocalDrivingLicenseID = LocalDrivingLicenseID;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlTest1.GetFillDataByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseID);
        }
    }
}