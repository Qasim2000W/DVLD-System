using System;
using System.Windows.Forms;
using Business;
using DVLD.Properties;
using static Business.ClsTestTypeBusiness;

namespace DVLD
{
    public partial class FrmListTestAppointments : Form
    {
        int _LocalDrivingLicenseApplicationID = -1;
        ClsTestTypeBusiness.enTestType _TestTypeID;


        public FrmListTestAppointments(int LocalDrivingLicenseApplicationID, ClsTestTypeBusiness.enTestType TestTypeID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {
                case ClsTestTypeBusiness.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Vision 512.png";
                        break;
                    }

                case ClsTestTypeBusiness.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Written Test 512.png";
                        break;
                    }
                case ClsTestTypeBusiness.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\driving-test 512.png";
                        break;
                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmVision_Test_Appointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlTest1.GetFillDataByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            dataGridView1.RowTemplate.Height = 36;
            dataGridView1.DataSource = ClsAppointment.GetRecoredDT(_LocalDrivingLicenseApplicationID,(int)_TestTypeID);

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            if (dataGridView1.RowCount != 0)
            {
                dataGridView1.Columns[0].Width = 180;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 150;
                dataGridView1.Columns[3].Width = 120;
            }
        }

        private void btnAddNewPeople_Click(object sender, EventArgs e)
        {
            ClsLocalDrivingLicenseApplicationBusiness clsLocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplicationBusiness.Find
                                                                                                                            (_LocalDrivingLicenseApplicationID);

            if (clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            ClsTests LastTest = clsLocalDrivingLicenseApplication.FindLastTestForTestType(_TestTypeID);

            if (LastTest==null)
            {
                FrmTest frmTest = new FrmTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frmTest.ShowDialog();
                FrmVision_Test_Appointments_Load(null, null);
                return;
            }

            if (LastTest.TestResult==true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            FrmTest frmTest2 = new FrmTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frmTest2.ShowDialog();
            FrmVision_Test_Appointments_Load(null, null);
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTest frmTest = new FrmTest(_LocalDrivingLicenseApplicationID, _TestTypeID, (int)dataGridView1.CurrentRow.Cells[0].Value);
            frmTest.ShowDialog();
            FrmVision_Test_Appointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTakeTest frmTakeTest = new FrmTakeTest(_TestTypeID, (int)dataGridView1.CurrentRow.Cells[0].Value);
            frmTakeTest.ShowDialog();
            FrmVision_Test_Appointments_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
        }
    }
}