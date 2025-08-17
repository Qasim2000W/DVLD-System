using ClsDataAccess;

namespace Business
{
    public class ClsTests
    {
        public int ID { get; set; }

        public int TestAppointmentID { get; set; }
        ClsAppointment appointment { get; set; }

        public string Notes { get; set; }

        public bool TestResult { get; set; }

        public int CreatedByUserID { get; set; }

        public enum enMode
        {
            ADD,
            Ubdate
        }

        public enMode Mode { get; set; }

        public ClsTests()
        {
            this.ID = -1;
            this.TestAppointmentID = -1;
            this.Notes = string.Empty;
            this.TestResult = false;
            this.CreatedByUserID = -1;

            Mode = enMode.ADD;
        }

        ClsTests(int ID, string Notes, int AppointmentID, bool TestResult, int CreatedByUserID)
        {
            this.ID = ID;
            this.TestAppointmentID = AppointmentID;
            this.appointment = ClsAppointment.GetRecored(AppointmentID);
            this.Notes = Notes;
            this.TestResult = TestResult;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Ubdate;
        }

        private bool _AddNew()
        {
            this.ID = ClsTestsData.AddNewRecored(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return (this.ID != -1);
        }

        public static ClsTests Find(int TestID)
        {
            int AppointmentID = -1;
            string Notes = string.Empty;
            bool TestResult = false;
            int CreatedByUserID = -1;

            if (ClsTestsData.Find(TestID, ref AppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new ClsTests(TestID, Notes, AppointmentID, TestResult, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        private bool _UpdateTest()
        {
            return ClsTestsData.UpdateTest(this.ID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.ADD:

                    if (_AddNew())
                    {
                        Mode = enMode.Ubdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Ubdate:
                    return _UpdateTest();
            }

            ClsEventLog.EventLogger("Not Saved New Data, something wrong", ClsEventLog.ENTypeMessage.warning);
            return false;
        }

        public static int GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
           return ClsTestsData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static ClsTests FindLastTestByTestType(int PersonID, int LicenseClassID, ClsTestTypeBusiness.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; 
            string Notes = ""; 
            int CreatedByUserID = -1;

            if (ClsTestsData.GetLastTestByPersonIDAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID, ref TestID, ref TestAppointmentID,
                                                                             ref TestResult, ref Notes, ref CreatedByUserID))

                return new ClsTests(TestID, Notes, TestAppointmentID, TestResult, CreatedByUserID);
            else
            {
                ClsEventLog.EventLogger("the object is null, something wrong", ClsEventLog.ENTypeMessage.warning);
                return null;
            }
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}