using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class ComandosNo : ArvoreNo
    {
        public List<ArvoreNo> Comandos { get; private set; }

        public ComandosNo(List<ArvoreNo> comandos)
        {
            Comandos = new List<ArvoreNo>();

            // TODO: alterar
            foreach (var item in comandos)
            {
                if (item is VazioNo)
                    continue;
                Comandos.Add(item);
            }
        }

        public override object Aceitar(object opcoes)
        {
            return opcoes;
        }
    }
}
