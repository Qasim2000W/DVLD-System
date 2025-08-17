using System.Configuration;
namespace ClsDataAccess
{
    internal class ClssDataConnection
    {
        static public string connection =ConfigurationManager.ConnectionStrings["DBconnection"].ToString();
    }
}