using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX
{
    public static class CodeParser
    {
        public static string SimpleContent(string command, string code)
        {
            var match = Regex.Match(code, @"\\" + command + @"\{([^}]+)\}");
            return match.Groups[1].Value;
        }

        public static string CodeInsideTag(string command, string code)
        {
            var element = Regex.Match(code, @"\\begin\{(" + command + @")\}(.*|\s)\\end\{(" + command + @")\}");
            return element.Value;
        }
    }
}