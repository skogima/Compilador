namespace Compiler
{
    public class ExpressaoNo : ArvoreNo
    {
        public ArvoreNo Esquerda { get; private set; }
        public Token Operacao { get; private set; }
        public ArvoreNo Direita { get; private set; }

        public ExpressaoNo(ArvoreNo esq, Token op, ArvoreNo dir)
        {
            Esquerda = esq;
            Operacao = op;
            Direita = dir;
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetExpressao(Esquerda, Operacao, Direita);
        }
    }
}
