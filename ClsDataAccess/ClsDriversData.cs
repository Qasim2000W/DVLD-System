using System;
using System.Data.SqlClient;
using System.Data;

namespace ClsDataAccess
{
    public class ClsDriversData
    {
        public static int AddNew(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO Drivers(PersonID, CreatedByUserID, CreatedDate) 
                            VALUES(@PersonID, @CreatedByUserID, @CreatedDate); 
                            select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

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

        public static bool GetRecored(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"Select * from Drivers where PersonID=@PersonID";

            SqlCommand command = new SqlCommand(query, connect);


            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static bool GetRecoredByID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"Select * from Drivers where DriverID=@DriverID";

            SqlCommand command = new SqlCommand(query, connect);


            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static bool ExistsIs(int PersonID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"Select found=1 from Drivers where PersonID=@PersonID";

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

        public static DataTable GetList()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "Select *from DriversManage";
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
    }
}