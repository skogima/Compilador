using System;

namespace Compiler
{
    public class LexicoException : Exception
    {
        public int Linha { get; set; }
        public char Caracter { get; set; }

        public LexicoException(string message) : base(message) { }
        public LexicoException(string message, Exception inner) : base(message, inner) { }

        public LexicoException(string message, int linha, char caracter) : base(message)
        {
            Linha = linha;
            Caracter = caracter;
        }
    }
}
