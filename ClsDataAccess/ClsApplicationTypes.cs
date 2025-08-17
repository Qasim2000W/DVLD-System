using System.Data.SqlClient;
using System.Data;
using System;

namespace ClsDataAccess
{
    public class ClsApplicationTypes
    {
        public static DataTable GetListApplication()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select *from ApplicationTypes";
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

        public static bool UbdateRecored(int ID,string Title, decimal Fees)
        {

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE ApplicationTypes 
            set
            [ApplicationTypeTitle] =@Title 
           ,[ApplicationFees] = @Fees
            WHERE ApplicationTypeID=@ID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);
           
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

        public static bool GetrecoredByID(int ID,ref string Title,ref decimal Fees)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from ApplicationTypes where ApplicationTypeID=@ID";
            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    Title = (string)reader["ApplicationTypeTitle"];
                    Fees = (decimal)reader["ApplicationFees"];
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
    }
}