using System;

namespace DVLD.Global_Classes
{
    public class ClsFormat
    {
        public static string DateToShort(DateTime dateTime)
        {
            return dateTime.ToString("dd/MMM/yyyy");
        }
    }
}
