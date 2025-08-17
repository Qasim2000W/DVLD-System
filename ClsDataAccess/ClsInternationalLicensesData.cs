using System;
using System.Data;
using System.Data.SqlClient;


namespace ClsDataAccess
{
    public class ClsInternationalLicensesData
    {
        public static int AddNew(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, 
                                 DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"Update InternationalLicenses 
                             set IsActive=0
                             where DriverID=@DriverID;

                           INSERT INTO InternationalLicenses(ApplicationID, DriverID, IssuedUsingLocalLicenseID,IssueDate,
                           ExpirationDate, IsActive, CreatedByUserID) VALUES(@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,
                           @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID ); select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
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

        public static bool ExistsIs(int LicenseID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"Select found=1 from InternationalLicenses where IssuedUsingLocalLicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connect.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                    isfound = true;
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

            return isfound;
        }

        public static DataTable GetListForDriver(int DriverID)
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select InternationalLicenseID,ApplicationID, IssuedUsingLocalLicenseID as [L.License ID],IssueDate," +
                           "ExpirationDate,IsActive from InternationalLicenses where DriverID=@DriverID";
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

        public static bool GetRecored(int IssuedUsingLocalLicenseID, ref int ID, ref int ApplicationID, ref int DriverID,
                                      ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive,ref int CreatedByUserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"SELECT *FROM InternationalLicenses where IssuedUsingLocalLicenseID=@IssuedUsingLocalLicenseID";
            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    ID = (int)reader["InternationalLicenseID"];
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    IsActive = (bool)reader["IsActive"];
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

        public static bool GetRecoredByID(int ID, ref int IssuedUsingLocalLicenseID, ref int ApplicationID, ref int DriverID,
                                     ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"SELECT *FROM InternationalLicenses where InternationalLicenseID=@ID";
            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    IsActive = (bool)reader["IsActive"];
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

        public static DataTable GetList()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID as [L.License ID]," +
                           "IssueDate,ExpirationDate,IsActive from InternationalLicenses";

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select top 1 InternationalLicenseID from InternationalLicenses where DriverID=@DriverID and GETDATE() between IssueDate and 
                           ExpirationDate";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connect.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
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

            return InternationalLicenseID;
        }
    }
}