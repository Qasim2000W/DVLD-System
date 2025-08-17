using System;
using System.Data;
using System.Windows.Forms;
using Business;
using DVLD.Licenses.LocalLicense;

namespace DVLD
{
    public partial class FrmLocalDriving_LicenseApplication : Form
    {
        public FrmLocalDriving_LicenseApplication()
        {
            InitializeComponent();
        }

        DataTable DT = new DataTable();

        private void _RefreshList()
        {
            dataGridView1.RowTemplate.Height = 36;

            cbFilter.SelectedIndex = 0;

            DT = ClsLocalDrivingLicenseApplicationBusiness.GetList();
            dataGridView1.DataSource = DT;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            if (DT.Rows.Count>0)
            {
                dataGridView1.Columns[0].HeaderText = "L.D.L.AppID";
                dataGridView1.Columns[0].Width = 180;

                dataGridView1.Columns[1].HeaderText = "Driving Class";
                dataGridView1.Columns[1].Width = 400;

                dataGridView1.Columns[2].HeaderText = "National No.";
                dataGridView1.Columns[2].Width = 170;

                dataGridView1.Columns[3].HeaderText = "Full Name";
                dataGridView1.Columns[3].Width = 470;

                dataGridView1.Columns[4].HeaderText = "Application Date";
                dataGridView1.Columns[4].Width = 229;

                dataGridView1.Columns[5].HeaderText = "Passed Tests";
                dataGridView1.Columns[5].Width = 200;

                dataGridView1.Columns[6].Width = 180;
            }
            
            LBLRecoreds.Text = DT.Rows.Count.ToString();
        }

        private void FrmLocalDriving_LicenseApplication_Load(object sender, EventArgs e)
        {
            _RefreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text != "None");

            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }

            DT.DefaultView.RowFilter = "";
            LBLRecoreds.Text = DT.Rows.Count.ToString();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "L.D.L.AppID";
                    break;
                case "NationalNo":
                    FilterColumn = "NationalNo";
                    break;

                case "FullName":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text==""||FilterColumn== "None")
            {
                DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = DT.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "L.D.L.AppID")
                DT.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,txtFilter.Text);
            else
                DT.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text);

            LBLRecoreds.Text = DT.DefaultView.Count.ToString();
        }

        private void btnAddNewPeople_Click(object sender, EventArgs e)
        {
            FrmNewLocalDrivingLicenseApplication frmNLDLAPP = new FrmNewLocalDrivingLicenseApplication();
            frmNLDLAPP.ShowDialog();
            _RefreshList();
        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListTestAppointments frmVisionTestAppointment = new FrmListTestAppointments((int)dataGridView1.CurrentRow.Cells[0].Value, 
                                                                                                    ClsTestTypeBusiness.enTestType.VisionTest);
            frmVisionTestAppointment.ShowDialog();
            _RefreshList();
        }

        private void Written_Test_Click(object sender, EventArgs e)
        {
            FrmListTestAppointments frmVisionTestAppointment = new FrmListTestAppointments((int)dataGridView1.CurrentRow.Cells[0].Value,
                                                                                                     ClsTestTypeBusiness.enTestType.WrittenTest);
            frmVisionTestAppointment.ShowDialog();
            _RefreshList();
        }

        private void Sechdule_Street_Test_Click(object sender, EventArgs e)
        {
            FrmListTestAppointments frmListTestAppointments = new FrmListTestAppointments((int)dataGridView1.CurrentRow.Cells[0].Value, ClsTestTypeBusiness.
                                                                                         enTestType.StreetTest);
            frmListTestAppointments.ShowDialog();
            FrmLocalDriving_LicenseApplication_Load(null, null);
        }

        private void issueDrivingLicenseFirstMenuItem_Click(object sender, EventArgs e)
        {
            FrmIssueDrivingLicense frmIssueDriving = new FrmIssueDrivingLicense((int)dataGridView1.CurrentRow.Cells[0].Value);
            frmIssueDriving.ShowDialog();
            FrmLocalDriving_LicenseApplication_Load(null, null);
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ARE YOU SURE DO WANT TO CANCEL THIS APPLICATION?", "CONFIRM", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            ClsLocalDrivingLicenseApplicationBusiness clsLocalDrivingLicenseApplicationBusiness =
            ClsLocalDrivingLicenseApplicationBusiness.Find((int)dataGridView1.CurrentRow.Cells[0].Value);

            if (clsLocalDrivingLicenseApplicationBusiness!=null)
            {
                if (clsLocalDrivingLicenseApplicationBusiness.Cancel())
                {
                    MessageBox.Show("CANCELLED SUCESSEFULY.", "CANCELLED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmLocalDriving_LicenseApplication_Load(null,null);
                }
                else
                {
                    MessageBox.Show("CANCELLED NOT SUCESSEFULY.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = ClsLocalDrivingLicenseApplicationBusiness.Find((int)dataGridView1.CurrentRow.Cells[0].Value).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                FrmLicenseInfo frm = new FrmLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseHistory frmLicenseHistory = new FrmLicenseHistory((string)dataGridView1.CurrentRow.Cells[2].Value);
            frmLicenseHistory.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.
                                                                                                                                                               No)
                return;

            ClsLocalDrivingLicenseApplicationBusiness clsLocalDriving=ClsLocalDrivingLicenseApplicationBusiness.Find((int)dataGridView1.CurrentRow.Cells[0].Value);

            if (clsLocalDriving!=null)
            {
                if (clsLocalDriving.Delete())
                {
                    MessageBox.Show("Deleting SUCESSEFULY.", "Deleting", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FrmLocalDriving_LicenseApplication_Load(null, null);
                    return;
                }
                else
                {
                    MessageBox.Show("Deleting NOT SUCESSEFULY.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocalDrivingLicenseApplicationInfo frmLocalDrivingLicenseApplicationInfo = new FrmLocalDrivingLicenseApplicationInfo((int)dataGridView1.CurrentRow.
                                                                                                                                     Cells[0].Value);
            frmLocalDrivingLicenseApplicationInfo.ShowDialog();

            FrmLocalDriving_LicenseApplication_Load(null, null);
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewLocalDrivingLicenseApplication frmNewLDLApp = new FrmNewLocalDrivingLicenseApplication((int)dataGridView1.CurrentRow.Cells[0].Value);
            frmNewLDLApp.ShowDialog();
            FrmLocalDriving_LicenseApplication_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[6].Value.ToString() == "Canseled" ||
                dataGridView1.CurrentRow.Cells[6].Value.ToString() == "Completed")
            {
                SechduleTests.Enabled = false;
                CancelToolStripMenuItem.Enabled = false;
            }
            else
            {
                SechduleTests.Enabled = true;
                CancelToolStripMenuItem.Enabled = true;
            }

            if (dataGridView1.CurrentRow.Cells[6].Value.ToString() == "New" && (int)dataGridView1.CurrentRow.Cells[5].Value == 3)
            {
                SechduleTests.Enabled = false;
                issueDrivingLicenseFirstMenuItem.Enabled = true;
            }
            else
            {
                issueDrivingLicenseFirstMenuItem.Enabled = false;
            }


            if (dataGridView1.CurrentRow.Cells[6].Value.ToString() == "Completed")
            {
                showLicenseToolStripMenuItem.Enabled = true;
                editeToolStripMenuItem.Enabled = false;
                showPersonToolStripMenuItem.Enabled = true;
            }
            else
            {
                showLicenseToolStripMenuItem.Enabled = false;
                editeToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                showPersonToolStripMenuItem.Enabled = false;
            }

            if (dataGridView1.CurrentRow.Cells[6].Value.ToString() == "Completed" || (int)dataGridView1.CurrentRow.Cells[5].Value > 0)
            {
                deleteToolStripMenuItem.Enabled = false;
            }
            else
            {
                deleteToolStripMenuItem.Enabled = true;
            }

            if ((int)dataGridView1.CurrentRow.Cells[5].Value == 0)
            {
                Sechdule_Vision_Test.Enabled = true;
                Sechdule_Street_Test.Enabled = false;
                Written_Test.Enabled = false;
            }
            else if ((int)dataGridView1.CurrentRow.Cells[5].Value == 1)
            {
                Sechdule_Vision_Test.Enabled = false;
                Written_Test.Enabled = true;
                Sechdule_Street_Test.Enabled = false;
            }
            else if ((int)dataGridView1.CurrentRow.Cells[5].Value == 2)
            {
                Sechdule_Vision_Test.Enabled = false;
                Written_Test.Enabled = false;
                Sechdule_Street_Test.Enabled = true;
            }
        }
    }
}