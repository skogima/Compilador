namespace Compiler
{
    public class CondicionalNo : ArvoreNo
    {
        public ArvoreNo Booleana { get; private set; }
        public ArvoreNo Corpo { get; private set; }

        public CondicionalNo(ArvoreNo booleana, ArvoreNo corpo)
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
