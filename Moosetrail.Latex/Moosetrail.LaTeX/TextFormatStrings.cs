using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Helperclass that holds handling of formating text in regular text
    /// </summary>
    public static class TextFormatStrings
    {
        public static List<string> List = new List<string>
        {
            @"\n\r",
            @"\r\n",
            @"\n\t",
            @"\t\n",
            @"\n",
            @"\r",
            @"\t",
        };

        public static string Remove(string str)
        {
            var sb = new StringBuilder(str);

            foreach (var format in List.Where(format => sb.ToString().StartsWith(format)))
            {
                sb.Remove(0, format.Length);
            }
           

            return sb.ToString();
        }
    }
}