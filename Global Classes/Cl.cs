using System;
using System.IO;
using System.Windows.Forms;
using Business;
using Microsoft.Win32;


namespace DVLD
{
    internal class ClsGlobal
    {
        public static ClsUsersBussiness CurrentUser = new ClsUsersBussiness();

        public static bool RegisterUser(string UserName, string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

            try
            {
                string ValueName = "UserName";
                Registry.SetValue(KeyPath, ValueName, UserName);
                ValueName = "Password";
                Registry.SetValue(KeyPath, ValueName,Password);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool GetDataUser(ref string UserName, ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

            try
            {
                string ValueName = "UserName";
                UserName = Registry.GetValue(KeyPath, ValueName, null) as string;
                ValueName = "Password";
                Password = Registry.GetValue(KeyPath, ValueName, null) as string;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}