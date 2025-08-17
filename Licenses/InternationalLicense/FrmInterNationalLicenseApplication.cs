using System;
using System.Data;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Forms;
using Business;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class FrmInterNationalLicenseApplication : Form
    {
        public FrmInterNationalLicenseApplication()
        {
            InitializeComponent();
        }

        DataTable DT = new DataTable();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmInterNationalLicenseApplication_Load(object sender, EventArgs e)
        {
            dataGridView1.RowTemplate.Height = 36;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            DT = ClsInternationalLicenses.GetList();
            dataGridView1.DataSource = DT.DefaultView;

            dataGridView1.Columns[0].Width = 240;
            dataGridView1.Columns[1].Width = 180;
            dataGridView1.Columns[2].Width = 280;
            dataGridView1.Columns[3].Width = 300;
            dataGridView1.Columns[4].Width = 229;
            dataGridView1.Columns[5].Width = 250;
            dataGridView1.Columns[6].Width = 189;

            LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text=="IsActive")
            {
                txtFilter.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.SelectedIndex = 0;
                cbIsReleased.Focus();
            }
            else
            {
                cbIsReleased.Visible = false;
                txtFilter.Enabled = (cbFilter.Text!="None");

                if (cbFilter.Text != "None")
                {
                    txtFilter.Visible = true;
                    DT.DefaultView.RowFilter = "";
                    LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
                }
                else
                {
                    txtFilter.Enabled = false;
                }

                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.Text)
            {
                case "InternationalLicenseID":
                    FilterColumn = "InternationalLicenseID";
                    break;

                case "ApplicationID":
                    FilterColumn = "ApplicationID";
                    break;

                case "DriverID":
                    FilterColumn = "DriverID";
                    break;

                case "[L.License ID]":
                    FilterColumn = "L.License ID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text==""||FilterColumn=="None")
            {
                DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
                return;
            }

            DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
        }

        private void btnAddNewPeople_Click(object sender, EventArgs e)
        {
            FrmNewInternationalLicense frmNewInternational = new FrmNewInternationalLicense();
            frmNewInternational.ShowDialog();
            FrmInterNationalLicenseApplication_Load(null, null);
        }

        private void showDetialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetails personDetails = new PersonDetails(ClsDrivers.GetRecoredByID((int)dataGridView1.CurrentRow.Cells[2].Value).PersonID);
            personDetails.ShowDialog();
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory(ClsDrivers.GetRecoredByID((int)dataGridView1.CurrentRow.Cells[2].Value).PersonID);
            frmLicenseHistory.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIntrnationalDriverInfo frmIntrnationalDriverInfo = new FrmIntrnationalDriverInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frmIntrnationalDriverInfo.ShowDialog();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsReleased.Text;

            switch(FilterValue)
            {
                case "All":
                    break;

                case "Yes":
                    FilterValue = "1";
                        break;

                case "No":
                    FilterValue = "0";
                    break;
            }
            if (FilterValue == "All")
                DT.DefaultView.RowFilter = "";
            else
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, FilterValue);

            LBLRecoreds.Text = dataGridView1.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }
    }
}