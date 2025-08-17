using System;
using System.ComponentModel;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FormAddNewUser : Form
    {
        int UserID = -1;

        public FormAddNewUser()
        {
            InitializeComponent();
            _Mode = ENMode.ADD;
        }

        public FormAddNewUser(int UserId)
        {
            InitializeComponent();
            UserID = UserId;
            _Mode = ENMode.UBDATE;
        }

        ClsUsersBussiness User;
        public enum ENMode
        {
            ADD,
            UBDATE
        }

        public ENMode _Mode;

        private void _ResetDefaultValues()
        {
            if (_Mode==ENMode.ADD)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                tPLogin.Enabled = false;
                User = new ClsUsersBussiness();
            }
            else
            {
                lblTitle.Text = "Ubdate User";
                this.Text = "Ubdate User";
                btnSave.Enabled = true;
                tPLogin.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_Mode==ENMode.UBDATE)
            {
                btnSave.Enabled = true;
                tPLogin.Enabled = true;
                tabControl1.SelectedIndex = 1;

                return;
            }

            if (ctrlPersonDetailsWithFilter1.PersonID!=-1)
            {
                if (ClsUsersBussiness.IsExistsPersonID(ctrlPersonDetailsWithFilter1.PersonID))
                {
                    MessageBox.Show("Salect Person Already Has a user, Choice Another One", "Salect Another Person", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    ctrlPersonDetailsWithFilter1.FilterFocus();
                    return;
                }
                else
                {
                    btnSave.Enabled = true;
                    tPLogin.Enabled = true;
                    tabControl1.SelectedIndex = 1;
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
        }

        ClsBusinessPeople person = new ClsBusinessPeople();

        private void txtxConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtxConfirmPassword.Text!= txtxPassword.Text)
            {
                e.Cancel = true;
                txtxConfirmPassword.Focus();
                errorProvider1.SetIconAlignment(txtxConfirmPassword, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtxConfirmPassword, "Passward Confirmation Does Not Match Passward.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtxConfirmPassword, "");
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                e.Cancel = true;
                txtUserName.Focus();
                errorProvider1.SetIconAlignment(txtUserName, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtUserName, "please write your User name!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }


            if (_Mode==ENMode.ADD)
            {
                if (ClsUsersBussiness.IsExists(txtUserName.Text))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtUserName, "");
                }
            }
            else
            {
                if (User.UserName!=txtUserName.Text)
                {
                    if (ClsUsersBussiness.IsExists(txtUserName.Text))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                        return;
                    }
                    else
                    {
                        e.Cancel = false;
                        errorProvider1.SetError(txtUserName, "");
                    }
                }
            }
        }

        private void txtxPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtxPassword.Text))
            {
                e.Cancel = true;
                txtxPassword.Focus();
                errorProvider1.SetIconAlignment(txtxPassword, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtxPassword, "please write your User name!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtxPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           // btnSave.CausesValidation = false;
           // this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            
            if (!this.ValidateChildren(ValidationConstraints.Enabled | ValidationConstraints.Visible))
            {
                MessageBox.Show("Some Fields Are NOT Valide!.", "WORNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User.PersonID = ctrlPersonDetailsWithFilter1.PersonID;
            User.UserName = txtUserName.Text;
            User.Password = ClsSecurity.Hashing(txtxPassword.Text);
            User.IsActive = chBoxIsActive.Checked;

            if (User.Save())
            {
                if (_Mode==ENMode.ADD)
                {
                    MessageBox.Show("ADDIND SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _Mode = ENMode.UBDATE;
                    lblUserId.Text = User.UserID.ToString();
                    lblTitle.Text = "Ubdate User";
                    this.Text= "Ubdate User";
                }
                else
                {
                    MessageBox.Show("Udbating SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (_Mode == ENMode.ADD)
                {
                    MessageBox.Show("ADDIND Not SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Udbating Not SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FormAddNewUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode==ENMode.UBDATE)
            {
                User = ClsUsersBussiness.Find(UserID);
                person = ClsBusinessPeople.Find(User.PersonID);
                ctrlPersonDetailsWithFilter1.EnabledFilter = false;
                ctrlPersonDetailsWithFilter1.GetPerson(person);
                txtUserName.Text = User.UserName;
                txtxPassword.Text = User.Password;
                txtxConfirmPassword.Text = User.Password;
                chBoxIsActive.Checked = User.IsActive;
                lblUserId.Text = User.UserID.ToString();
            }
        }
    }
}