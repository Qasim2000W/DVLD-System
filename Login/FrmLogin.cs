using System;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        static ClsUsersBussiness User = new ClsUsersBussiness();

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (ClsGlobal.GetDataUser(ref UserName,ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chBoxRemember.Checked = true;
            }
            else
            {
                chBoxRemember.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;

            User = ClsUsersBussiness.Find(UserName);

            if (User!=null)
            {
                if (User.Password == ClsSecurity.Hashing(txtPassword.Text))
                {
                    if (User.IsActive)
                    {
                        if (chBoxRemember.Checked)
                        {
                            ClsGlobal.RegisterUser(txtUserName.Text, txtPassword.Text);
                        }
                        else
                        {
                            ClsGlobal.RegisterUser("", "");
                        }

                        ClsGlobal.CurrentUser = User;
                        this.Hide();
                        Form1 form1 = new Form1(this);
                        form1.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("This User Not Active.!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("This password Not Corrected.!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Not Exists User By User Name : {UserName} .!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}