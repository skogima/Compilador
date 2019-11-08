using System;

namespace Compiler
{
    public class LoopNo : ArvoreNo
    {
        public ArvoreNo Booleana { get; private set; }
        public ArvoreNo Corpo { get; private set; }

        public LoopNo(ArvoreNo booleana, ArvoreNo corpo)
        {
            Booleana = booleana;
            Corpo = corpo;
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}
