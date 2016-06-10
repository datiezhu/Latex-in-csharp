using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    /// <summary>
    /// Parser for Enumerate code into an enumerate ojbect
    /// </summary>
    public class EnumerateParser : LaTeXElementParser, LaTexElementParser<Enumerate>
    {
        private static readonly int EndCommandLength = @"\end{enumerate}".Length;

        private static readonly IEnumerable<string> BeginCommands = new List<string>
        {
             @"\\begin{enumerate}",
            "\\\\begin{enumerate}",
            @"\\\\begin{enumerate}",
        };

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators = new List<string>(BeginCommands)
        {
            @"\\end{enumerate}",
            @"\\\\end{enumerate}"
        };

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<Enumerate>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public Enumerate GetEmptyElement()
        {
            return new Enumerate();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(Enumerate element, params LaTeXElement[] children)
        {
            if (!children.All(x => x is Item))
                throw new ArgumentException("Enumerate can only hold Item objects");
            else if (!children.Any())
                throw new ArgumentException("No child elements supplied to set as child");

            var enumerate = element;
            enumerate.ItemList.AddRange(children.Select(x => x as Item));
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
        public Enumerate ParseCode(StringBuilder code)
        {
            if (!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            if (Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(BeginCommands)))
                return createNewEnumerate(code);
            else
                return endEnumerate(code);
        }


        private static Enumerate createNewEnumerate(StringBuilder code)
        {
            var enumerate = new Enumerate();
            removeUntilFirstClose(code);

            return enumerate;
        }

        private static void removeUntilFirstClose(StringBuilder code)
        {
            while (code[0] != '}')
            {
                code.Remove(0, 1);
            }
            code.Remove(0, 1);
        }

        private static Enumerate endEnumerate(StringBuilder code)
        {
            removeUntilFirstClose(code);
            return null;
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        LaTeXElement LaTeXElementParser.GetEmptyElement()
        {
            return new Enumerate();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void LaTeXElementParser.SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            SetChildElement(element as Enumerate, children);
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
        LaTeXElement LaTeXElementParser.ParseCode(StringBuilder code)
        {
            return ParseCode(code);
        }
    }
}