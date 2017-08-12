using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.Helpers;

namespace Moosetrail.LaTeX.ElementsParser
{
    public class EnvelopeParser : LaTeXElementParser, LaTexElementParser<Envelope>
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators { get; }
        private static readonly IEnumerable<string> BeginCommands;

        static EnvelopeParser()
        {
            var beginCommands = new List<string>();
            foreach (var command in EnumUtil.GetValues<EnvelopeCommand>())
            {
                beginCommands.Add(@"\\begin{" + command + "}");
                beginCommands.Add("\\\\begin{" + command + "}");
                beginCommands.Add(@"\\\\begin{" + command + "}");
            }
            BeginCommands = beginCommands;
            CodeIndicators = new List<string>(BeginCommands);
        }

        IEnumerable<string> LaTexElementParser<Envelope>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public Envelope GetEmptyElement()
        {
            throw new NotSupportedException();
        }

        public void SetChildElement(Envelope element, params LaTeXElement[] children)
        {
            throw new System.NotImplementedException();
        }

        public Envelope ParseCode(StringBuilder code)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        LaTeXElement LaTeXElementParser.GetEmptyElement()
        {
            return GetEmptyElement();
        }

        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            throw new System.NotImplementedException();
        }

        LaTeXElement LaTeXElementParser.ParseCode(StringBuilder code)
        {
            return ParseCode(code);
        }

        
    }
}