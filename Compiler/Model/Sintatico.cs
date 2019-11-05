using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Sintatico
    {
        private ArvoreSintatica arvore;
        private List<string> parser;

        public Sintatico(List<string> _parser)
        {
            parser = _parser;
            arvore = new ArvoreSintatica();
            arvore.Valor = "programa";
        }

        public void Analisar()
        {
            var enumerator = parser.GetEnumerator();
            enumerator.MoveNext();
            bool result = AnalisarPrograma(enumerator);
            if (!result) throw new SintaticoException("Função main não declarada");
        }

        private bool AnalisarPrograma(IEnumerator<string> enumerator)
        {
            string item = enumerator.Current;

            if (item.Contains("tipo"))
            {
                var itens = item.Split(' ');

                if (itens[1].Equals(Simbolos.Int) || item[1].Equals(Simbolos.Void) ||
                    item[1].Equals(Simbolos.Float) || item[1].Equals(Simbolos.Char))
                {
                    enumerator.MoveNext();
                    return AnalisarPrograma(enumerator);
                }
                else
                {
                    throw new SintaticoException($"Tipo não reconhecido: {itens[1]}");
                }
            }
            else if (item.Equals("identificador main"))
            {
                enumerator.MoveNext();
                if (enumerator.Current.Equals(Simbolos.AbreParenteses))
                {
                    enumerator.MoveNext();
                    if (enumerator.Current.Equals(Simbolos.FechaParenteses))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            else return false;
        }
        private bool AnalisarExpressao(IEnumerator<string> enumerator)
        {
            // TODO: Implementar
            return false;
        }
        private bool AnalisarFuncao(IEnumerator<string> enumerator)
        {
            // TODO: implementar
            return false;
        }
    }
}