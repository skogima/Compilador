using System;

namespace Compiler
{
    public class SintaticoException : Exception
    {
        public SintaticoException(string message) : base(message) { }
        public SintaticoException(string message, Exception inner) : base(message, inner) { }
    }
}
