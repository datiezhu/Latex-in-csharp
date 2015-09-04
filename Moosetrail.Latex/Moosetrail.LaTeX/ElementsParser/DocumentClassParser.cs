using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    public class DocumentClassParser : LaTeXElementParser
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators => new List<string>(packageIndicators)
        {
            @"\\documentclass"
        };

        private static IEnumerable<string> packageIndicators => new List<string>
        {
            @"\\usepackage\{([^}]+)\}",
            @"\\usepackage\[([^}]+)\}"
        };

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            if (!children.Any())
                throw new ArgumentException("No document supplied to set as child");
            else if (children.Length > 1)
                throw new ArgumentException("More than one document supplied, not accepted");
            else if (!(children[0] is Document))
                throw new ArgumentException("The supplied element wasn't a Document, only Document is allowed");
            else
                ((DocumentClass)element).Document = (Document)children[0];
        }

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <param name="element">The element to parse the code into</param>
        /// <returns>
        /// What is left of the string after the parser is done. 
        /// The code that have been parsed into the object will have been removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        public string ParseCode(string code, LaTeXElement element)
        {
            if (!Regex.IsMatch(code, CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            var codeProcess = new StringBuilder(code);

            setPackages(codeProcess, element as DocumentClass);

            return codeProcess.ToString();
        }

        private void setPackages(StringBuilder code, DocumentClass docClass)
        {
            foreach (var indicator in packageIndicators)
            {
                addUsePackagesForFormat(code, indicator, docClass);
            }
        }

        private void addUsePackagesForFormat(StringBuilder code, string pattern, DocumentClass docClass)
        {
            var matches = Regex.Matches(code.ToString(), pattern);
            foreach (var package in from object match in matches select new StringBuilder(match.ToString()))
            {
                code.Replace(package.ToString(), "");
                package.Replace("\\usepackage", "");
                package.Replace("{", "");
                package.Replace("}", "");
                docClass.UsePackages.Add(package.ToString());
            }
        }
    }
}