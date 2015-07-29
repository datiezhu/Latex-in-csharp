using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.Elements
{
    public class CustomCommand : LaTeXElement
    {
        public string CommandName { get; set; }

        public string Name { get; set; }

        public IEnumerable<LaTeXElement> Content { get; set; }

        internal void ParseCode(string code)
        {
            var command = Regex.Match(code, @"\\begin\{(.*?)\}");
            CommandName = command.Groups[1].Value;
            var name = Regex.Match(code, @"\\begin\{.*?\}\[(.*?)\]");
            Name = name.Groups[1].Value;

            code = code.Replace(name.Value != "" ? name.Value : command.Value, "");

            code = code.Replace(@"\end{" + CommandName + "}", "");

            Content = LaTeXElements.GenerateElementsFromCode(code);
        }
    }
}