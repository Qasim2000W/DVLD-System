using System;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmUbdateTestType : Form
    {
        int ID = 0;
        ClsTestTypeBusiness Test = new ClsTestTypeBusiness();

        public FrmUbdateTestType(int id)
        {
            InitializeComponent();
            ID = id;
        }

        private void FrmUbdateTestType_Load(object sender, EventArgs e)
        {
            Test = ClsTestTypeBusiness.GetRecored((ClsTestTypeBusiness.enTestType)ID);

            if (Test == null)
            {
                MessageBox.Show($"THIS Test WITH THIS ID : {ID} NOT FOUND.!", "ERRORE", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                lblID.Text = Test.ID.ToString();
                txtTitle.Text = Test.Title;
                txtDescripton.Text = Test.Description;
                txtFees.Text = Test.Fees.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            Test.Title = txtTitle.Text;
            Test.Description = txtDescripton.Text;
            Test.Fees = Convert.ToDecimal(txtFees.Text);

            if (Test.SAVE())
            {
                MessageBox.Show("Ubdating Successifuly.!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ubdating Not Successifuly.!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            };
        }

        private void txtDescripton_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescripton.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescripton, "Description cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtDescripton, null);
            }
        }

        private void txtFees_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            };

            if (!ClsValidition.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            };
        }
    }
}