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
            @"\\documentclass",
            @"\\\\documentclass"
        };

        private static IEnumerable<string> packageIndicators => new List<string>
        {
            @"\\usepackage\{([^}]+)\}",
            @"\\usepackage\[([^}]+)\}",
            @"\\\\usepackage\{([^}]+)\}",
            @"\\\\usepackage\[([^}]+)\}"
        };

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public LaTeXElement GetEmptyElement()
        {
            return new DocumentClass();
        }

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
                throw new ArgumentException("No child document supplied to set as child");
            else if (children.Length > 1)
                throw new ArgumentException("More than one child element supplied, not accepted");
            else if (!(children[0] is Document))
                throw new ArgumentException("The supplied child wasn't a Document, only Document is allowed");
            else if(!(element is DocumentClass))
                throw new ArgumentException("The supplied element wasn't a Document, only Document is allowed");
            else
                ((DocumentClass)element).Document = (Document)children[0];
        }

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// The newly parsed object. The string builder will also have been updted, the code parsed is removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        public LaTeXElement ParseCode(StringBuilder code)
        {
            if (!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            removeDocumentClassInfo(code);

            var docClass = new DocumentClass();
            setPackages(code, docClass);

            return docClass;
        }

        private static void removeDocumentClassInfo(StringBuilder codeProcess)
        {
            var docuStartWithoutLineBreak = Regex.Match(codeProcess.ToString(), @"(\\documentclass(.*?)\\)");
            if (docuStartWithoutLineBreak.Success)
            {
                codeProcess.Replace(docuStartWithoutLineBreak.Groups[2].Value, "");
            }   
            else
            {
                var docStartWithLinebreak = Regex.Match(codeProcess.ToString(), @"\\documentclass(.*)");
                if (docStartWithLinebreak.Success)
                    codeProcess.Replace(docStartWithLinebreak.Value, "");
            }

            codeProcess.Replace("\\documentclass", "");

        }

        private static void setPackages(StringBuilder code, DocumentClass docClass)
        {
            foreach (var indicator in packageIndicators)
            {
                addUsePackagesForFormat(code, indicator, docClass);
            }
        }

        private static void addUsePackagesForFormat(StringBuilder code, string pattern, DocumentClass docClass)
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