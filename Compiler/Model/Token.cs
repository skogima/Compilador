using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Model
{
    public class Token
    {
        public string Valor { get; set; }
        public TipoToken Tipo { get; set; }

        public Token(TipoToken tipo, string valor)
        {
            Valor = valor;
            Tipo = tipo;
        }

        public Token()
        {
            Tipo = TipoToken.Vazio;
            Valor = string.Empty;
        }
    }
}
