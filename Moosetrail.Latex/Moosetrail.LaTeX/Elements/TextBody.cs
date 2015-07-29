namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Object represents a textblock in a latex element
    /// </summary>
    public class TextBody : LaTeXElement
    {
        private string _theText;

        /// <summary>
        /// Creates a new instance of the text object
        /// </summary>
        /// <param name="text">The text this object contains</param>
        public TextBody(string text = "")
        {
            TheText = text;
        }

        /// <summary>
        /// Get/Set the text 
        /// </summary>
        public string TheText
        {
            get { return _theText; }
            set
            {
               // _theText = StandardLaTeXParser.RemoveNewlines(value.Trim());
            }
        }
    }
}