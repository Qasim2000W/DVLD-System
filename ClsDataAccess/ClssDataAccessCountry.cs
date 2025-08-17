using System;
using System.Data.SqlClient;
using System.Data;

namespace ClsDataAccess
{
    public class ClssDataAccessCountry
    {
        public static bool GetCountryByName(string namecountry, ref string countryname, ref int idcountry)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from Countries where CountryName=@CountryName";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@CountryName", namecountry);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    countryname = (string)reader["CountryName"];
                    idcountry = (int)reader["CountryID"];
                }

                reader.Close();
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

        public static DataTable GetListRecoreds()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select CountryName from Countries";
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
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }

            return DT;
        }

    }
}