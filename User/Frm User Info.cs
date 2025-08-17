using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class Frm_User_Info : Form
    {
        ClsUsersBussiness User;

        public Frm_User_Info(int userid)
        {
            InitializeComponent();
            User = ClsUsersBussiness.Find(userid);
        }

        public Frm_User_Info()
        {
            InitializeComponent();
        }

        private void Frm_User_Info_Load(object sender, EventArgs e)
        {
            if (User == null)
            {
                MessageBox.Show("Not find this User.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClsBusinessPeople Person = ClsBusinessPeople.Find(User.PersonID);
            ctrlUserDetail1.CtrlUserDetail_Load(User, Person);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}