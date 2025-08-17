using System;
using System.Windows.Forms;
using Business;
using static Business.ClsTestTypeBusiness;

namespace DVLD
{
    public partial class CtrlSechduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;
        ClsLocalDrivingLicenseApplicationBusiness _ClsLocalDrivingLicenseApplication;
        ClsTestTypeBusiness.enTestType _TestTypeID;
        ClsAppointment _ClsAppointment;
        int _LocalDrivingLicenseApplicationID = -1;
        int _TestAppointmentID = -1;

        public ClsTestTypeBusiness.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case ClsTestTypeBusiness.enTestType.VisionTest:
                        groupBox1.Text = "Vision Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Vision 512.png";
                        break;
                    case ClsTestTypeBusiness.enTestType.WrittenTest:
                        groupBox1.Text = "Written Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\Written Test 512.png";
                        break;
                    case ClsTestTypeBusiness.enTestType.StreetTest:
                        groupBox1.Text = "Street Test";
                        pBTitle.ImageLocation = "C:\\Users\\PC\\Downloads\\Icons\\Icons\\driving-test 512.png";
                        break;
                }
            }
        }

        public CtrlSechduleTest()
        {
            InitializeComponent();
        }

        public void GitFillData(int LocalDrivingLicenseApplicationID, int AppoinmentID=-1)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppoinmentID;

            if (_TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _ClsLocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplicationBusiness.Find(_LocalDrivingLicenseApplicationID);

            if (_ClsLocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(), "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;
                return;
            }

            if (_ClsLocalDrivingLicenseApplication.DoesThisTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if (_CreationMode==enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.RetakeTest).Fees.ToString();
                gBRetakeTest.Enabled = true;
                lblRTAppID.Text = "0";
                lblMesaage.Text = "Schdule Retak Test.";
            }
            else
            {
                lblRAppFees.Text = "0"; ;
                gBRetakeTest.Enabled = false;
                lblRTAppID.Text = "N/A";
            }

            lbltrail.Text = _ClsLocalDrivingLicenseApplication.GetTotalTrail(_TestTypeID).ToString();

            if (_Mode==enMode.AddNew)
            {
                dateTimePicker1.MinDate = DateTime.Now;
                lbLfEES.Text = ClsTestTypeBusiness.GetRecored(_TestTypeID).Fees.ToString();
                lblRTAppID.Text = "N/A"; ;
                _ClsAppointment = new ClsAppointment();
            }
            else
            {
                if (!_LoadTestAppoiment())
                    return;
            }

            lblTotoalFees.Text = (Convert.ToSingle(lblRAppFees.Text)+ Convert.ToSingle(lbLfEES.Text)).ToString();
            lBLclass.Text = _ClsLocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            LBlName.Text = _ClsLocalDrivingLicenseApplication.FullName;

            if (!_HandleActiveTestAppointment())
                return;

            if (!_HandleAppointmentLooked())
                return;
        }

        private bool _LoadTestAppoiment()
        {
            _ClsAppointment = ClsAppointment.GetRecored(_TestAppointmentID);

            if (_ClsAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lbLfEES.Text = _ClsAppointment.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _ClsAppointment.AppointmentDate) < 0)
                dateTimePicker1.MinDate = DateTime.Now;
            else
                dateTimePicker1.MinDate = _ClsAppointment.AppointmentDate;

            dateTimePicker1.Value = _ClsAppointment.AppointmentDate;
            LBLDLAppID.Text = _ClsLocalDrivingLicenseApplication.ID.ToString();

            if (_ClsAppointment.RetakeTestApplicationID==-1)
            {
                lblRAppFees.Text = "0";
                lblRTAppID.Text = "N/A";
            }
            else
            {
                lblRAppFees.Text = _ClsAppointment.RetakeTestAppInfo.PaidFees.ToString();
                lblRTAppID.Text = _ClsAppointment.RetakeTestApplicationID.ToString();
                lblMesaage.Text = "Schdule Retak Test.";
                gBRetakeTest.Enabled = true;
            }

            return true;
        }

        private bool _HandleActiveTestAppointment()
        {
            if (_Mode==enMode.AddNew&&ClsLocalDrivingLicenseApplicationBusiness.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID,_TestTypeID))
            {
                lblMesaage.Text = "Person Already have an active appointment for this test.";
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }

            return true;
        }

        private bool _HandleAppointmentLooked()
        {
            if (_ClsAppointment.IsLocked)
            {
                lblMesaage.Text = "Person already sat for the test, appointment loacked.";
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
           
            return true;
        }

        private bool _HandleRetakeApplication()
        {
            if (_Mode==enMode.AddNew&&_CreationMode==enCreationMode.RetakeTestSchedule)
            {
                ClsApplicationBusiness clsApplication = new ClsApplicationBusiness();

                clsApplication.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.Completed;
                clsApplication.ApplicationDate = DateTime.Now;
                clsApplication.LastStatusDate = DateTime.Now;
                clsApplication.ApplicationTypeID =(int)ClsApplicationBusiness.enApplicationType.RetakeTest;
                clsApplication.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
                clsApplication.PaidFees = ClsApplicationTypeBusiness.GetRecored((int)_TestTypeID).Fees;
                clsApplication.PersonID = _ClsLocalDrivingLicenseApplication.PersonID;

                if (!clsApplication.Save())
                {
                    _ClsAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _ClsAppointment.RetakeTestApplicationID = (int)clsApplication.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _ClsAppointment.LocalDrivingLicenseApplicationID = _ClsLocalDrivingLicenseApplication.ID;
            _ClsAppointment.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
            _ClsAppointment.AppointmentDate = dateTimePicker1.Value;
            _ClsAppointment.TestTypeID = _TestTypeID;
            _ClsAppointment.PaidFees = Convert.ToDecimal(lbLfEES.Text);

            if (_ClsAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}