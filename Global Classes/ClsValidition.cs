using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class ClsValidition
    {
        public static bool ValditeEmail(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(emailAddress);
        }

        public static bool ValditeInteger(string number)
        {
            var pattern = @"^[0-9]*$";
            var regex = new Regex(pattern);

            return regex.IsMatch(number);
        }

        public static bool ValditeFloat(string number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(number);
        }

        public static bool IsNumber(string number)
        {
            return (ValditeInteger(number)||ValditeFloat(number));
        }
    }
}