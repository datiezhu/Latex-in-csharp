﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    /// <summary>
    /// Parser for subsections
    /// </summary>
    public class SubsectionParser : LaTeXElementParser
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        public static IEnumerable<string> CodeIndicators = new List<string>
        {
            @"\\subsection"
        }; 

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            if (children.Any(x => x is DocumentClass) || children.Any(x => x is Document) ||
                children.Any(x => x is Chapter) || children.Any(x => x is Section))
                throw new ArgumentException("A Subsection can't have a DocumentClass, Document, Chapter or Section as a child");
            else if (!(element is Subsection))
                throw new ArgumentException("The supplied element wasn't a Subsection, only Subsection is allowed");
            else if (!children.Any())
                throw new ArgumentException("No child elements supplied to set as child");

            var doc = (Subsection)element;
            doc.Elements.AddRange(children);
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
            if(!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            var subsection = new Subsection
            {
                Name = CodeParser.SimpleContent("subsection", code)
            };

            return subsection;
        }
    }
}