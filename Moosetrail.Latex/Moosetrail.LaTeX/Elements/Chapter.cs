using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moosetrail.LaTeX.Elements
{
    public class Chapter
    {
        public Chapter()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get/Set the name of the chapter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set the elements of the chapter
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }

        /// <summary>
        /// Parse the code to set the information in the document. 
        /// Must begin with \chapter
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <exception cref="ArgumentException">The the code doesn't begin with \chapter</exception>
        internal void ParseCode(string code)
        {
            _code = new StringBuilder(code);
            //Name = StandardLaTeXParser.SpecificCommand(_code, @"\\chapter\{([^}]+)\}");
            Elements = LaTeXElements.GenerateElementsFromCode(_code.ToString()).ToList();
        }

        private StringBuilder _code;
    }
}