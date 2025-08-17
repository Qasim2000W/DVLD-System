using System.Data.SqlClient;
using System.Data;
using System;

namespace ClsDataAccess
{
    public class ClsTestTypeData
    {
        public static DataTable GetList()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select *from TestTypes";
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

        public static bool UbdateRecored(int ID, string Title, string Description, decimal Fees)
        {

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE TestTypes 
            set
            [TestTypeTitle] =@Title 
           ,[TestTypeDescription] = @Description
           ,[TestTypeFees]=@Fees
            WHERE TestTypeID=@ID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
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

        public static bool GetrecoredByID(int ID, ref string Title, ref string Description, ref decimal Fees)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from TestTypes where TestTypeID=@ID";
            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    Title = (string)reader["TestTypeTitle"];
                    Description = (string)reader["TestTypeDescription"];
                    Fees = (decimal)reader["TestTypeFees"];
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