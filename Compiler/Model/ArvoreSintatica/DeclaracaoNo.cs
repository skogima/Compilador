
namespace Compiler
{
    public class DeclaracaoNo : ArvoreNo
    {
        public Token Tipo { get; set; }
        public Token Identificador { get; set; }
        private ArvoreNo Atribuicao { get; set; }

        public DeclaracaoNo(Token tipo, Token identificador, ArvoreNo atribuicao)
        {
            Tipo = tipo;
            Identificador = identificador;
            Atribuicao = atribuicao;
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetDeclaracao(Tipo, Identificador, Atribuicao);
        }
    }
}
