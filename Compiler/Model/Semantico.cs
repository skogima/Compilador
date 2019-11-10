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
                        throw new SemanticoException("Tipo errado foi atribuído a variável");
                    return Convert.ToInt32(result);
                }
                else if (result.GetType() == typeof(float))
                {
                    if (VariaveisTipo[identificador.Valor] != "float")
                        throw new SemanticoException("Tipo errado foi atribuído a variável");
                    return float.Parse(result.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    if (VariaveisTipo[identificador.Valor] != "char")
                        throw new SemanticoException("Tipo errado foi atribuído a variável");
                    return Convert.ToChar(result);
                }
            }
            else
                throw new SemanticoException($"Variável {identificador.Valor} não existe.");
        }

        public object GetBooleana(ArvoreNo esquerda, Token operador, ArvoreNo direita)
        {
            var esq = esquerda.GetValor(this);
            var dir = direita.GetValor(this);
            IComparer<object> comparer = Comparer<object>.Default;

            switch (operador.Tipo)
            {
                case TipoToken.Igual:
                    return comparer.Compare(esq, dir) == 0 ? true : false;
                case TipoToken.Maior:
                    return comparer.Compare(esq, dir) > 0 ? true : false;
                case TipoToken.Menor:
                    return comparer.Compare(esq, dir) < 0 ? true : false;
                case TipoToken.MaiorIgual:
                    return comparer.Compare(esq, dir) >= 0 ? true : false;
                case TipoToken.MenorIgual:
                    return comparer.Compare(esq, dir) <= 0 ? true : false;
                case TipoToken.Diferente:
                    return comparer.Compare(esq, dir) != 0 ? true : false;
                default:
                    throw new SemanticoException("Erro ao analisar expressão booleana");
            }
        }

        public object GetCondicional(ArvoreNo booleana, ArvoreNo corpo, ArvoreNo senao)
        {
            bool condicao = (bool)booleana.GetValor(this);
            object retorno = null;
            if (condicao)
            {
                retorno = corpo.GetValor(this);
            }
            else
            {
                if (!(senao is VazioNo))
                {
                    retorno = senao.GetValor(this);
                }
            }

            RemoverDeclaracaoLocal(corpo as ComandosNo);

            return retorno;
        }

        public object GetLoop(ArvoreNo booleana, ArvoreNo corpo)
        {
            bool condicao = (bool)booleana.GetValor(this);
            object resultado = new object();

            while (condicao)
            {
                resultado = corpo.GetValor(this);
                condicao = (bool)booleana.GetValor(this);
                RemoverDeclaracaoLocal(corpo as ComandosNo);
            }

            return resultado;
        }

        private void RemoverDeclaracaoLocal(ComandosNo no)
        {
            foreach (var item in no.Comandos)
            {
                if (item is DeclaracaoNo)
                {
                    string id = (item as DeclaracaoNo).Identificador.Valor;
                    if (Variaveis.ContainsKey(id))
                    {
                        Variaveis.Remove(id);
                        VariaveisTipo.Remove(id);
                    }

                }
            }
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
            var tipoDir = dir.GetType();

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
                        throw new SemanticoException($"Não foi possível realizar operações entre {esq} e {dir}");
                }
            }
            else
                throw new SemanticoException($"Não é possível realizar operação entre {esq.GetType()} e {dir.GetType().Name}");
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
                    throw new SemanticoException($"Variável {fator.Valor} não existe.");
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
    }
}
