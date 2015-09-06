using System;
using System.Collections.Generic;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Parser class that parses Latex code and returns an object hirarcy that represnts it
    /// </summary>
    public static class LatexParser
    {
        /// <summary>
        /// Parses latex code into c# objects representing the code
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <returns>A hirarcy of elements parsed from the code</returns>
        /// <exception cref="ArgumentException">Thrown if the code coudn't be fully parsed</exception>
        public static IEnumerable<LaTeXElement> ParseCode(string code)
        {
            var parser = new CodeParser(code);
            return parser.GetFullElementStack();
        }
    }
}