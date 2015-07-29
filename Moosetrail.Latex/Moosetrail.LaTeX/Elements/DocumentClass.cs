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
        private string codeInicator = @"\\documentclass";

        /// <summary>
        /// Creates a new intance of the DocumentClass without any data set
        /// </summary>
        public DocumentClass()
        {
            UsePackages = new List<string>();
        }

        /// <summary>
        /// Creates a nwe instance of the DocumentClass and 
        /// parses the latex code and adding all the informaton that could be pased
        /// </summary>
        /// <param name="latexCode">The latex code to create object for</param>
        public DocumentClass(string latexCode)
        {
            UsePackages = new List<string>();

            if (!Regex.IsMatch(latexCode, "^" + codeInicator))
                return;

            setPackages(latexCode);
            Document = new Document(latexCode);
        }

        private void setPackages(string code)
        {
            addUsePackagesForFormat(code, @"\\usepackage\{([^}]+)\}");
            addUsePackagesForFormat(code, @"\\usepackage\[([^}]+)\}");
        }

        private void addUsePackagesForFormat(string code, string pattern)
        {
            var matches = Regex.Matches(code, pattern);
            foreach (var package in from object match in matches select new StringBuilder(match.ToString()))
            {
                package.Replace("\\usepackage", "");
                package.Replace("{", "");
                package.Replace("}", "");
                UsePackages.Add(package.ToString());
            }
        }

        /// <summary>
        /// Get/Set a list of the packages the document uses
        /// </summary>
        public List<string> UsePackages { get; set; }

        /// <summary>
        /// Get/Set the document of the document class
        /// </summary>
        public Document Document { get; set; }
    }
}