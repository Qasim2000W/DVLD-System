using System;
using System.Data;
using System.Windows.Forms;
using Business;

namespace DVLD
{
    public partial class Manage_Users : Form
    {
        public Manage_Users()
        {
            InitializeComponent();
        }

        DataTable DT;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "IsActive")
            {
                txtFilter.Visible = false;
                cbFilterIsActive.Visible = true;
            }
            else
            {
                txtFilter.Visible = (cbFilter.Text != "None");
                cbFilterIsActive.Visible = false;
                txtFilter.Clear();
                txtFilter.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbFilterIsActive.Text;

            switch (FilterValue)
            {
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
                case "All":
                    FilterValue = "All";
                    break;
            }

            if (FilterValue == "All")
                DT.DefaultView.RowFilter = "";
            else
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, FilterValue);

            LBLRecoreds.Text = DGVUsers.Rows.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddNewUser formAddNewUser = new FormAddNewUser();
            formAddNewUser.ShowDialog();
            Manage_Users_Load(null,null);
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddNewUser formAddNewUser = new FormAddNewUser();
            formAddNewUser.ShowDialog();
            Manage_Users_Load(null, null);
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddNewUser formAddNewUser = new FormAddNewUser((int)DGVUsers.CurrentRow.Cells[0].Value);
            formAddNewUser.ShowDialog();
            Manage_Users_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are You Sure to delete this User [{(int)DGVUsers.CurrentRow.Cells[0].Value}].", "Confirm Delete",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (!ClsUsersBussiness.IsExists((int)DGVUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Not find this User.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ClsUsersBussiness.Delete((int)DGVUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("done delete this User .", "Deleting", MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);

                    Manage_Users_Load(null, null);
                }
                else
                {
                    MessageBox.Show("User Was Not deleting because it has data to link it.", "ERRORE",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_User_Info frm_User_Info = new Frm_User_Info((int)DGVUsers.CurrentRow.Cells[0].Value);
            frm_User_Info.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FrmChangePassword frmChangePassword = new FrmChangePassword((int)DGVUsers.CurrentRow.Cells[0].Value);
            frmChangePassword.ShowDialog();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "PersonID" || cbFilter.Text == "UserID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilter_TextChanged_1(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.Text)
            {
                case "UserID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;
                case "PersonID":
                    FilterColumn = "PersonID";
                    break;
                case "FullName":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text == "" || cbFilter.Text == "None")
            {
                DT.DefaultView.RowFilter = "";
                LBLRecoreds.Text = DGVUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                DT.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text);
            else
                DT.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text);

            LBLRecoreds.Text = DGVUsers.Rows.Count.ToString();
        }

        private void Manage_Users_Load(object sender, EventArgs e)
        {
            DGVUsers.RowTemplate.Height = 36;

            DT = ClsUsersBussiness.GetListUsers();

            cbFilter.SelectedIndex = 0;
            DGVUsers.ReadOnly = true;
            DGVUsers.AllowUserToAddRows = false;
            DGVUsers.DataSource = DT.DefaultView;

            if (DGVUsers.Rows.Count > 0)
            {
                DGVUsers.Columns[0].HeaderText = "User ID";
                DGVUsers.Columns[0].Width = 110;

                DGVUsers.Columns[1].HeaderText = "Person ID";
                DGVUsers.Columns[1].Width = 120;

                DGVUsers.Columns[2].HeaderText = "Full Name";
                DGVUsers.Columns[2].Width = 350;

                DGVUsers.Columns[3].HeaderText = "UserName";
                DGVUsers.Columns[3].Width = 120;

                DGVUsers.Columns[4].HeaderText = "Is Active";
                DGVUsers.Columns[4].Width = 120;
            }

            LBLRecoreds.Text = DGVUsers.Rows.Count.ToString();
        }
    }
}