using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public class ClsUtil
    {
        public static bool CreateFileIfDoesNotExists(string SourcPath)
        {
            if (!Directory.Exists(SourcPath))
            {
                try
                {
                    Directory.CreateDirectory(SourcPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;
        }

        public static string GenerateGuid()
        {
            Guid guid = Guid.NewGuid();

            return guid.ToString();
        }

        public static string ReplcementfileWithNewGuid(string SourcPath)
        {
            FileInfo fileInfo = new FileInfo(SourcPath);
            string Fileextense = fileInfo.Extension;
            return GenerateGuid() + Fileextense;
        }

        public static bool CopyImage(ref string SourcPath)
        {
            
            string DestinationFolder = @"H:\DVLD\PICTURES PEOPLE\";

            if (!CreateFileIfDoesNotExists(DestinationFolder))
            {
                return false;
            }

            string DestinFile = DestinationFolder + ReplcementfileWithNewGuid(SourcPath);

            try
            {
                File.Copy(SourcPath, DestinFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourcPath = DestinFile;
            return true;
        }

        public static bool DeleteImage(string SourcPath)
        {
            if (SourcPath != "")
            {
                try
                {
                    File.Delete(SourcPath);
                    return true;
                }
                catch (IOException iox)
                {
                    MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
