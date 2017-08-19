namespace Moosetrail.LaTeX.Exceptions
{
    public class InvalidLatexCodeException : LaTeXException
    {
        public InvalidLatexCodeException() { }

        public InvalidLatexCodeException(string msg) 
            : base(msg)
        { }
    }
}