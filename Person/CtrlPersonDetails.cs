using System.IO;
using System.Windows.Forms;
using Business;
using DVLD.Properties;

namespace DVLD
{
    public partial class CtrlPersonDetails : UserControl
    {
        public CtrlPersonDetails()
        {
            InitializeComponent();
        }

        ClsBusinessPeople Person1 = new ClsBusinessPeople();  

        public void CtrlPersonDetails_Load(ClsBusinessPeople Person)
        {
            Person1 = Person;

            if (Person1.countryid == -1||Person1==null)
            {
                lnkLblEdit.Enabled = false;
            }
            else
            {
                lnkLblEdit.Enabled = true;
            }

            lblPersonId.Text = Person.id.ToString();
            lblName.Text = Person.fullname;
            lblNationalNo.Text = Person.NationalNo;
            lblGender.Text = Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = Person.email;
            lblAddress.Text = Person.address;
            lblDateOfBirth.Text = Person.DateOfbirth.ToString();
            lblPhone.Text = Person.phone;
            lblCountry.Text = Person.countryName;

            if (Person.ImagePath!=""&& File.Exists(Person.ImagePath))
            {
                picturePerson.ImageLocation = Person.ImagePath;
            }
            else
            {
                if (Person.Gendor==0)
                    picturePerson.Image = Resources.Male_512;
                else
                    picturePerson.Image = Resources.Female_512;
            }
        }

        public void FillPerson(object sender, ClsBusinessPeople Person)
        {
            Person1 = Person;
        }

        private void lnkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Add__Edite_Person Add__Edite_PersonForm = new Add__Edite_Person(Person1.id);
            Add__Edite_PersonForm.DlgGetPerson += FillPerson;
            Add__Edite_PersonForm.ShowDialog();
            CtrlPersonDetails_Load(Person1);
        }
    }
}