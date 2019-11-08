namespace Compiler
{
    public class FatorNo : ArvoreNo
    {
        public Token Fator { get; private set; }

        public FatorNo(Token fator)
        {
            Fator = fator;
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}
