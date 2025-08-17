using System;
using System.Data;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmNewLocalDrivingLicenseApplication : Form
    {
        int _PersonID=-1;
        ClsLocalDrivingLicenseApplicationBusiness LDLAPP = new ClsLocalDrivingLicenseApplicationBusiness();
        decimal PaidFees=0;
        int _LocalalDrivingLicenseApplicationID=-1;

        public enum EnMode
        {
            Add,
            Ubdate
        }

        EnMode Mode;

        public FrmNewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            Mode = EnMode.Add;
        }

        public FrmNewLocalDrivingLicenseApplication(int LocalalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalalDrivingLicenseApplicationID = LocalalDrivingLicenseApplicationID;
            Mode = EnMode.Ubdate;
        }

        private void FillcbLicenseClass()
        {
            foreach (DataRow row in ClsClassLicenseBusiness.GetList().Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void LoadData()
        {
            LDLAPP = ClsLocalDrivingLicenseApplicationBusiness.Find(_LocalalDrivingLicenseApplicationID);

            if (LDLAPP == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, 
                                 MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrlPersonDetailsWithFilter1.EnabledFilter = false;
            ClsBusinessPeople clsPerson = ClsBusinessPeople.Find(LDLAPP.PersonID);
            ctrlPersonDetailsWithFilter1.GetPerson(clsPerson);
            lblDLApplicationId.Text = LDLAPP.ID.ToString();
            lbldate.Text = LDLAPP.ApplicationDate.ToShortDateString();
            cbLicenseClass.SelectedIndex = LDLAPP.LicenseClassID;
            lblFees.Text = LDLAPP.PaidFees.ToString();
            PaidFees = LDLAPP.PaidFees;
            lblUserName.Text = ClsUsersBussiness.Find(LDLAPP.CreatedByUserID).UserName;
        }

        private void _ResetValues()
        {
            FillcbLicenseClass();

            if (Mode==EnMode.Add)
            {
                PaidFees = ClsApplicationTypeBusiness.GetRecored((int)ClsApplicationBusiness.enApplicationType.NewDrivingLicense).
                           Fees;
                lblFees.Text = PaidFees.ToString();
                lblUserName.Text = ClsGlobal.CurrentUser.UserName;
                lbldate.Text = DateTime.Now.ToShortDateString();
                tabPage2.Enabled = false;
                cbLicenseClass.SelectedIndex = 2;
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
            }
            else
            {
                lblTitle.Text = "Ubdate Local Driving License Application";
                this.Text = "Ubdate Local Driving License Application";
                tabPage2.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void FrmNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetValues();

            if (Mode == EnMode.Ubdate)
                LoadData();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Mode==EnMode.Ubdate)
            {
                tabPage2.Enabled = true;
                btnSave.Enabled = true;
                tabControl1.SelectedIndex = 1;
                return;
            }

            if (ctrlPersonDetailsWithFilter1.PersonID!=-1)
            {
                tabPage2.Enabled = true;
                btnSave.Enabled = true;
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = ClsClassLicenseBusiness.GetRecored(cbLicenseClass.Text).ID;

            int? ActiveApplicationID = ClsApplicationBusiness.GetActiveApplicationID(_PersonID, LicenseClassID, ClsApplicationBusiness.
                                     EnApplicationStatus.New);

            if (ActiveApplicationID != null)
            {
                MessageBox.Show("Choice Another License Class, The Selected Person Already Have An Active Application For The " +
                             "Selected Class With Id = " + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ClsLicenses.IsLicenseIDExistsByPersonID(_PersonID, LicenseClassID))
            {
                MessageBox.Show("Person Already Have a License With Same Applied Driving Class, Choose Different Driving Class.", 
                               "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            LDLAPP.ApplicationStatus = ClsApplicationBusiness.EnApplicationStatus.New;
            LDLAPP.LicenseClassID = LicenseClassID;
            LDLAPP.ApplicationDate = DateTime.Now;
            LDLAPP.LastStatusDate = DateTime.Now;
            LDLAPP.ApplicationTypeID = 1;
            LDLAPP.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
            LDLAPP.PaidFees = PaidFees;
            LDLAPP.PersonID = _PersonID;

            if (LDLAPP.Save())
            {
                MessageBox.Show("Saving SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblTitle.Text = "Ubdate Local Driving License Application";
                lblDLApplicationId.Text = LDLAPP.ID.ToString();
            }
            else
                MessageBox.Show("Saving NOT SUCESSEFULY.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
        }

        private void FrmNewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonDetailsWithFilter1.FilterFocus();
        }
    }
}