using System;
using System.Data.SqlClient;
using System.Data;

namespace ClsDataAccess
{
    public class ClsDataUsers
    {
        public static DataTable GetListRecoredsUsers()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select *from UsersInfo";
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

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO Users(PersonID, UserName, Password, IsActive) 
                            VALUES(@PersonID, @UserName, @Password, @IsActive); 
                            select SCOPE_IDENTITY();";
                                               
            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

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

        public static bool UbdateRecored(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE Users 
            set
            [PersonID] =@PersonID
           ,[UserName] =@UserName
           ,[Password] =@Password
           ,[IsActive] =@IsActive
            WHERE UserID=@UserID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool GetrecoredByID(int UserID, ref int PersonID, ref string UserName, ref string Password,
                                          ref bool IsActive)                                                      
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from Users where UserID=@UserID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
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

        public static bool GetrecoredByUserName(string Username, ref int UserID, ref int PersonID, ref string Password,
                                                 ref bool IsActive)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from Users where UserName=@Username";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@Username", Username);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    Password = (string)reader["Password"];
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

        public static bool DeleteRecored(int UserID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"DELETE FROM Users WHERE UserID=@UserID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool ExistsIs(int UserID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select found=1 FROM Users WHERE UserID=@UserID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool ExistsIs(string Username)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select found=1 FROM Users WHERE UserName=@Username";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@UserName", Username);

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

        public static bool ExistsIsByPersonID(int PersonID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select found=1 FROM Users WHERE PersonID=@PersonID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(ClssDataConnection.connection);

            string query = @"Update Users set Password = @Password where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Password", NewPassword);

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