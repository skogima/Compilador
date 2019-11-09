
namespace Compiler
{
    public class DeclaracaoNo : ArvoreNo
    {
        private Token mTipo;
        private Token mIdentificador;
        private ArvoreNo mAtribuicao;

        public DeclaracaoNo(Token tipo, Token identificador, ArvoreNo atribuicao)
        {
            mTipo = tipo;
            mIdentificador = identificador;
            mAtribuicao = atribuicao;
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetDeclaracao(mTipo, mIdentificador, mAtribuicao);
        }
    }
}
