using System;
using System.Data.SqlClient;


namespace ClsDataAccess
{
    public class ClsTestsData
    {
        public static int AddNewRecored(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"Insert Into Tests (TestAppointmentID,TestResult, Notes, CreatedByUserID) Values(@TestAppointmentID,@TestResult,@Notes,
                           @CreatedByUserID); UPDATE TestAppointments SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;
                           SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (Notes != "" && Notes != null)
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connect.Open();

                Object RESULT = command.ExecuteScalar();

                if (RESULT != null && int.TryParse(RESULT.ToString(), out int INsertedID))
                {
                    ContactId = INsertedID;
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return -1;
            }

            finally
            {
                connect.Close();
            }

            return ContactId;
        }

        public static bool Find(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from Tests where TestID=@TestID";
            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];

                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                isfound = false;
            }
            finally
            {
                connect.Close();
            }

            return isfound;
        }

        public static int GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            int NumberPassedTests = 0;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string Query = "select PassedCount=COUNT(TestTypeID) from TestAppointments inner join Tests on Tests.TestAppointmentID" +
                           "=TestAppointments.TestAppointmentID where LocalDrivingLicenseApplicationID=" +
                           "@LocalDrivingLicenseApplicationID and TestResult=1";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (Result != null && byte.TryParse(Result.ToString(), out byte Count))
                {
                    NumberPassedTests = Count;
                }

            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return NumberPassedTests;
            }
            finally
            {
                connection.Close();
            }

            return NumberPassedTests;
        }

        public static bool GetLastTestByPersonIDAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID, ref int TestID, 
                                                                          ref int TestAppointmentID, ref bool TestResult, ref string Notes, 
                                                                          ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"select top 1 Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, Tests.Notes, Tests.CreatedByUserID, Applications.
                           ApplicantPersonID from Tests inner join TestAppointments on Tests.TestAppointmentID=TestAppointments.TestAppointmentID inner join 
                           LocalDrivingLicenseApplications on TestAppointments.LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplications.
                           LocalDrivingLicenseApplicationID inner join Applications on Applications.ApplicationID=LocalDrivingLicenseApplications.ApplicationID
                           where (ApplicantPersonID=@ApplicantPersonID) and (LocalDrivingLicenseApplications.LicenseClassID=@LicenseClassID) and 
                           (TestAppointments.TestTypeID=@TestTypeID) order by Tests.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestID = (int)reader["TestID"];
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"Update Tests set TestAppointmentID = @TestAppointmentID, TestResult=@TestResult, Notes = @Notes,
                           CreatedByUserID=@CreatedByUserID where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
    }
}