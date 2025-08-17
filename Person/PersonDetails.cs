using System;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class PersonDetails : Form
    {
        ClsBusinessPeople person;
        public PersonDetails(int PersonID)
        {
            InitializeComponent();
            person = ClsBusinessPeople.Find(PersonID);
        }

        public PersonDetails(string NationalNo)
        {
            InitializeComponent();
            person = ClsBusinessPeople.FindByNationalNo(NationalNo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PersonDetails_Load(object sender, EventArgs e)
        {
            if (person != null)
            {
                ctrlPersonDetails1.CtrlPersonDetails_Load(person);
            }
            else
            {
                MessageBox.Show("Something Wrong.", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}