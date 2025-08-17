using System;
using System.Data.SqlClient;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace ClsDataAccess
{
    public class ClsAppointmentData
    {
        public static int AddNewRecored(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, 
                                        decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO TestAppointments(TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,
                           PaidFees,CreatedByUserID,IsLocked, RetakeTestApplicationID) VALUES(@TestTypeID,
                           @LocalDrivingLicenseApplicationID, @AppointmentDate,@PaidFees, @CreatedByUserID,@IsLocked,
                           @RetakeTestApplicationID); select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (RetakeTestApplicationID==-1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            }

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

        public static bool UbdateRecored(int ID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, 
                                         decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE TestAppointments 
            set
            [TestTypeID]            =@TestTypeID      
           ,[LocalDrivingLicenseApplicationID]=@LocalDrivingLicenseApplicationID
           ,[AppointmentDate]       =@AppointmentDate
           ,[PaidFees]              =@PaidFees
           ,[IsLocked]              =@IsLocked  
           ,[CreatedByUserID]       =@CreatedByUserID 
           ,[RetakeTestApplicationID]=@RetakeTestApplicationID
            WHERE TestAppointmentID =@ID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (RetakeTestApplicationID == -1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            }

            try
            {
                connect.Open();
                roweffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return false;
            }
            finally
            {
                connect.Close();
            }

            return (roweffected > 0);
        }

        public static bool GetRecored(int LDLAppID, int TestTypeID, ref int AppointmentID, ref DateTime AppointmentDate,
                                      ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "SELECT * FROM TestAppointments where LocalDrivingLicenseApplicationID=@LDLAppID and " +
                           "TestTypeID=@TestTypeID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    IsFound = true;

                    AppointmentID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                    {
                        RetakeTestApplicationID = -1;
                    }
                    else
                    {
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                IsFound = false;
            }
            finally
            {
                connect.Close();
            }

            return IsFound;
        }

        public static bool GetRecored(int AppointmentID, ref int LDLAppID, ref int TestTypeID, ref DateTime AppointmentDate,
                                      ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "SELECT * FROM TestAppointments where TestAppointmentID=@AppointmentID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    IsFound = true;

                    LDLAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    TestTypeID = (int)reader["TestTypeID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"]==DBNull.Value)
                    {
                        RetakeTestApplicationID = -1;
                    }
                    else
                    {
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                IsFound = false;
            }
            finally
            {
                connect.Close();
            }

            return IsFound;
        }

        public static DataTable GetRecored(int LDLAppID, int TestTypeID)
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select TestAppointmentID as AppointmentID,AppointmentDate as [Appointment Date], " +
                           "PaidFees as [Paid Fees], IsLocked as [Is Locked] from TestAppointments " +
                           "where LocalDrivingLicenseApplicationID=@LDLAppID and TestTypeID=@TestTypeID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    DT.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return null;
            }
            finally
            {
                connect.Close();
            }

            return DT;
        }

        public static byte GetTotalTrail(int LDLAppID, int TestTypeID)
        {
            byte TotalTrail = 0;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select count(TestID) from LocalDrivingLicenseApplications inner join TestAppointments on " +
                           "LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = " +
                           "TestAppointments.LocalDrivingLicenseApplicationID inner join Tests on TestAppointments.TestAppointmentID" +
                           " = Tests.TestAppointmentID where(LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID " +
                           "= @LDLAppID) and(TestAppointments.TestTypeID = @TestTypeID)";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connect.Open();

                object reader = command.ExecuteScalar();

                if (reader!=null&&byte.TryParse(reader.ToString(),out byte Trials))
                {
                    TotalTrail = Trials;
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return TotalTrail;
            }
            finally
            {
                connect.Close();
            }

            return TotalTrail;
        }

        public static DataTable GetList()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select TestAppointmentID as AppointmentID,AppointmentDate as [Appointment Date], " +
                           "PaidFees as [Paid Fees], IsLocked as [Is Locked] from TestAppointments Where IsLocked=0";

            SqlCommand command = new SqlCommand(query, connect);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    DT.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return null;
            }
            finally
            {
                connect.Close();
            }

            return DT;
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return TestID;
            }
            finally
            {
                connection.Close();
            }

            return TestID;
        }
    }
}