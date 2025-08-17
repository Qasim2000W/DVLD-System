using System;
using System.ComponentModel;
using System.Windows.Forms;
using Business;

namespace DVLD.Person
{
    public partial class CtrlPersonDetailsWithFilter : UserControl
    {
        public CtrlPersonDetailsWithFilter()
        {
            InitializeComponent();
        }

        ClsBusinessPeople Person;

        private bool _ShowAddPerson = true;

        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }

            set
            {
                _ShowAddPerson = value;
                btnAddPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _EnabledFilter = true;

        public bool EnabledFilter
        {
            get { return _EnabledFilter; }

            set
            {
                _EnabledFilter = value;
                groupBoxFilter.Enabled = _EnabledFilter;
            }
        }

        public int PersonID 
        { 
            get { return Person.id; } 
        }

        public enum ENMode
        {
            Add,
            Ubdate
        }

        ENMode Mode = ENMode.Add;

        public void GetPerson(ClsBusinessPeople person)
        {
            Mode = ENMode.Ubdate;
            Person = person;
            FindNow();
        }

        public void FindNow()
        {
            if (Mode == ENMode.Add)
            {
                switch (cbFilter.Text)
                {
                    case "NationalNo":
                        Person = ClsBusinessPeople.FindByNationalNo(txtFilter.Text);
                        break;
                    case "PersonID":
                        Person = ClsBusinessPeople.Find(int.Parse(txtFilter.Text));
                        break;
                }
            }

            if (Person==null)
            {
                MessageBox.Show("Not Found Person.", "Not found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (OnPersonSelected != null)
                PersonSelected(Person.id);

            ctrlPersonDetails1.CtrlPersonDetails_Load(Person);
        }

        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); 
            }
        }

        public void FilterFocus()
        {
            txtFilter.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FindNow();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Add__Edite_Person frmadd__Edite_Person = new Add__Edite_Person();
            frmadd__Edite_Person.DlgGetPerson += Frmadd__Edite_Person_DlgGetPerson;
            frmadd__Edite_Person.ShowDialog();
            ctrlPersonDetails1.CtrlPersonDetails_Load(Person);
        }

        private void Frmadd__Edite_Person_DlgGetPerson(object sender, Business.ClsBusinessPeople person)
        {
            Person = person;
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                e.Cancel = true;
                txtFilter.Focus();
                errorProvider1.SetError(txtFilter, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilter,null);
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)13)
            {
                btnSearch.PerformClick();
            }

            if (cbFilter.Text== "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ctrlPersonDetails1_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
        }
    }
}