using System;
using System.Data.SqlClient;
using System.Data;

namespace ClsDataAccess
{
    public class ClsPeoplleData
    {
        public static DataTable GetListRecoredsPeople()
        {
            DataTable DT = new DataTable();

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select *from Datagrd";
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

        public static int AddNewRecored(string NationalNo,string firstname, string secondName, string thirdname, string lastname,string email, string phone,
                                        string address, DateTime DateOfbirth, int NationalityCountryID, byte Gender,string ImagePath)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int ContactId = -1;

            string query = @"INSERT INTO People(NationalNo,FirstName,SecondName,ThirdName ,LastName,DateOfbirth ,Gendor ,
                           Address ,Phone ,Email ,NationalityCountryID,ImagePath) VALUES(@NationalNo,@FirstName,@secondName,
                           @ThirdName ,@LastName,@DateOfbirth,@Gender,@Address,@Phone,@Email,@NationalityCountryID,
                           @ImagePath); select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", firstname);
            command.Parameters.AddWithValue("@secondName", secondName);
            command.Parameters.AddWithValue("@ThirdName", thirdname);
            command.Parameters.AddWithValue("@LastName", lastname);
            command.Parameters.AddWithValue("@DateOfbirth", DateOfbirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (string.IsNullOrEmpty(ImagePath))
            {
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
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

        public static bool UbdateRecored(int PersonID, string NationalNo, string firstname, string secondName,  string thirdname, string lastname,string email, 
                                         string phone, string address, DateTime DateOfbirth, int NationalityCountryID,byte Gender, string ImagePath)
        {

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"UPDATE People 
            set
            [NationalNo] =@NationalNo
           ,[FirstName] = @FirstName
           ,[SecondName] =@secondName
           ,[ThirdName] =@thirdname
           ,[LastName] = @LastName
           ,[DateOfBirth]=@DateOfbirth
           ,[Gendor] = @Gender
           ,[Address] = @Address
           ,[Phone] = @Phone
           ,[Email] = @Email
           ,[NationalityCountryID]=@NationalityCountryID
           ,[ImagePath] =@ImagePath
            WHERE PersonID=@PersonID";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", firstname);
            command.Parameters.AddWithValue("@secondName", secondName);
            command.Parameters.AddWithValue("@ThirdName", thirdname);
            command.Parameters.AddWithValue("@LastName", lastname);
            command.Parameters.AddWithValue("@DateOfbirth", DateOfbirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (string.IsNullOrEmpty(ImagePath))
            {
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }

            try
            {
                connect.Open();
                roweffected = command.ExecuteNonQuery();
            }

            catch(Exception ex)
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

        public static bool GetrecoredByID(int ID,ref int PersonID, ref string NationalNo, ref string firstname, ref string secondName,ref string thirdname, 
                                         ref string lastname, ref string email, ref string phone, ref string address, ref DateTime DateOfbirth, ref int 
                                        NationalityCountryID, ref byte Gender, ref string ImagePath, ref string CountryName)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from PersonInfo where PersonID=@ID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = (int)reader["PersonID"];
                    NationalNo= (string)reader["NationalNo"];
                    firstname = (string)reader["firstname"];
                    secondName = (string)reader["secondName"];
                    thirdname = (string)reader["thirdname"];
                    lastname = (string)reader["lastname"];
                    email = (string)reader["email"];
                    phone = (string)reader["phone"];
                    address = (string)reader["address"];
                    DateOfbirth = (DateTime)reader["DateOfbirth"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    Gender = (byte)reader["Gendor"];
                    CountryName= (string)reader["CountryName"];

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
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

        public static bool GetrecoredByNationalNo(string NationalNum, ref int PersonID, ref string NationalNo, ref string firstname,ref string secondName, ref 
                                                 string thirdname, ref string lastname,ref string email, ref string phone, ref string address, ref DateTime 
                                                DateOfbirth,ref int NationalityCountryID, ref byte Gender, ref string ImagePath, ref string CountryName)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = "select * from PersonInfo where NationalNo=@NationalNum";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@NationalNum", NationalNum);

            try
            {
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = (int)reader["PersonID"];
                    NationalNo = (string)reader["NationalNo"];
                    firstname = (string)reader["firstname"];
                    secondName = (string)reader["secondName"];
                    thirdname = (string)reader["thirdname"];
                    lastname = (string)reader["lastname"];
                    email = (string)reader["email"];
                    phone = (string)reader["phone"];
                    address = (string)reader["address"];
                    DateOfbirth = (DateTime)reader["DateOfbirth"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    Gender = (byte)reader["Gendor"];
                    CountryName = (string)reader["CountryName"];

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
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

        public static bool ExistsIs(int PersonID)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select found=1 FROM People WHERE PersonID=@PersonID";

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

        public static bool ExistsIs(string NationalNo)
        {
            bool isfound = false;

            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            string query = @"select found=1 FROM People WHERE NationalNo=@NationalNo";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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

        public static bool DeleteRecored(int PersonID)
        {
            SqlConnection connect = new SqlConnection(ClssDataConnection.connection);

            int roweffected = 0;

            string query = @"DELETE FROM People WHERE PersonID=@PersonID";

            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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
    }
}