using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class CtrlUserDetail : UserControl
    {
       
        public CtrlUserDetail()
        {
            InitializeComponent();
        }

        ClsUsersBussiness User = new ClsUsersBussiness();

        public void CtrlUserDetail_Load(ClsUsersBussiness user,ClsBusinessPeople clsPeople)
        {
            User = user;

            if (User.UserID !=-1)
            {
                lblUserId.Text = User.UserID.ToString();
                lblUserName.Text = User.UserName;
                lblIsActive.Text = User.IsActive == true ? "Yes" : "No";
                ctrlPersonDetails1.CtrlPersonDetails_Load(clsPeople);
            }
        }
    }
}