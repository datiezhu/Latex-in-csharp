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
            set { _theText = value.Trim(); }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return equals(obj as TextBody);
        }

        protected bool equals(TextBody other)
        {
            return other != null && TheText == other.TheText;
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return (_theText != null ? _theText.GetHashCode() : 0);
        }

    }
}