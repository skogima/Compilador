namespace Compiler
{
    class SenaoNo : ArvoreNo
    {
        public ArvoreNo Condicional { get; private set; }
        public ArvoreNo Corpo { get; private set; }

        public SenaoNo(ArvoreNo condicional, ArvoreNo corpo)
        {
            Condicional = condicional;
            Corpo = corpo;
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}