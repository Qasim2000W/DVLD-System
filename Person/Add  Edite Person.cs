using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;
using DVLD.Properties;

namespace DVLD
{
    public partial class Add__Edite_Person : Form
    {
        public delegate void DelgatePerson(object sender, ClsBusinessPeople person);
        public event DelgatePerson DlgGetPerson;
        int _PersonID=-1;
        ClsBusinessPeople _Person = new ClsBusinessPeople();
        public ENMode _Mode;

        public Add__Edite_Person()
        {
            InitializeComponent();
            _Mode = ENMode.ADD;
        }

        public Add__Edite_Person(int PersonId)
        {
            InitializeComponent();
            _PersonID = PersonId;
            _Mode = ENMode.UBDATE;
        }

        public enum ENMode
        {
            ADD,
            UBDATE
        }

        private void _EditePerson()
        {
            _Person = ClsBusinessPeople.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("Not find this Person.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtfirstname.Text = _Person.firstname;
            txtsecondname.Text = _Person.Secondname;
            txtthirdname.Text = _Person.thirdname;
            txtlastname.Text = _Person.lastname;
            txtnationalno.Text = _Person.NationalNo;
            
            dateTimaofbirth.Value = _Person.DateOfbirth;

            if (_Person.Gendor == 0)
            {
                rbtnmale.Checked = true;
            }
            else
            {
                rbtnfemale.Checked = true;
            }

            txtPhone.Text = _Person.phone;
            txtemail.Text = _Person.email;
            cBcountry.SelectedIndex =cBcountry.FindString(_Person.countryName);
            txtAddress.Text = _Person.address;

            if (!string.IsNullOrEmpty(_Person.ImagePath)&&File.Exists(_Person.ImagePath))
            {
                pbPerson.ImageLocation= _Person.ImagePath;
                linkLblRemove.Visible = true;
            }

            txtnationalno.Enabled = false;
        }

        private bool _HandleImage()
        {
            if (_Person.ImagePath!=pbPerson.ImageLocation)
            {
                if (_Person.ImagePath!="")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(IOException iox)
                    {
                        MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (pbPerson.ImageLocation!=null)
                {
                    string SourcePath = pbPerson.ImageLocation;

                    if (ClsUtil.CopyImage(ref SourcePath))
                    {
                        _Person.ImagePath = SourcePath;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are NOT Valide!.", "WORNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandleImage())
                return;

            _Person.firstname = txtfirstname.Text;
            _Person.Secondname = txtsecondname.Text;
            _Person.thirdname = txtthirdname.Text;
            _Person.lastname = txtlastname.Text;
            _Person.DateOfbirth = dateTimaofbirth.Value;
            _Person.NationalNo = txtnationalno.Text;
            _Person.Gendor = rbtnmale.Checked ? (byte)0 : (byte)1;
            _Person.phone = txtPhone.Text;
            _Person.email = txtemail.Text;
            _Person.countryid = ClssCountry.FindByName(cBcountry.Text).countryid;
            _Person.address = txtAddress.Text;

            

            if (_Person.Save())
            {
                if (_Mode == ENMode.ADD)
                {
                    _Mode = ENMode.UBDATE;
                    MessageBox.Show("ADDIND SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DlgGetPerson?.Invoke(this, _Person);
                }
                else
                {
                    MessageBox.Show("Ubdating SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DlgGetPerson?.Invoke(this, _Person);
                }
            }
            else
            {
                if (_Mode == ENMode.ADD)
                {
                    MessageBox.Show("ADDIND NOT SUCESSEFULY.", "WORNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Ubdating NOT SUCESSEFULY.", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            _Mode = ENMode.UBDATE;
        }

        private void FillcBcountry()
        {
            DataTable dt = ClssCountry.GetList();

            foreach (DataRow row in dt.Rows)
            {
                cBcountry.Items.Add(row["CountryName"]);
            }
            cBcountry.SelectedIndex = cBcountry.FindString("iraq");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtfirstname_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                errorProvider1.SetIconAlignment(textBox, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(textBox, "please Fill This Field.");
            }
            else
            {
                if (Regex.IsMatch(textBox.Text, @"\d"))
                {
                    e.Cancel = true;
                    textBox.Focus();
                    errorProvider1.SetIconAlignment(textBox, ErrorIconAlignment.MiddleRight);
                    errorProvider1.SetError(textBox, "this feild not accept digital!.");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(textBox, "");
                }
            }
        }

        private void txtnationalno_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtnationalno.Text))
            {
                e.Cancel = true;
                txtnationalno.Focus();
                errorProvider1.SetIconAlignment(txtnationalno, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtnationalno, "please write your national number!.");
            }
            else
            {
                string firtsN = txtnationalno.Text;

                if (firtsN.ToLower().IndexOf("n") != 0)
                {
                    e.Cancel = true;
                    txtnationalno.Focus();
                    errorProvider1.SetIconAlignment(txtnationalno, ErrorIconAlignment.MiddleRight);
                    errorProvider1.SetError(txtnationalno, "this feild must be started by (n) or (N)!.");
                }
                else if (firtsN.ToLower().IndexOf("n") == 0 && (!Regex.IsMatch(firtsN, @"\d")))
                {
                    e.Cancel = true;
                    txtnationalno.Focus();
                    errorProvider1.SetIconAlignment(txtnationalno, ErrorIconAlignment.MiddleRight);
                    errorProvider1.SetError(txtnationalno, "this feild must be Have Number.");
                }
                else
                {
                    if (ClsBusinessPeople.IsExists(txtnationalno.Text)&&txtnationalno.Enabled==true)
                    {
                        e.Cancel = true;
                        txtnationalno.Focus();
                        errorProvider1.SetIconAlignment(txtnationalno, ErrorIconAlignment.MiddleRight);
                        errorProvider1.SetError(txtnationalno, "this national Number Is Exists, Choice Another it.");
                    }
                    else
                    {
                        e.Cancel = false;
                        errorProvider1.SetError(txtnationalno, "");
                    }               
                }
            }
        }

        private void Add__Edite_Person_Load(object sender, EventArgs e)
        {
            FillcBcountry();
            dateTimaofbirth.MaxDate = DateTime.Now.AddYears(-18);
            dateTimaofbirth.Value = dateTimaofbirth.MaxDate;
            dateTimaofbirth.MinDate = DateTime.Now.AddYears(-130);
            cBcountry.SelectedIndex = cBcountry.FindString("Iraq");

            if (_Mode==ENMode.UBDATE)
            {
                _EditePerson();
                lblTittle.Text = "UBDATE INFO";
            }

            linkLblRemove.Visible = (pbPerson.Image!=null);
        }

        private void rbtnmale_CheckedChanged_1(object sender, EventArgs e)
        {
            if (pbPerson.ImageLocation==null)
            pbPerson.Image=Resources.Male_512;
        }

        private void rbtnfemale_CheckedChanged_1(object sender, EventArgs e)
        {
            if (pbPerson.ImageLocation == null)
                pbPerson.Image = Resources.Female_512;
        }

        private void txtemail_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtemail.Text=="")
                return;
            

            if (!ClsValidition.ValditeEmail(txtemail.Text))
            {
                e.Cancel = true;
                txtemail.Focus();
                errorProvider1.SetIconAlignment(txtemail, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtemail, "this field must be have @!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtemail, "");
            }
        }

        private void txtPhone_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string firtsN = txtPhone.Text;

            if (!Regex.IsMatch(firtsN, @"\d"))
            {
                e.Cancel = true;
                txtPhone.Focus();
                errorProvider1.SetIconAlignment(txtPhone, ErrorIconAlignment.MiddleRight);
                errorProvider1.SetError(txtPhone, "this field must be have just Numbers!.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }

        private void linkLblsetimage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 0;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPerson.ImageLocation=openFileDialog1.FileName;
                linkLblRemove.Visible = true;
            }
        }

        private void linkLblRemove_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPerson.ImageLocation = null;
            linkLblRemove.Visible = false;

            if (rbtnmale.Checked)
                pbPerson.Image = Resources.Male_512;
            else
                pbPerson.Image = Resources.Female_512;
        }
    }
}