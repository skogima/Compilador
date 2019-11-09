using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    public class Semantico : IValor
    {
        public Dictionary<string, object> Variaveis { get; private set; }

        public Semantico()
        {
            Variaveis = new Dictionary<string, object>();
        }

        public object GetComandos(List<ArvoreNo> comandos)
        {
            foreach (var item in comandos)
            {
                item.GetValor(this);
            }

            StringBuilder builder = new StringBuilder();

            foreach (var key in Variaveis.Keys)
            {
                builder.AppendLine($"{key} = {Variaveis[key]}");
            }

            return builder.ToString();
        }

        public object GetAtribuicao(Token identificador, ArvoreNo expressao)
        {
            var result = expressao.GetValor(this);
            if (Variaveis.ContainsKey(identificador.Valor))
                Variaveis[identificador.Valor] = result;
            else
                throw new Exception($"Variável {identificador.Valor} não existe.");
            return result;
        }

        public object GetBooleana(ArvoreNo esquerda, Token operador, ArvoreNo direita)
        {
            throw new System.NotImplementedException();
        }

        public object GetCondicional(ArvoreNo booleana, ArvoreNo corpo, ArvoreNo senao)
        {
            throw new System.NotImplementedException();
        }

        public object GetDeclaracao(Token tipo, Token identificador, ArvoreNo atribuicao)
        {
            Variaveis.Add(identificador.Valor, null);
            var atrib = atribuicao.GetValor(this);
            Variaveis[identificador.Valor] = atrib;
            return atrib;
        }

        public object GetExpressao(ArvoreNo esquerda, Token operacao, ArvoreNo direita)
        {
            var esq = esquerda.GetValor(this);
            var dir = direita.GetValor(this);

            switch (operacao.Tipo)
            {
                case TipoToken.Mais:
                    return Convert.ToInt32(esq) + Convert.ToInt32(dir);
                case TipoToken.Menos:
                    return Convert.ToInt32(esq) - Convert.ToInt32(dir);
                case TipoToken.Vezes:
                    return Convert.ToInt32(esq) * Convert.ToInt32(dir);
                case TipoToken.Dividir:
                    return Convert.ToInt32(esq) / Convert.ToInt32(dir);
                default:
                    throw new System.Exception("Fudeu");
            }
        }

        public object GetFator(Token fator)
        {
            if (fator.Tipo == TipoToken.Identificador)
            {
                if (Variaveis.ContainsKey(fator.Valor))
                    return Variaveis[fator.Valor];
                else
                    throw new Exception($"Variável {fator.Valor} não existe.");
            }
            return fator.Valor;
        }

        public object GetLoop(ArvoreNo booleana, ArvoreNo corpo)
        {
            throw new System.NotImplementedException();
        }

        public object GetSenao(ArvoreNo booleana, ArvoreNo corpo)
        {
            throw new System.NotImplementedException();
        }
    }
}
