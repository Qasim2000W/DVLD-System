using System;
using System.Data;
using System.Data.SqlClient;


namespace ClsDataAccess
{
    public class ClsDetainedLicenseData
    {
        public static int AddNew(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO DetainedLicenses(LicenseID, DetainDate, FineFees, CreatedByUserID) VALUES(@LicenseID, @DetainDate, @FineFees, @CreatedByUserID
                           );select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
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

        public static bool UbdateRecored(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE DetainedLicenses 
            set
            [LicenseID] =@LicenseID
           ,[DetainDate] =@DetainDate
           ,[FineFees] =@FineFees
           ,[CreatedByUserID] =@CreatedByUserID 
            WHERE DetainID=@DetainID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
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

        public static bool GetRecored(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref decimal FineFees,
                                     ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID,
                                     ref int ReleaseApplicationID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"Select * from DetainedLicenses where LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connect);


            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    IsReleased = (bool)reader["IsReleased"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    FineFees = (decimal)reader["FineFees"];
                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    if (reader["ReleasedByUserID"] == DBNull.Value)
                    {
                        ReleasedByUserID = -1;
                    }
                    else
                    {
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];
                    }

                    if (reader["ReleaseDate"] == DBNull.Value)
                    {
                        ReleaseDate = DateTime.MaxValue;
                    }
                    else
                    {
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];
                    }

                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                    {
                        ReleaseApplicationID = -1;
                    }
                    else
                    {
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                    }
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

            string query = "select *from ListDetainedLicense";
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

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string Query = "select IsDetained=1 from DetainedLicenses where LicenseID=@LicenseID and IsReleased=0";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (Result!=null)
                {
                    IsFound = Convert.ToBoolean(Result);
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return IsFound;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int RowsEffected = 0;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"UPDATE dbo.DetainedLicenses
                             SET IsReleased = 1, 
                             ReleaseDate = @ReleaseDate, 
                             ReleaseApplicationID = @ReleaseApplicationID   
                             WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                RowsEffected = command.ExecuteNonQuery();

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

            return (RowsEffected > 0);
        }
    }
}