using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    public class EnumerateParser : LaTeXElementParser
    {
        private const string BeginCommand = @"\\begin{enumerate}";
        private static readonly int BeginCommandLength = BeginCommand.Length - 1;
        private static readonly int EndCommandLength = @"\end{enumerate}".Length;

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators = new List<string>
        {
            BeginCommand,
            @"\\end{enumerate}"
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
            if(!children.All(x => x is Item))
                throw new ArgumentException("Enumerate can only hold Item objects");
            else if(!(element is Enumerate))
                throw new ArgumentException("The supplied element wasn't a Enumerate, only Enumerate is allowed");
            else if(!children.Any())
                throw new ArgumentException("No child elements supplied to set as child");

            var enumerate = (Enumerate) element;
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
        public LaTeXElement ParseCode(StringBuilder code)
        {
            
           if (!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            if (Regex.IsMatch(code.ToString(), "^" + BeginCommand))
                return createNewEnumerate(code);
            else
                return endEnumerate(code);
        }

     
        private static LaTeXElement createNewEnumerate(StringBuilder code)
        {
            var enumerate = new Enumerate();
            code.Remove(0, BeginCommandLength);

            return enumerate;
        }

        private static LaTeXElement endEnumerate(StringBuilder code)
        {
            code.Remove(0, EndCommandLength);
            return null;
        }

    }
}