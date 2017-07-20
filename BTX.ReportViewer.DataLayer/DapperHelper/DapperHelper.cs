using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTX.ReportViewer.DataLayer.DapperHelper
{
    class DapperHelper
    {
        public static string StringToMdxMember(string inputstring, string mdxwrapper)
        {
            char[] delimitedChar = {','};
            string[] inputList = inputstring.Split(delimitedChar);
            string output = string.Empty;
            
            if (inputList.Length > 0 && !String.IsNullOrEmpty(inputstring))
            {
                foreach (string input in inputList)
                {
                    output += mdxwrapper + @".&[" + input + @"],";
                }
            }
            
            if (String.IsNullOrEmpty(output))
            {
                return mdxwrapper + @".[All]";
            }
            else
            {
                return output.TrimEnd(output[output.Length - 1]);
            }
        }

        public static string UnitSizeToMdxMember(string unitsizes)
        {
            char[] delimitedChar = { ',' };
            string[] inputList = unitsizes.Split(delimitedChar);
            string output = string.Empty;
            int unitsize = 0;

            if (inputList.Length > 0 && !String.IsNullOrEmpty(unitsizes))
            {
                foreach (string input in inputList)
                {
                    if (int.TryParse(input, out unitsize))
                    {
                        output += @"[Product].[Volume Per Unit ML].&[" + UnitSizeMdxConverter(unitsize) + @"],";
                    }
                }
            }

            if (String.IsNullOrEmpty(output))
            {
                return @"[Product].[Volume Per Unit ML].[All]";
            }
            else
            {
                return output.TrimEnd(output[output.Length - 1]);
            }
        }

        private static string UnitSizeMdxConverter(int unitsize)
        {
            string tmp = unitsize.ToString("G1", CultureInfo.InvariantCulture);
            string stringtoparse = tmp.Substring(tmp.Length - 1, 1);
            int pow = 0;
            string output = string.Empty;

            if (unitsize < 10)
            {
                output = unitsize.ToString() + @".";
            } else
            {
                if (int.TryParse(stringtoparse, out pow))
                {
                    output = (unitsize / Math.Pow(10, pow)).ToString();
                }
                if (output.Length == 1)
                    output += ".";
                output += "E" + stringtoparse;
            }

            return output;
        }
    }
}
