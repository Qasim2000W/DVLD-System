using System;
using System.Data;
using System.Windows.Forms;
using Business;
using DVLD.Global_Classes;

namespace DVLD
{
    public partial class FormPeopleManagement : Form
    {
        public FormPeopleManagement()
        {
            InitializeComponent();
            _RefreshList();
            cbFilter.SelectedIndex = 0;
        }

        static DataTable DT = ClsBusinessPeople.GetList();

        DataTable _DT=DT.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "Gendor", "DateOfBirth", "CountryName","Phone", "Email");


        private void _RefreshList()
        {
            dtGridViewPeople.RowTemplate.Height = 36;
            DT = ClsBusinessPeople.GetList();
            _DT = DT.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "Gendor", "DateOfBirth", "CountryName", "Phone", "Email");
            
            dtGridViewPeople.DataSource = _DT.DefaultView;
        }

        private void btnAddNewPeople_Click(object sender, EventArgs e)
        {
            Add__Edite_Person Add__Edite_PersonForm = new Add__Edite_Person();
            Add__Edite_PersonForm.ShowDialog();
            _RefreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            
            switch (cbFilter.Text)
            {
                case "PersonID":
                    FilterColumn = "PersonID";
                    break;

                case "NationalNo":
                    FilterColumn = "NationalNo";
                    break;

                case "FirstName":
                    FilterColumn = "FirstName";
                    break;

                case "SecondName":
                    FilterColumn = "SecondName";
                    break;

                case "ThirdName":
                    FilterColumn = "ThirdName";
                    break;

                case "LastName":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;                       
                                                  
                case "Gendor":
                    FilterColumn = "Gendor";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(txtFilter.Text==""|| FilterColumn=="None")
            {
                _DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = dtGridViewPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
                _DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text);
            else
                _DT.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterColumn, txtFilter.Text);

            LBLRecoreds.Text = dtGridViewPeople.Rows.Count.ToString();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Clear();
            txtFilter.Visible = (cbFilter.Text!="None");
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add__Edite_Person Add__Edite_PersonForm = new Add__Edite_Person((int)dtGridViewPeople.CurrentRow.Cells[0].Value);
            Add__Edite_PersonForm.ShowDialog();
            _RefreshList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are You Sure to delete this Person [{(int)dtGridViewPeople.CurrentRow.Cells[0].Value}].", 
                "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)== DialogResult.OK)
            { 
               if (!ClsBusinessPeople.IsExists((int)dtGridViewPeople.CurrentRow.Cells[0].Value))
               {
                    MessageBox.Show("Not find this Person.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
               }

                if (!ClsUtil.DeleteImage(ClsBusinessPeople.Find((int)dtGridViewPeople.CurrentRow.Cells[0].Value).ImagePath))
                {
                    MessageBox.Show("Image person Was Not deleting.", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (ClsBusinessPeople.Delete((int)dtGridViewPeople.CurrentRow.Cells[0].Value))
                {
                   MessageBox.Show("done delete this person .", "Deleting", MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);

                    _RefreshList();
                }
               else
               {
                   MessageBox.Show("person Was Not deleting because it has data to link it.", "ERRORE",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
            }
        }

        private void showDetialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetails FormDetailPerson = new PersonDetails((int)dtGridViewPeople.CurrentRow.Cells[0].Value);
            FormDetailPerson.ShowDialog();
            _RefreshList();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add__Edite_Person Add__Edite_PersonForm = new Add__Edite_Person();
            Add__Edite_PersonForm.ShowDialog();
            _RefreshList();
        }

        private void FormPeopleManagement_Load(object sender, EventArgs e)
        {
            dtGridViewPeople.DataSource = _DT;

            dtGridViewPeople.ReadOnly = true;
            dtGridViewPeople.AllowUserToAddRows = false;

            if (_DT.Rows.Count > 0)
            {
                dtGridViewPeople.Columns[0].HeaderText = "Person ID";
                dtGridViewPeople.Columns[0].Width = 130;

                dtGridViewPeople.Columns[1].HeaderText = "National No";
                dtGridViewPeople.Columns[1].Width = 150;

                dtGridViewPeople.Columns[2].HeaderText = "First Name";
                dtGridViewPeople.Columns[2].Width = 180;

                dtGridViewPeople.Columns[3].HeaderText = "Second Name";
                dtGridViewPeople.Columns[3].Width = 171;

                dtGridViewPeople.Columns[4].HeaderText = "Third Name";
                dtGridViewPeople.Columns[4].Width = 200;

                dtGridViewPeople.Columns[5].HeaderText = "Last Name";
                dtGridViewPeople.Columns[5].Width = 200;

                dtGridViewPeople.Columns[6].HeaderText = "Gender";
                dtGridViewPeople.Columns[6].Width = 90;

                dtGridViewPeople.Columns[7].HeaderText = "Date Of Birth";
                dtGridViewPeople.Columns[7].Width = 200;

                dtGridViewPeople.Columns[8].HeaderText = "Nationality";
                dtGridViewPeople.Columns[8].Width = 120;

                dtGridViewPeople.Columns[9].HeaderText = "Phone";
                dtGridViewPeople.Columns[9].Width = 190;

                dtGridViewPeople.Columns[10].HeaderText = "Email";
                dtGridViewPeople.Columns[10].Width = 240;
            }

            LBLRecoreds.Text = dtGridViewPeople.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}