using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.ElementsParser;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Class that parses a code into objects
    /// </summary>
    internal class CodeParser
    {
        private readonly string _fullCode;
        private readonly List<LaTeXElement> _elements;
        private readonly List<LaTeXElementParser> _parserList;
        private readonly List<LaTeXElement> _elementsList;

        private readonly TextBodyParser _textParser;

        private readonly IReadOnlyDictionary<IEnumerable<string>, LaTeXElementParser> _laTeXElements
            = new Dictionary<IEnumerable<string>, LaTeXElementParser>
            {
                {
                    DocumentClassParser.CodeIndicators,
                    new DocumentClassParser()
                },
                {
                    DocumentParser.CodeIndicators,
                    new DocumentParser()
                },
                {
                    ChapterParser.CodeIndicators,
                    new ChapterParser()
                },
                {
                    SectionParser.CodeIndicators,
                    new SectionParser()
                },
                {
                    SubsectionParser.CodeIndicators,
                    new SubsectionParser()
                },
                {
                    SubsubsectionParser.CodeIndicators,
                    new SubsubsectionParser()
                },
                {
                    EnumerateParser.CodeIndicators,
                    new EnumerateParser()
                },
                {
                    ItemParser.CodeIndicators,
                    new ItemParser()
                },
                {
                    TextBodyParser.CodeIndicators,
                    new TextBodyParser()
                }
            };

        /// <summary>
        /// Creates a new instance of the CodeParser class
        /// </summary>
        /// <param name="code">The code to parse</param>
        public CodeParser(string code)
        {
            _fullCode = code;
            _elements = new List<LaTeXElement>();
            _parserList = new List<LaTeXElementParser>();
            _elementsList = new List<LaTeXElement>();
            _textParser = new TextBodyParser();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the code coudn't be fully parsed</exception>
        public IEnumerable<LaTeXElement> GetFullElementStack()
        {
            praseCode(new StringBuilder(_fullCode));
            return _elements;
        }

        private void praseCode(StringBuilder code)
        {
            code = TextFormatStrings.RemoveStartingFormating(code);

            if (string.IsNullOrWhiteSpace(code.ToString()))
                return;

            var parser = getParserForCode(code);

            if (codeCantBeProcessed(code, parser))
                throw new ArgumentException("The code couldnt be parsed: " + code);

            var element = parseElement(code, parser);

            setElementInHierarchy(element, parser);

            praseCode(code);
        }

        private LaTeXElementParser getParserForCode(StringBuilder code)
        {
            var parser =
                _laTeXElements.SingleOrDefault(x => x.Key.Any(pattern => Regex.IsMatch(code.ToString(), "^" + pattern))).Value;
            return parser;
        }

        private bool codeCantBeProcessed(StringBuilder code, LaTeXElementParser parser)
        {
            return parser == null && !_textParser.CodeStartsWithText(code);
        }

        private LaTeXElement parseElement(StringBuilder code, LaTeXElementParser parser)
        {
            var element = _textParser.CodeStartsWithText(code) ? _textParser.ParseCode(code) : parser.ParseCode(code);
            return element;
        }

        private void setElementInHierarchy(LaTeXElement element, LaTeXElementParser parser)
        {
            if (element != null && _elements.Count == 0)
                addElementToList(element, parser);
            else if (element != null)
                setElementToParent(element, parser);
            else
                backofElement(parser);
        }

     
        private void addElementToList(LaTeXElement element, LaTeXElementParser parser)
        {
            _elements.Add(element);
            addElementAndParserToTracking(element, parser);
        }

        private void addElementAndParserToTracking(LaTeXElement element, LaTeXElementParser parser)
        {
            if (parser != null)
            {
                _parserList.Add(parser);
                _elementsList.Add(element);
            }
        }

        private void setElementToParent(LaTeXElement element, LaTeXElementParser parser)
        {
            try
            {
                setElementToClosestParent(element, parser);
            }
            catch (ArgumentException)
            {
                serachForParent(element, parser);
            }
            catch (NotSupportedException)
            {
                serachForParent(element, parser);
            }
        }

        private void setElementToClosestParent(LaTeXElement element, LaTeXElementParser parser)
        {
            _parserList[_parserList.Count - 1].SetChildElement(_elementsList[_elementsList.Count - 1], element);
            addElementAndParserToTracking(element, parser);
        }

        private void serachForParent(LaTeXElement element, LaTeXElementParser parser)
        {
            var parentFound = false;
            for (var i = _parserList.Count - 2; i > -1; i--)
            {
                try
                {
                    _parserList[i].SetChildElement(_elementsList[i], element);

                    parentFound = true;
                    _elementsList.RemoveRange(i + 1, _elementsList.Count - i - 1);
                    _parserList.RemoveRange(i + 1, _parserList.Count - i - 1);
                    addElementAndParserToTracking(element, parser);

                    break;
                }
                catch (ArgumentException)
                {
                }
                catch (NotSupportedException)
                {
                }
            }

            if (!parentFound)
            {
                _parserList.Clear();
                _elementsList.Clear();

                addElementToList(element, parser);
            }
        }

        private void backofElement(LaTeXElementParser parser)
        {
            var searchingForType = parser.GetEmptyElement().GetType();
            for (var i = _elementsList.Count - 1; i >= 0; i--)
            {
                if (_elementsList[i].GetType() == searchingForType)
                {
                    _elementsList.RemoveRange(i +1, _elementsList.Count - i-1);
                    _parserList.RemoveRange(i + 1, _parserList.Count - i-1);
                }
            }
        }


        /// <summary>
        /// Extracts a specific element from the strig and removes in
        /// </summary>
        /// <param name="command">The command to get the value for</param>
        /// <param name="code">A stringbuilder holding the code to get the value from</param>
        /// <returns>
        /// The value of the command, otherwise an empy tring
        /// </returns>
        public static string SimpleContent(string command, StringBuilder code)
        {
            var match = Regex.Match(code.ToString(), @"\\" + command + @"\{([^}]+)\}");

            if (match.Success)
            {
                var startIndex = code.ToString().IndexOf(match.Value, StringComparison.Ordinal);
                code.Replace(match.Value, "", startIndex, match.Value.Length);
            }
                
            return match.Groups[1].Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeInsideTag(string command, string code)
        {
            var element = Regex.Match(code, @"\\begin\{(" + command + @")\}(.*|\s)\\end\{(" + command + @")\}");
            return element.Value;
        }

        public static string CreateCodeStartPattern(IEnumerable<string> codeIndicators)
        {
            var indicators = codeIndicators as IList<string> ?? codeIndicators.ToList();

            if (indicators.Count > 1)
            {
                return createMultipPossiblePattern(indicators);
            }
            else
            {
                return "^" + indicators.ElementAt(0);
            }
        }

        private static string createMultipPossiblePattern(IEnumerable<string> codeIndicators)
        {
            var sb = new StringBuilder("^(");

            foreach (var indicator in codeIndicators)
            {
                sb.AppendFormat("|{0}", indicator);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Remove(2, 1);
            sb.Append(")");

            return sb.ToString();
        }
    }
}