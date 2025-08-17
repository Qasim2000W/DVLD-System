using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmListApplicationTypes : Form
    {
        public FrmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmListApplicationTypes_Load(object sender, EventArgs e)
        {
            DGVApplication.RowTemplate.Height = 36;

            DGVApplication.ReadOnly = true;
            DGVApplication.AllowUserToAddRows = false;

            DGVApplication.DataSource = ClsApplicationTypeBusiness.GetListApplicationTypes().DefaultView;

            if (DGVApplication.RowCount>0)
            {
                DGVApplication.Columns[0].Width = 200;
                DGVApplication.Columns[1].Width = 540;
                DGVApplication.Columns[2].Width = 174;
            }

            LBLRecoreds.Text = DGVApplication.RowCount.ToString();
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUbdateApplicationType frmUbdateApplicationType = new FrmUbdateApplicationType((int)DGVApplication.CurrentRow.Cells[0].
                                                                                              Value);
            frmUbdateApplicationType.ShowDialog();
            FrmListApplicationTypes_Load(null, null);
        }
    }
}