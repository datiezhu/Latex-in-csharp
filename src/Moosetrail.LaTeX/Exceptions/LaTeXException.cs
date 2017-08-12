using System;

namespace Moosetrail.LaTeX.Exceptions
{
    public class LaTeXException : ArgumentException
    {
        public LaTeXException() { }

        public LaTeXException(string msg): base(msg) { }
    }
}