namespace Compiler
{
    public class AtribuicaoNo : ArvoreNo
    {
        public Token Identificador { get; private set; }
        public Token Atribuicao { get; private set; }
        public ArvoreNo Expressao { get; private set; }

        public AtribuicaoNo(Token identificador, Token atribuicao, ArvoreNo expr)
        {
            Identificador = identificador;
            Atribuicao = atribuicao;
            Expressao = expr;
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}
