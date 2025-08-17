using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD.Applications
{
    public partial class CtrlApplicationBasic : UserControl
    {
        ClsApplicationBusiness application = new ClsApplicationBusiness();

        public int ApplicationID { get { return (int)application.ApplicationID; } }

        public CtrlApplicationBasic()
        {
            InitializeComponent();
        }

        public void CtrlApplicationBasic_Load(ClsApplicationBusiness clsApplication)
        {
            application = clsApplication;

            _Refresh();
        }

        private void _Refresh()
        {
            lblID.Text = application.ApplicationID.ToString();
            lblsTATUS.Text = application.ApplicationStatusText;
            lbLfEES.Text = application.PaidFees.ToString();
            LBLAppliCant.Text = application.clsPerson.fullname;
            lbldATE.Text = application.ApplicationDate.ToString();
            lbltYPE.Text = application.ApplicationTypeInfo.Title;
            lblStatusDate.Text = ClsFormat.DateToShort(application.LastStatusDate);
            lblCreatedBy.Text = ClsGlobal.CurrentUser.UserID.ToString();
        }

        private void lnkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersonDetails FrmpersonDetails = new PersonDetails(application.PersonID);
            FrmpersonDetails.ShowDialog();

            _Refresh();
        }
    }
}