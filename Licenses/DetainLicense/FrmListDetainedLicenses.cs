using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Business;
using DVLD.Licenses.ReleaseLicense;

namespace DVLD
{
    public partial class FrmListDetainedLicenses : Form
    {
        public FrmListDetainedLicenses()
        {
            InitializeComponent();
        }

        DataTable DT = new DataTable();

        private void _RerfreshList()
        {
            dataGridView1.RowTemplate.Height = 36;

            cbFilter.SelectedIndex = 0;

            DT = ClsDetainedLicense.GetList();
            dataGridView1.DataSource = DT.DefaultView;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            if (DT.Rows.Count > 0)
            {
                dataGridView1.Columns[0].Width = 130;
                dataGridView1.Columns[1].Width = 150;
                dataGridView1.Columns[2].Width = 200;
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.Columns[4].Width = 200;
                dataGridView1.Columns[5].Width = 200;
                dataGridView1.Columns[6].Width = 150;
                dataGridView1.Columns[7].Width = 450;
                dataGridView1.Columns[8].Width = 199;
            }

            LBLRecoreds.Text = DT.Rows.Count.ToString();
        }

        private void FrmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _RerfreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetails personDetails = new PersonDetails((string)dataGridView1.CurrentRow.Cells[6].Value);
            personDetails.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo((int)dataGridView1.CurrentRow.Cells[1].Value);
            frmLicenseInfo.ShowDialog();
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory((string)dataGridView1.CurrentRow.Cells[6].Value);
            frmLicenseHistory.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReleaseDetainedLicense frmRelease = new FrmReleaseDetainedLicense((int)dataGridView1.CurrentRow.Cells[1].Value);
            frmRelease.ShowDialog();
            FrmListDetainedLicenses_Load(null, null);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "IsReleased")
            {
                txtFilter.Visible = false;
                cbFilterIsReleased.Visible = true;
                cbFilterIsReleased.Focus();
                cbFilterIsReleased.SelectedIndex = 0;
            }
            else
            {
                cbFilterIsReleased.Visible = false;
                txtFilter.Visible = (cbFilter.Text!="None");

                if (cbFilter.Text == "None")
                {
                    txtFilter.Enabled = false;
                    DT.DefaultView.RowFilter = "";
                    LBLRecoreds.Text = dataGridView1.RowCount.ToString();
                } 
                else
                    txtFilter.Enabled = true;

                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColoumn = cbFilter.Text;

            switch(cbFilter.Text)
            {
                case "[D.ID]":
                    FilterColoumn = "D.ID";
                    break;

                case "FullName":
                    FilterColoumn = "Full Name";
                    break;

                case "[N.No]":
                    FilterColoumn = "N.No";
                    break;

                case "[ReleaseApp.ID]":
                    FilterColoumn = "ReleaseApp.ID";
                    break;

                default:
                    FilterColoumn = "None";
                    break;

            }

            if (FilterColoumn == "None"||txtFilter.Text=="")
            {
                DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = dataGridView1.RowCount.ToString();
                return;
            }

            if (FilterColoumn == "D.ID" || FilterColoumn == "ReleaseApp.ID")
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColoumn, txtFilter.Text.Trim());
            else
                DT.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColoumn, txtFilter.Text.Trim());

            LBLRecoreds.Text = dataGridView1.RowCount.ToString();
        }

        private void cbFilterIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColoumn = "IsReleased";
            string FilterValue = cbFilterIsReleased.Text;

            switch (FilterValue)
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
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColoumn, FilterValue);

            LBLRecoreds.Text = dataGridView1.RowCount.ToString();
        }

        private void btnAddNewPeople_Click(object sender, EventArgs e)
        {
            FrmDetainLicense frmDetainLicense = new FrmDetainLicense();
            frmDetainLicense.ShowDialog();
            _RerfreshList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmReleaseDetainedLicense frmReleaseDetain = new FrmReleaseDetainedLicense();
            frmReleaseDetain.ShowDialog();
            _RerfreshList();
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "[D.ID]" || cbFilter.Text == "[ReleaseApp.ID]")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dataGridView1.CurrentRow.Cells[3].Value;
        }
    }
}