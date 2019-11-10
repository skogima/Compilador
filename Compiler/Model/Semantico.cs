using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Compiler
{
    public class Semantico : IValor
    {
        public Dictionary<string, object> Variaveis { get; private set; }
        public Dictionary<string, string> VariaveisTipo { get; private set; }

        public Semantico()
        {
            Variaveis = new Dictionary<string, object>();
            VariaveisTipo = new Dictionary<string, string>();
        }

        public object GetComandos(List<ArvoreNo> comandos)
        {
            foreach (var item in comandos)
            {
                item.GetValor(this);
            }

            List<Variaveis> variaveis = new List<Variaveis>();

            foreach (var key in Variaveis.Keys)
            {
                variaveis.Add(new Variaveis { 
                    Identificadores = key, 
                    Tipos = VariaveisTipo[key], 
                    Valores = Variaveis[key] ?? "variável declarada, mas valor não atribuído."
                });
            }

            return variaveis;
        }

        public object GetAtribuicao(Token identificador, ArvoreNo expressao)
        {
            object result;

            if (Variaveis.ContainsKey(identificador.Valor))
            {
                result = expressao.GetValor(this);
                Variaveis[identificador.Valor] = result;

                if (result.GetType() == typeof(int))
                {
                    if (VariaveisTipo[identificador.Valor] != "int")
                        throw new Exception("Tipo errado foi atribuído a variável");
                    return Convert.ToInt32(result);
                }
                else if (result.GetType() == typeof(float))
                {
                    if (VariaveisTipo[identificador.Valor] != "float")
                        throw new Exception("Tipo errado foi atribuído a variável");
                    return float.Parse(result.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    if (VariaveisTipo[identificador.Valor] != "char")
                        throw new Exception("Tipo errado foi atribuído a variável");
                    return Convert.ToChar(result);
                }
            }
            else
                throw new Exception($"Variável {identificador.Valor} não existe.");
        }

        public object GetBooleana(ArvoreNo esquerda, Token operador, ArvoreNo direita)
        {
            switch (operador.Tipo)
            {
                case TipoToken.Igual:
                    return (esquerda.GetValor(this) == direita.GetValor(this));
                case TipoToken.Maior:
                    return true;
                default:
                    return false;
            }
        }

        public object GetCondicional(ArvoreNo booleana, ArvoreNo corpo, ArvoreNo senao)
        {
            throw new System.NotImplementedException();
        }

        public object GetDeclaracao(Token tipo, Token identificador, ArvoreNo atribuicao)
        {
            Variaveis.Add(identificador.Valor, null);
            VariaveisTipo.Add(identificador.Valor, tipo.Valor);

            if (!(atribuicao is VazioNo))
            {
                return atribuicao.GetValor(this);
            }

            return identificador;
        }

        public object GetExpressao(ArvoreNo esquerda, Token operacao, ArvoreNo direita)
        {
            var esq = esquerda.GetValor(this);
            var dir = direita.GetValor(this);

            var tipoEsq = esq.GetType();
            var tipoDir = esq.GetType();

            if (tipoDir.Equals(tipoEsq))
            {
                switch (operacao.Tipo)
                {
                    case TipoToken.Mais:
                        if (tipoDir.Equals(typeof(int)))
                            return Convert.ToInt32(esq) + Convert.ToInt32(dir);
                        else
                            return Convert.ToSingle(esq) + Convert.ToSingle(dir);
                    case TipoToken.Menos:
                        if (tipoDir.Equals(typeof(int)))
                            return Convert.ToInt32(esq) - Convert.ToInt32(dir);
                        else
                            return Convert.ToSingle(esq) - Convert.ToSingle(dir);
                    case TipoToken.Vezes:
                        if (tipoDir.Equals(typeof(int)))
                            return Convert.ToInt32(esq) * Convert.ToInt32(dir);
                        else
                            return Convert.ToSingle(esq) * Convert.ToSingle(dir);
                    case TipoToken.Dividir:
                        if (tipoDir.Equals(typeof(int)))
                            return Convert.ToInt32(esq) / Convert.ToInt32(dir);
                        else
                            return Convert.ToSingle(esq) / Convert.ToSingle(dir);
                    default:
                        throw new System.Exception("Fudeu");
                }
            }
            else
                throw new Exception($"Não é possível realizar operação entre {esq.GetType()} e {dir.GetType().Name}");
        }

        public object GetFator(Token fator)
        {
            if (fator.Tipo == TipoToken.Identificador)
            {
                if (Variaveis.ContainsKey(fator.Valor))
                {
                    if (VariaveisTipo[fator.Valor] == "int")
                        return Convert.ToInt32(Variaveis[fator.Valor]);
                    else if (VariaveisTipo[fator.Valor] == "float")
                        return Convert.ToSingle(Variaveis[fator.Valor]);
                    else
                        return Convert.ToChar(Variaveis[fator.Valor]);
                }
                else
                    throw new Exception($"Variável {fator.Valor} não existe.");
            }
            else if (fator.Tipo == TipoToken.Literal)
                return Convert.ToChar(fator.Valor);
            else if (fator.Tipo == TipoToken.NumeroDecimal)
            {
                float result = float.Parse(fator.Valor, CultureInfo.InvariantCulture.NumberFormat);
                return result;
            }
            else
                return Convert.ToInt32(fator.Valor);
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
