using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;
using DVLD.Properties;

namespace DVLD
{
    public partial class CTRLInfoLicense : UserControl
    {
        ClsLicenses clsLicense;
        int _LicenseID = -1;

        public int SelectLicenseID { get { return _LicenseID; } }

        public ClsLicenses SelectedLicenseInfo
        { get { return clsLicense; } }
        
        public CTRLInfoLicense()
        {
            InitializeComponent();
        }

        public enum EnReason
        {
            Firsttime=1,
            Renew=2,
            Lost=3,
            Damaged=4
        }

        public void Appeareance(int LicenseID)
        {
            clsLicense = ClsLicenses.FindByID(LicenseID);
            _LicenseID = LicenseID;

            if (clsLicense==null)
            {
                MessageBox.Show("Could not find License ID = " + LicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblClassName.Text = clsLicense.ClsLicenseClass.ClassName;
            lblName.Text = clsLicense.clsDriver.clsPerson.fullname;
            lblLicenseId.Text = clsLicense.ID.ToString();
            lblNationalNo.Text = clsLicense.clsDriver.clsPerson.NationalNo;
            lblGender.Text = clsLicense.clsDriver.clsPerson.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = ClsFormat.DateToShort(clsLicense.IssueDate);
            lblIssueReason.Text = clsLicense.IssueReasonText;
            lblNotes.Text = clsLicense.Notes!=string.Empty? clsLicense.Notes:"No Notes";
            lblIsActive.Text = clsLicense.IsActive == true ? "Yes" : "No";
            lblDateOfBirth.Text = ClsFormat.DateToShort(clsLicense.clsDriver.clsPerson.DateOfbirth);
            lblDriverid.Text = clsLicense.DriverID.ToString();
            lblExpirationDate.Text = ClsFormat.DateToShort(clsLicense.ExpirationDate);
            lblIsDetained.Text = clsLicense.IsDetained ? "Yes" : "No";
            _LoadImage();
        }

        private void _LoadImage()
        {
            string ImagePath = clsLicense.clsDriver.clsPerson.ImagePath;

            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (File.Exists(ImagePath))
                    picturePerson.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (clsLicense.clsDriver.clsPerson.Gendor == 0)
                    picturePerson.Image = Resources.Male_512;
                else
                    picturePerson.Image = Resources.Female_512;
            }
        }
    }
}