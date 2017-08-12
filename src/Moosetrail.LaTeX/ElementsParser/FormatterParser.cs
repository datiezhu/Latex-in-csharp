using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.ElementRules;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.Exceptions;
using Moosetrail.LaTeX.Helpers;

namespace Moosetrail.LaTeX.ElementsParser
{
    public class FormatterParser : LaTeXElementParser, LaTexElementParser<Formatter>
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators { get; }
        private static readonly IEnumerable<string> BeginCommands;

        static FormatterParser()
        {
            var beginCommands = new List<string>();
            foreach (var command in EnumUtil.GetValues<FormatterCommand>())
            {
                beginCommands.Add(@"\\" + command);
                beginCommands.Add("\\\\" + command);
                beginCommands.Add(@"\\\\" + command);
            }
            BeginCommands = beginCommands;
            CodeIndicators = new List<string>(BeginCommands);
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<Formatter>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public Formatter GetEmptyElement()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(Formatter element, params LaTeXElement[] children)
        {
            if(children.Length == 0)
                throw new ArgumentException("No child elements supplied to set as child");

            var rules = ElementsRulesFactory.GetRules(element.Type);
            if(higherRankRuleFails(children, rules) || containSelfRuleFails(children, rules, element.Type))
                throw new LaTeXException(rules.ElementIsHigherRankingExceptionMessage);

            element.InnerElements.AddRange(children);
        }

        private static bool higherRankRuleFails(LaTeXElement[] children, Rules rules)
        {
            return children.Any(x => rules.HigherRankingElements.Any(h => h.GetType() == x.GetType()));
        }

        private static bool containSelfRuleFails(LaTeXElement[] children, Rules rules, FormatterCommand selfCommandType)
        {
            return !rules.AllowSelfInside && children.Any(x => x is Formatter && ((Formatter)x).Type == selfCommandType);
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
        public Formatter ParseCode(StringBuilder code)
        {
            if (!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            var type = getFormatterType(code.ToString());
            var rules = ElementsRulesFactory.GetRules(type);

            if (!rules.AllowOtherElementsInside)
            {
                var text = new TextBody(CodeParser.SimpleContent(type.ToString(), code));
                return new Formatter(type){InnerElements = new List<LaTeXElement>{text}};
            }

            throw new System.NotImplementedException();
        }

        private FormatterCommand getFormatterType(string code)
        {
            var match = Regex.Match(code, @"\\([a-z]*){");
            switch (match.Groups[1].Value)
            {
                case "chapter": return FormatterCommand.chapter;
                default:
                    throw new NotSupportedException();
            }
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
            return GetEmptyElement();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            var formatter = element as Formatter;
            if(formatter != null)
                SetChildElement(formatter, children);
            else 
                throw new ArgumentException("The supplied element wasn't a Formatter, only Formatter is allowed");
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