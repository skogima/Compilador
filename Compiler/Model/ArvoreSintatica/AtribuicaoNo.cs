namespace Compiler
{
    public class AtribuicaoNo : ArvoreNo
    {
        public Token Identificador { get; private set; }
        public ArvoreNo Expressao { get; private set; }

        public AtribuicaoNo(Token identificador, ArvoreNo expr)
        {
            Identificador = identificador;
            Expressao = expr;
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetAtribuicao(Identificador, Expressao);
        }
    }
}
