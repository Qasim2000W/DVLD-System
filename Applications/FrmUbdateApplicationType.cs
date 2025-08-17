using System;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FrmUbdateApplicationType : Form
    {
        int AppID = 0;
        ClsApplicationTypeBusiness AppType;

        public FrmUbdateApplicationType(int Appid)
        {
            InitializeComponent();
            AppID = Appid;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUbdateApplicationType_Load(object sender, EventArgs e)
        {
            AppType = ClsApplicationTypeBusiness.GetRecored(AppID);

            if (AppType == null)
            {
                MessageBox.Show($"THIS APPLICATION WITH THIS ID : {AppID} NOT FOUND.!", "ERRORE", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                lblID.Text = AppType.ID.ToString();
                txtTitle.Text = AppType.Title;
                txtFees.Text = AppType.Fees.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AppType.Title = txtTitle.Text;
            AppType.Fees =Convert.ToDecimal(txtFees.Text);

            if (AppType.SAVE())
            {
                MessageBox.Show("Ubdating Successifuly.!", "Saved", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ubdating Not Successifuly.!", "Saved", MessageBoxButtons.OK,MessageBoxIcon.Error);
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