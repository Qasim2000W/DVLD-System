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

namespace DVLD
{
    public partial class FrmIntrnationalDriverInfo : Form
    {
        int _InternationalID = -1;

        public FrmIntrnationalDriverInfo(int InternationalID)
        {
            InitializeComponent();
            _InternationalID = InternationalID;
        }

        private void FrmIntrnationalDriverInfo_Load(object sender, EventArgs e)
        {
            ctrlIntrnationalLicenseInfo1.LoadInfo(_InternationalID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}