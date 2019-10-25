using System;
using System.Collections.Generic;

namespace Compiler
{
    public class ArvoreSintatica
    {
        public List<ArvoreSintatica> Filhos { get; set; }
        public string Valor { get; set; }
        public ArvoreSintatica Pai { get; set; }

        public ArvoreSintatica()
        {
            Filhos = new List<ArvoreSintatica>();
        }

        public void AdicionarFilho(ArvoreSintatica filho)
        {
            filho.Pai = this;
            Filhos.Add(filho);
        }
    }
}
