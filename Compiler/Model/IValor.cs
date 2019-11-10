using System.Collections.Generic;

namespace Compiler
{
    public interface IValor
    {
        object GetDeclaracao(Token tipo, Token identificador, ArvoreNo atribuicao);
        object GetAtribuicao(Token identificador, ArvoreNo expressao);
        object GetExpressao(ArvoreNo esquerda, Token operacao, ArvoreNo direita);
        object GetLoop(ArvoreNo booleana, ArvoreNo corpo);
        object GetFator(Token fator);
        object GetCondicional(ArvoreNo booleana, ArvoreNo corpo, ArvoreNo senao);
        object GetComandos(List<ArvoreNo> comandos);
        object GetBooleana(ArvoreNo esquerda, Token operador, ArvoreNo direita);
    }
}
