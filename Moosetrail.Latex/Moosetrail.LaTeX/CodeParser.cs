using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Class that parses a code into objects
    /// </summary>
    internal class CodeParser
    {
        private readonly IReadOnlyDictionary<IEnumerable<string>, LaTeXElement> _laTeXElements = new Dictionary<IEnumerable<string>, LaTeXElement>
        {
            {DocumentClassParser.CodeIndicators, new DocumentClass() }
        };

        /// <summary>
        /// Creates a new instance of the CodeParser class
        /// </summary>
        /// <param name="code">The code to parse</param>
        public CodeParser(string code)
        {
            
        }

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

        public static string CreateCodeStartPattern(IEnumerable<string> codeIndicators)
        {
            var sb = new StringBuilder("^(");

            foreach (var indicator in codeIndicators)
            {
                sb.AppendFormat("|{0}", indicator);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Remove(2, 1);
            sb.Append(")");

            return sb.ToString();
        }
    }
}