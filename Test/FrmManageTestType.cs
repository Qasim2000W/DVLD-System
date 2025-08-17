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
    public partial class FrmManageTestType : Form
    {
        public FrmManageTestType()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmManageTestType_Load(object sender, EventArgs e)
        {
            DGVTestType.RowTemplate.Height = 36;

            DGVTestType.ReadOnly = true;
            DGVTestType.AllowUserToAddRows = false;

            DGVTestType.DataSource = ClsTestTypeBusiness.GetListTestTypes().DefaultView;

            if (DGVTestType.RowCount>0)
            {
                DGVTestType.Columns[0].Width = 140;
                DGVTestType.Columns[1].Width = 250;
                DGVTestType.Columns[2].Width = 540;
                DGVTestType.Columns[3].Width = 174;
            }
            
            LBLRecoreds.Text = DGVTestType.RowCount.ToString();
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUbdateTestType frmUbdateTestType = new FrmUbdateTestType((int)DGVTestType.CurrentRow.Cells[0].Value);
            frmUbdateTestType.ShowDialog();
            FrmManageTestType_Load(null,null);
        }
    }
}