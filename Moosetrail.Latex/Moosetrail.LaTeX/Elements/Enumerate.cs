using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            _itemList.Add(new TextBody(item));
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
        public static string Pattern = @"(?s)\\begin{enumerate}(.*?)\\end{enumerate}";

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