using System;
using System.Data;
using System.Data.SqlClient;


namespace ClsDataAccess
{
    public class ClsLicensesData
    {
        public static int AddNew(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                                 string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO Licenses(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees,
                           IsActive, IssueReason, CreatedByUserID) VALUES(@ApplicationID, @DriverID, @LicenseClass, @IssueDate, 
                           @ExpirationDate, @Notes, @PaidFees,@IsActive, @IssueReason, @CreatedByUserID); 
                           select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (string.IsNullOrEmpty(Notes))
            {
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Notes", Notes);
            }

            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
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

        public static bool UbdateRecored(int ID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, 
                     DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int roweffected = 0;

            string query = @"UPDATE Licenses 
            set
            [ApplicationID] =@ApplicationID
           ,[DriverID] = @DriverID
           ,[LicenseClass] =@LicenseClass
           ,[IssueDate] =@IssueDate
           ,[ExpirationDate] = @ExpirationDate
           ,[Notes]=@Notes
           ,[PaidFees] = @PaidFees
           ,[IsActive] = @IsActive
           ,[IssueReason] = @IssueReason
           ,[CreatedByUserID] = @CreatedByUserID
            WHERE LicenseID=@ID";


            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);
            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (string.IsNullOrEmpty(Notes))
            {
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Notes", Notes);
            }

            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool GetRecored(int ApplicationID, ref int LicenseID, ref int DriverID, ref int LicenseClass,
                                      ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, 
                                      ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = " SELECT* FROM Licenses Where ApplicationID=@ApplicationID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    LicenseID = (int)reader["LicenseID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"]==DBNull.Value)
                    {
                        Notes = string.Empty;
                    }
                    else
                    {
                        Notes = (string)reader["Notes"];
                    }
                    
                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
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

        public static bool GetRecoredByID(int LicenseID,  ref int ApplicationID, ref int DriverID, ref int LicenseClass,
                                      ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees,
                                      ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "SELECT * FROM Licenses Where LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] == DBNull.Value)
                    {
                        Notes = string.Empty;
                    }
                    else
                    {
                        Notes = (string)reader["Notes"];
                    }

                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
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

        public static DataTable GetListLicenseForPersonToShowing(int DriverID)
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "Select LicenseID, ApplicationID, LicenseClasses.ClassName, IssueDate, ExpirationDate, IsActive " +
                           "from Licenses Inner join LicenseClasses ON LicenseClasses.LicenseClassID=Licenses.LicenseClass " +
                           "where DriverID=@DriverID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static DataTable GetListForDriver(int DriverID)
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from Licenses where DriverID=@DriverID";
            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"SELECT Licenses.LicenseID FROM Licenses INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID WHERE Licenses.LicenseClass = 
                           @LicenseClass AND Drivers.PersonID = @PersonID And IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return LicenseID;
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }

        public static bool DeActiveLicense(int LicenseID)
        {
            int RowIsEffect = 0;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string Query = @"UPDATE Licenses set IsActive = 0 where LicenseID=@LicenseID";
                
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                RowIsEffect = command.ExecuteNonQuery();
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

            return (RowIsEffect > 0);
        }
    }
}