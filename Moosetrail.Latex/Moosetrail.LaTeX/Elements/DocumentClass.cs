using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents a documet class in latex (container for all other code)
    /// Contains all the data from \documentclass up unti \document
    /// </summary>
    public class DocumentClass : LaTeXElement
    {
        private List<string> _usePackages;

        /// <summary>
        /// Creates a new intance of the DocumentClass without any data set
        /// </summary>
        public DocumentClass()
        {
            UsePackages = new List<string>();
        }

        /// <summary>
        /// Get/Set a list of the packages the document uses
        /// </summary>
        public IEnumerable<string> UsePackages
        {
            get { return _usePackages; }
            set { _usePackages = value.ToList(); }
        }

        /// <summary>
        /// Get/Set the document of the document class
        /// </summary>
        public Document Document { get; private set; }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElement.CodeIndicators => CodeIndicators;

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
        /// Sets the child elements of the current element
        /// </summary>
        /// <param name="elements">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the the element isn't a document or there is more than one element to add</exception>
        public void SetChildElement(params LaTeXElement[] elements)
        {
            if(!elements.Any())
                throw new ArgumentException("No document supplied to set as child");
            else if(elements.Length > 1)
                throw new ArgumentException("More than one document supplied, not accepted");
            else if(!(elements[0] is Document))
                throw new ArgumentException("The supplied element wasn't a Document, only Document is allowed");
            else
                Document = (Document) elements[0];
        }

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <returns>
        /// What is left of the string after the parser is done. 
        /// The code that have been parsed into the object will have been removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators</exception>
        public string ParseCode(string code)
        {
            if (!Regex.IsMatch(code, CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            var codeProcess = new StringBuilder(code);
        
            setPackages(codeProcess);

            return codeProcess.ToString();
        }
     
        private void setPackages(StringBuilder code)
        {
            foreach (var indicator in packageIndicators)
            {
                addUsePackagesForFormat(code, indicator);
            }
        }

        private void addUsePackagesForFormat(StringBuilder code, string pattern)
        {
            var matches = Regex.Matches(code.ToString(), pattern);
            foreach (var package in from object match in matches select new StringBuilder(match.ToString()))
            {
                code.Replace(package.ToString(), "");
                package.Replace("\\usepackage", "");
                package.Replace("{", "");
                package.Replace("}", "");
                _usePackages.Add(package.ToString());
            }
        }
    }
}