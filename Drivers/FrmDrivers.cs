using System;
using System.Data;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmDrivers : Form
    {
        public FrmDrivers()
        {
            InitializeComponent();
        }

        DataTable DT;

        private void FrmDrivers_Load(object sender, EventArgs e)
        {
            dataGridView1.RowTemplate.Height = 36;

            cbFilter.SelectedIndex = 0;

            DT = ClsDrivers.GetList();
            dataGridView1.DataSource = DT;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].Width = 110;
                dataGridView1.Columns[1].Width = 110;
                dataGridView1.Columns[2].Width = 155;
                dataGridView1.Columns[3].Width = 468;
                dataGridView1.Columns[4].Width = 160;
                dataGridView1.Columns[5].Width = 180;
            }

            LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text!="None");

            if (cbFilter.Text=="None")
            {
                txtFilter.Enabled = false;
            }
            else
                txtFilter.Enabled = true;

            txtFilter.Text = "";
            txtFilter.Focus();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            
            switch (cbFilter.Text)
            {
                case "DriverID":
                    FilterColumn = "DriverID";
                    break;
                case "PersonID":
                    FilterColumn = "PersonID";
                    break;
                case "NationalNo":
                    FilterColumn = "NationalNo";
                    break;
                case "FullName":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text==""||cbFilter.Text=="None")
            {
                DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            else
                DT.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", FilterColumn, txtFilter.Text.Trim());

            LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetails FRMpersonDetails = new PersonDetails((int)dataGridView1.CurrentRow.Cells[1].Value);
            FRMpersonDetails.ShowDialog();
            FrmDrivers_Load(null, null);
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
       {
            if (cbFilter.Text == "DriverID" || cbFilter.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory((int)dataGridView1.CurrentRow.Cells[1].Value);
            frmLicenseHistory.ShowDialog();
            FrmDrivers_Load(null, null);
        }
    }
}