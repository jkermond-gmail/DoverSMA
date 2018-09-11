using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DoverUtilities
{
    public static class Utils
    {
        public static string CalculatePrevEndOfQtrDate(string sDate)
        {
            //CalculatePrevEndOfQtrDate("12/31/2016");
            //CalculatePrevEndOfQtrDate("9/30/2016");
            //CalculatePrevEndOfQtrDate("6/30/2016");
            //CalculatePrevEndOfQtrDate("3/31/2016");

            string str = sDate.Split(' ')[0].Trim();
            DateTime date = DateTime.ParseExact(str, "M/dd/yyyy", CultureInfo.InvariantCulture);

            int currQtrMonth = date.Month;
            int currQtrYear = date.Year;

            int prevQtrMonth = 0;
            int prevQtrDay = 0;
            int prevQtrYear = currQtrYear;

            switch (currQtrMonth)
            {
                case 3:
                    prevQtrMonth = 12;
                    prevQtrDay = 31;
                    prevQtrYear = currQtrYear - 1;
                    break;
                case 6:
                    prevQtrMonth = 3;
                    prevQtrDay = 31;
                    prevQtrYear = currQtrYear;
                    break;
                case 9:
                    prevQtrMonth = 6;
                    prevQtrDay = 30;
                    prevQtrYear = currQtrYear;
                    break;
                case 12:
                    prevQtrMonth = 9;
                    prevQtrDay = 30;
                    prevQtrYear = currQtrYear;
                    break;
            }

            DateTime prevEndOfQtrDate = new DateTime(prevQtrYear, prevQtrMonth, prevQtrDay);

            return (prevEndOfQtrDate.ToString("MM/dd/yyyy"));
        }

        public static string CalculateNextEndOfQtrDate(string sDate)
        {
            //CalculatePrevEndOfQtrDate("12/31/2016");
            //CalculatePrevEndOfQtrDate("9/30/2016");
            //CalculatePrevEndOfQtrDate("6/30/2016");
            //CalculatePrevEndOfQtrDate("3/31/2016");

            string str = sDate.Split(' ')[0].Trim();
            DateTime date = DateTime.ParseExact(str, "M/dd/yyyy", CultureInfo.InvariantCulture);

            int currQtrMonth = date.Month;
            int currQtrYear = date.Year;

            int nextQtrMonth = 0;
            int nextQtrDay = 0;
            int nextQtrYear = currQtrYear;

            switch (currQtrMonth)
            {
                case 3:
                    nextQtrMonth = 6;
                    nextQtrDay = 30;
                    nextQtrYear = currQtrYear;
                    break;
                case 6:
                    nextQtrMonth = 9;
                    nextQtrDay = 30;
                    nextQtrYear = currQtrYear;
                    break;
                case 9:
                    nextQtrMonth = 12;
                    nextQtrDay = 31;
                    nextQtrYear = currQtrYear;
                    break;
                case 12:
                    nextQtrMonth = 3;
                    nextQtrDay = 31;
                    nextQtrYear = currQtrYear + 1;
                    break;
            }

            DateTime nextEndOfQtrDate = new DateTime(nextQtrYear, nextQtrMonth, nextQtrDay);

            return (nextEndOfQtrDate.ToString("MM/dd/yyyy"));
        }

#if Commented
        public static decimal ConvertStringToDecimalOld(string decimalString, decimal defaultReturnValue)
        {
            decimal result;
            if (decimal.TryParse(decimalString.Trim(), out result))
            {
                return result;
            }

            return defaultReturnValue;
        }
#endif

        public static decimal ConvertStringToDecimal(string stringVal)
        {
            decimal decimalVal = 0;

            try
            {
                decimalVal = System.Convert.ToDecimal(stringVal);
                //System.Console.WriteLine(
                //    "The string as a decimal is {0}.", decimalVal);
            }
            catch (System.OverflowException)
            {
                System.Console.WriteLine(
                    "The conversion from string to decimal overflowed |" + stringVal);
            }
            catch (System.FormatException)
            {
                //System.Console.WriteLine(
                //    "The string is not formatted as a decimal |" + stringVal);
            }
            catch (System.ArgumentNullException)
            {
                //System.Console.WriteLine(
                //    "The string is null.");
            }

            // Decimal to string conversion will not overflow.
            stringVal = System.Convert.ToString(decimalVal);
            //System.Console.WriteLine(
            //    "The decimal as a string is {0}.", stringVal);
            return (decimalVal);
        }



    }
}
