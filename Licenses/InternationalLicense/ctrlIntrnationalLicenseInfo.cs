using System.IO;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;
using DVLD.Properties;

namespace DVLD
{
    public partial class ctrlIntrnationalLicenseInfo : UserControl
    {
        int _InternationalLicenseID = -1;
        ClsInternationalLicenses clsInternationalLicenses;

        public ctrlIntrnationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadImage()
        {
            string ImagePath = clsInternationalLicenses.DriverInfo.clsPerson.ImagePath;

            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (File.Exists(ImagePath))
                    picturePerson.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (clsInternationalLicenses.DriverInfo.clsPerson.Gendor == 0)
                    picturePerson.Image = Resources.Male_512;
                else
                    picturePerson.Image = Resources.Female_512;
            }
        }

        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            clsInternationalLicenses = ClsInternationalLicenses.GetRecoredByID(_InternationalLicenseID);

            if (clsInternationalLicenses == null)
            {
                MessageBox.Show("Could not find Internationa License ID = " + _InternationalLicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.
                                Error);
                _InternationalLicenseID = -1;
                return;
            }

            lblName.Text = clsInternationalLicenses.DriverInfo.clsPerson.fullname;
            lblApplicationID.Text = clsInternationalLicenses.ApplicationID.ToString();
            lblIntLicenseID.Text = clsInternationalLicenses.ID.ToString();
            lblLicenseId.Text = clsInternationalLicenses.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = clsInternationalLicenses.DriverInfo.clsPerson.NationalNo;
            lblGender.Text = clsInternationalLicenses.DriverInfo.clsPerson.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = ClsFormat.DateToShort(clsInternationalLicenses.IssueDate);
            lblIsActive.Text = clsInternationalLicenses.IsActive == true ? "Yes" : "No";
            lblDateOfBirth.Text = ClsFormat.DateToShort(clsInternationalLicenses.DriverInfo.clsPerson.DateOfbirth);
            lblDriverid.Text = clsInternationalLicenses.DriverID.ToString();
            lblExpirationDate.Text =ClsFormat.DateToShort(clsInternationalLicenses.ExpirationDate);
            _LoadImage();
        }
    }
}