using System;
namespace Compiler
{
    public class BooleanaNo : ArvoreNo
    {
        public Token Operador { get; private set; }
        public ArvoreNo Esquerda { get; private set; }
        public ArvoreNo Direita { get; private set; }

        public BooleanaNo(Token operador, ArvoreNo esq, ArvoreNo dir)
        {
            Operador = operador;
            Esquerda = esq;
            Direita = dir;
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}
