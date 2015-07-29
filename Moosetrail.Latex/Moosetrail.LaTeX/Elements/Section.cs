using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moosetrail.LaTeX.Elements
{
    public class Section : LaTeXElement
    {
        public Section()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get/Set the name of the section
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set the elements of the section
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }

        /// <summary>
        /// Parse the code to set the information in the document. 
        /// Must begin with \section
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <exception cref="ArgumentException">The the code doesn't begin with \section</exception>
        internal void ParseCode(string code)
        {
            _code = new StringBuilder(code);
            //Name = StandardLaTeXParser.SpecificCommand(_code, @"\\section\{([^}]+)\}");
            Elements = LaTeXElements.GenerateElementsFromCode(_code.ToString()).ToList();
        }

        private StringBuilder _code;
    }
}