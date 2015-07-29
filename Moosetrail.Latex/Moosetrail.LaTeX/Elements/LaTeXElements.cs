using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.Elements
{
    public static class LaTeXElements
    {
        public static List<string> List = new List<string>
        {
            @"\documentclass",
            @"\chapter",
            @"\section",
            @"\subsection",
        };

        public static List<string> ContentElements = new List<string>
        {
            "enumerate",
        };

        public static IEnumerable<LaTeXElement> GenerateElementsFromCode(string code)
        {
            //code = StandardLaTeXParser.RemoveNewlines(code);

            var elements = new List<LaTeXElement>();
            var elementTypeFound = false;

            foreach (var element in List)
            {
                if (code.StartsWith(element))
                {
                    elementTypeFound = true;
                    var individualElements = code.Split(new[] { element }, StringSplitOptions.RemoveEmptyEntries);
                    if (element == @"\documentclass")
                    {
                        var docClass = new DocumentClass();
                        //docClass.ParseCode(code);
                        return new List<LaTeXElement> { docClass };
                    }
                    if (element == @"\chapter")
                    {/*
                        var chapters = setElements(individualElements, element, delegate (string chapterCode, Chapter chapter) { chapter.ParseCode(chapterCode); return chapter; });
                        elements.AddRange(chapters);*/
                    }
                    if (element == @"\section")
                    {
                        var sections = setElements(individualElements, element, delegate (string sectionCode, Section section) { section.ParseCode(sectionCode); return section; });
                        elements.AddRange(sections);
                    }
                    break;
                }
            }

            if (!elementTypeFound)
            {
                var content = getContent(code);
                elements.AddRange(content);
            }

            return elements;
        }

        private static IEnumerable<T> setElements<T>(IEnumerable<string> elements, string indicator,
            Func<string, T, T> parseFunction) where T : new()
        {
            var list = new List<T>();
            foreach (var chapterCode in elements)
            {
                var chapter = new T();
                parseFunction(indicator + chapterCode, chapter);
                list.Add(chapter);
            }
            return list;
        }

        private static IEnumerable<LaTeXElement> getContent(string code)
        {
            var list = new List<LaTeXElement>();

            while (!String.IsNullOrWhiteSpace(code))
            {
                code = code.StartsWith(@"\begin{") ? createElement(code, list) : createTextElement(code, list);
            }

            return list;
        }

        private static string createElement(string code, List<LaTeXElement> list)
        {
            var elementType = Regex.Match(code, @"\\begin\{(.*?)\}").Groups[1].Value;
            var regex = new Regex(@"(?s)\\begin{" + elementType + @"}(.*?)\\end{" + elementType + "}");
            var element = regex.Match(code);

            switch (elementType)
            {
                case "enumerate":
                    {
                        code = createEnumerate(code, list, element);
                        break;
                    }
                default:
                    {
                        code = createCustomCommand(code, list, element);
                        break;
                    }
            }
            return code;
        }

        private static string createEnumerate(string code, List<LaTeXElement> list, Match element)
        {
            var enumerator = new Enumerate();
            enumerator.ParseCode(element.Value);
            list.Add(enumerator);
            return code.Replace(element.Value, "");
        }

        private static string createCustomCommand(string code, List<LaTeXElement> list, Match element)
        {
            var command = new CustomCommand();
            command.ParseCode(element.Value);
            list.Add(command);
            return code.Replace(element.Value, "");
        }

        private static string createTextElement(string code, List<LaTeXElement> list)
        {
            var codeSb = new StringBuilder(code);

            var content = getFirstPartOfTheText(codeSb);

            balanceMathSigns(content, codeSb);

            if (!String.IsNullOrWhiteSpace(content.ToString()))
            {
                var newText = setTheText(content, codeSb);
                list.Add(newText);
            }

            return codeSb.ToString();
        }

        private static StringBuilder getFirstPartOfTheText(StringBuilder codeSb)
        {
            /*
            var content = new StringBuilder(StandardLaTeXParser.GetNextTextPart(codeSb.ToString()));
            codeSb.Replace(content.ToString(), "");
            return content;*/
            return null;
        }

        private static void balanceMathSigns(StringBuilder content, StringBuilder codeSb)
        {
            /*
            while (StandardLaTeXParser.HasUnevenMathIndicator(content.ToString()))
            {
                getNextPart(codeSb, content);
            }*/
        }

        private static void getNextPart(StringBuilder codeSb, StringBuilder sb)
        {/*
            var command = StandardLaTeXParser.GetFirstCommand(codeSb.ToString());

            if (command == "")
                command = StandardLaTeXParser.GetNextTextPart(codeSb.ToString());

            sb.Append(command);

            if (command != "")
                codeSb.Replace(command, "");*/
        }

        private static TextBody setTheText(StringBuilder content, StringBuilder codeSb)
        {
            codeSb.Replace(content.ToString(), "");
            return new TextBody { TheText = content.ToString() };
        }
    }
}