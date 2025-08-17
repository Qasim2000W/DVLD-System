using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

namespace ClsDataAccess
{
    public class ClsApplicationData
    {
        public static int? AddNewRecored(int ApplicantPersonID,DateTime ApplicationDate,int ApplicationTypeID,
                                        byte ApplicationStatus,DateTime LastStatusDate,decimal PaidFees ,
                                        int CreatedByUserID)
        {
            int? ContactId = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(ClssDataConnection.connection))
                {
                    connect.Open();

                    string query = @"INSERT INTO Applications(ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,
                           LastStatusDate,PaidFees ,CreatedByUserID) VALUES(@ApplicantPersonID,@ApplicationDate,
                           @ApplicationTypeID,@ApplicationStatus, @LastStatusDate,@PaidFees ,@CreatedByUserID); 
                           select SCOPE_IDENTITY();";


                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        Object RESULT = command.ExecuteScalar();

                        if (RESULT != null && int.TryParse(RESULT.ToString(), out int INsertedID))
                        {
                            ContactId = INsertedID;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return ContactId;
            }

            return ContactId;
        }

        public static bool UbdateRecored(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
                                         byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        { 
            int? roweffected = 0;

            try
            {
                using (SqlConnection connect = new SqlConnection(ClssDataConnection.connection))
                {
                    connect.Open();


                    string query = @"UPDATE Applications 
                                   set
                                   [ApplicantPersonID] =@ApplicantPersonID      
                                  ,[ApplicationDate]   =@ApplicationDate
                                  ,[ApplicationTypeID] =@ApplicationTypeID
                                  ,[ApplicationStatus] =@ApplicationStatus
                                  ,[LastStatusDate]    =@LastStatusDate
                                  ,[PaidFees]          =@PaidFees  
                                  ,[CreatedByUserID]   =@CreatedByUserID 
                                   WHERE ApplicationID =@ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        roweffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return false;
            }

            return (roweffected !=null);
        }

        public static bool GetRecored(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte 
                                     ApplicationStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool isfound = false;

            try 
            {
                using (SqlConnection connect = new SqlConnection(ClssDataConnection.connection))
                {
                    connect.Open();

                    string query = "select *from Applications where ApplicationID=@ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    isfound = true;

                                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                                    PaidFees = (decimal)reader["PaidFees"];
                                    CreatedByUserID = (int)reader["CreatedByUserID"];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return isfound;
            }

            return isfound;
        }

        public static bool DeleteRecored(int ID)
        {
            int? roweffected = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(ClssDataConnection.connection))
                {
                    connect.Open();

                    string query = @"delete from Applications where ApplicationID=@ID";

                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@ID", ID);

                        roweffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return false;
            }

            return (roweffected !=null);
        }

        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            int? rowsAffected = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(ClssDataConnection.connection))
                {
                    connection.Open();

                    string query = @"Update  Applications set ApplicationStatus = @NewStatus, LastStatusDate = @LastStatusDate where ApplicationID=@ApplicationID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return false;
            }

            return (rowsAffected !=null);
        }

        public static int? GetActiveApplicationID(int PersonID, int LicenseClassID, int ApplicationStatus)
        {
            int? ActiveApplicationID = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(ClssDataConnection.connection))
                {
                    connection.Open();

                    string Query = "select ActiveApplicationID = Applications.ApplicationID from Applications inner join " +
                         "LocalDrivingLicenseApplications on Applications.ApplicationID=LocalDrivingLicenseApplications.ApplicationID" +
                         "where ApplicantPersonID=@ApplicantPersonID and LocalDrivingLicenseApplications.LicenseClassID" +
                         "=@LicenseClassID AND ApplicationStatus=@ApplicationStatus";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int ActiveID))
                        {
                            ActiveApplicationID = ActiveID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClsEventLog.EventLogger(ex.ToString(), ClsEventLog.ENTypeMessage.Error);
                return ActiveApplicationID;
            }
            
            return ActiveApplicationID;
        }
    }
}