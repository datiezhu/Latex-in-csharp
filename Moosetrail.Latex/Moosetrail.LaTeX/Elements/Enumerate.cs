using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class represent an enumerate list for LaTeX
    /// </summary>
    public class Enumerate : LaTeXElement
    {
        private const string BeginEnumerate = @"\begin{enumerate}";
        private const string EndEnumerate = @"\end{enumerate}";

        private StringBuilder _code;
        private List<LaTeXElement> _itemList;

        /// <summary>
        /// Creates a new instance of the Enumerate class
        /// </summary>
        public Enumerate()
        {
            ItemList = new List<LaTeXElement>();
        }

        /// <summary>
        /// Creates a new instance of the Enumerate class and parses code into it
        /// </summary>
        /// <param name="code">The code to parse into the object</param>
        public Enumerate(string code)
            :this()
        {
            var listBody = Regex.Match(code, Pattern);
            var itemParts = listBody.Groups[1].Value.Split(new[] {@"\item"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var itemPart in itemParts.Where(itemPart => !String.IsNullOrWhiteSpace(itemPart)))
            {
                _itemList.Add(new TextBody(itemPart.Trim()));
            }
        }

        /// <summary>
        /// Get/Set the list of itmes in the list
        /// </summary>
        public IEnumerable<LaTeXElement> ItemList
        {
            get { return _itemList; }
            set { _itemList = value.ToList(); }
        }

        /// <summary>
        /// Adds a new text item to the enumerate
        /// </summary>
        /// <param name="item">The text to add as the item</param>
        public void AddItem(string item)
        {
            if(!TextFormatStrings.List.Contains(item))
                _itemList.Add(new TextBody(TextFormatStrings.Remove(item)));
        }

        /// <summary>
        /// Converts the object into latex code
        /// </summary>
        /// <returns>A string with latex code that represents this object in latex format</returns>
        public string ToCode()
        {
            _code = new StringBuilder(BeginEnumerate);
            foreach (var element in ItemList)
            {
                _code.AppendFormat(@"\item {0} ", ((TextBody)element).TheText);
            }
            _code.Append(EndEnumerate);

            return _code.ToString();
        }

        /// <summary>
        /// Get the pattern that identifies lists
        /// </summary>
        public static string Pattern = @"\\begin{enumerate}(.*?)\\end{enumerate}";

        public IEnumerable<string> CodeIndicators
        {
            get { throw new NotImplementedException(); }
        }

        public void SetChildElement(params LaTeXElement[] elements)
        {
            throw new NotImplementedException();
        }

        internal void ParseCode(string code)
        {
            _code = new StringBuilder(code);
            _code.Replace(BeginEnumerate, "");
            _code.Replace(EndEnumerate, "");

            if (!_code.ToString().StartsWith(@"\item"))
            {
                //var update = StandardLaTeXParser.RemoveNewlines(_code.ToString());
                _code.Clear();
                //_code.Append(update);
            }

            var items = _code.ToString().Split(new[] { @"\item" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                AddItem(item);
            }
        }
    }
}