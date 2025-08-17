using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmTakeTest : Form
    {
        ClsAppointment _ClsAppointment = new ClsAppointment();
        ClsTestTypeBusiness.enTestType _TestTypeID;
        ClsTests _clsTests;

        public FrmTakeTest(ClsTestTypeBusiness.enTestType TestTypeID,int AppointmentID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
            _ClsAppointment = ClsAppointment.GetRecored(AppointmentID);
        }

        private void FrmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlTakeTest1.TestTypeID = _TestTypeID;

            if (_ClsAppointment.ID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            ctrlTakeTest1.GitFillData(_ClsAppointment);
            ctrlTakeTest1.AppeiranceData();

            int _TestID = ctrlTakeTest1.TestID;

            if (_TestID != -1)
            {
                _clsTests = ClsTests.Find(_TestID);

                if (_clsTests.TestResult)
                    rbpass.Checked = true;
                else
                    rbfail.Checked = true;

                txtNotes.Text = _clsTests.Notes;

                lblUserMessage.Visible = true;
                rbpass.Enabled = false;
                rbfail.Enabled = false;
            }
            else
                _clsTests = new ClsTests();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Save? After That You Cannot Change The Pass/Fail Result After You Save?.", "Confirm", 
                MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                _clsTests.Notes = txtNotes.Text;
                _clsTests.TestResult = rbpass.Checked ? true : false;
                _clsTests.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
                _clsTests.TestAppointmentID = _ClsAppointment.ID;

                if (_clsTests.Save())
                {
                    MessageBox.Show("ADDIND SUCESSEFULY.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("ADDIND Not SUCESSEFULY.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}