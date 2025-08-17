using System;
using System.ComponentModel;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmChangePassword : Form
    {
        ClsUsersBussiness User = new ClsUsersBussiness();

        public FrmChangePassword(int UserId)
        {
            InitializeComponent();
            User = ClsUsersBussiness.Find(UserId);
        }

        private void FrmChangePassword_Load(object sender, EventArgs e)
        {
            if (User == null)
            {
                MessageBox.Show("Not find this User.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClsBusinessPeople Person = ClsBusinessPeople.Find(User.PersonID);
            ctrlUserDetail1.CtrlUserDetail_Load(User, Person);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro","Validation Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ClsUsersBussiness.ChangePassword(User.UserID, ClsSecurity.Hashing(txtxNewPassword.Text)))
            {
                MessageBox.Show("Password Changed SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Password Not Changed SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtCurrentPassword.Text!=User.Password)
            {
                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetIconAlignment(txtCurrentPassword, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtCurrentPassword, "Current Password Is Wrong!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtxNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtxNewPassword.Text))
            {
                e.Cancel = true;
                txtxNewPassword.Focus();
                errorProvider1.SetIconAlignment(txtxNewPassword, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtxNewPassword,"Please Write New Password!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtxNewPassword, "");
            }
        }

        private void txtxConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtxConfirmPassword.Text))
            {
                e.Cancel = true;
                txtxConfirmPassword.Focus();
                errorProvider1.SetIconAlignment(txtxConfirmPassword, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtxConfirmPassword, "Please Write New Password To Confirmed!.");
            }
            else
            {
                if (txtxConfirmPassword.Text!= txtxNewPassword.Text)
                {
                    e.Cancel = true;
                    txtxConfirmPassword.Focus();
                    errorProvider1.SetIconAlignment(txtxConfirmPassword, ErrorIconAlignment.MiddleRight);
                    errorProvider1.SetError(txtxConfirmPassword, "Not Equal New Password!.");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtxConfirmPassword, "");
                }
            }
        }
    }
}