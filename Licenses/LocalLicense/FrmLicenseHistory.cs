using System;
using System.Data;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class FrmLicenseHistory : Form
    {
        string NationalNo;
        int PersonID = -1;
        ClsBusinessPeople clsPeople = new ClsBusinessPeople();
        DataTable _DTLocalLicense = new DataTable();
        DataTable _DTInterNationalLicense = new DataTable();

        public FrmLicenseHistory(string Nationalno)
        {
            InitializeComponent();

            NationalNo = Nationalno;
        }

        public FrmLicenseHistory(int Personid)
        {
            InitializeComponent();

            PersonID = Personid;
        }

        public FrmLicenseHistory(ClsBusinessPeople Person)
        {
            InitializeComponent();

            clsPeople = Person;
        }

        public void LoadPageLocal()
        {
            dtGViewLocal.RowTemplate.Height = 36;
            dtGViewLocal.ReadOnly = true;
            dtGViewLocal.AllowUserToAddRows = false;

            dtGViewLocal.DataSource = _DTLocalLicense.DefaultView;

            dtGViewLocal.Columns[0].Width = 170;
            dtGViewLocal.Columns[1].Width = 180;
            dtGViewLocal.Columns[2].Width = 320;
            dtGViewLocal.Columns[3].Width = 300;
            dtGViewLocal.Columns[4].Width = 229;
            dtGViewLocal.Columns[5].Width = 190;

            LBLRecoreds.Text = dtGViewLocal.Rows.Count.ToString();
        }

        public void LoadPageInterNational()
        {
            dtGViewInternational.RowTemplate.Height = 36;
            dtGViewInternational.ReadOnly = true;
            dtGViewInternational.AllowUserToAddRows = false;

            dtGViewInternational.DataSource = _DTInterNationalLicense.DefaultView;

            dtGViewInternational.Columns[0].Width = 170;
            dtGViewInternational.Columns[1].Width = 180;
            dtGViewInternational.Columns[2].Width = 320;
            dtGViewInternational.Columns[3].Width = 300;
            dtGViewInternational.Columns[4].Width = 229;
            dtGViewInternational.Columns[5].Width = 190;

            lblRecoredIntrnational.Text = dtGViewInternational.Rows.Count.ToString();
        }

        private void _Clear()
        {
            _DTLocalLicense.Clear();
            _DTInterNationalLicense.Clear();
        }

        private void FillDataGrid(int PersonID)
        {
            ClsDrivers clsDrivers = ClsDrivers.GetRecored(clsPeople.id);

            if (clsDrivers == null)
            {
                MessageBox.Show("This Person Not Driver In The System.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _DTLocalLicense = ClsLicenses.GetListLicenseForPersonToShowing(clsDrivers.ID);
            LoadPageLocal();

            _DTInterNationalLicense = ClsInternationalLicenses.GetListForDriver(clsDrivers.ID);

            if (_DTInterNationalLicense.Rows.Count != 0)
            {
                LoadPageInterNational();
            }

            ctrlPersonDetailsWithFilter1.EnabledFilter = false;
        }

        private void FrmLicenseHistory_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NationalNo))
            {
                clsPeople = ClsBusinessPeople.FindByNationalNo(NationalNo);
            }
            else if (clsPeople==null||clsPeople.id==-1)
            {
                clsPeople = ClsBusinessPeople.Find(PersonID);
            }

            FillDataGrid(clsPeople.id);

            ctrlPersonDetailsWithFilter1.GetPerson(clsPeople);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicenseInfo frmLicenseInfo = new FrmLicenseInfo((int)dtGViewLocal.CurrentRow.Cells[0].Value);
            frmLicenseInfo.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmIntrnationalDriverInfo frmIntrnationalDriverInfo = new FrmIntrnationalDriverInfo((int)dtGViewInternational.CurrentRow.Cells[0].Value);
            frmIntrnationalDriverInfo.ShowDialog();
        }

        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int obj)
        {
            PersonID = obj;

            if ((clsPeople == null || clsPeople.id == -1) && string.IsNullOrEmpty(NationalNo) && PersonID == -1)
                _Clear();
            else
                FillDataGrid(PersonID);
        }
    }
}