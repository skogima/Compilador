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
            
            foreach (var item in comandos)
            {
                if (item is VazioNo)
                    continue;
                Comandos.Add(item);
            }
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetComandos(Comandos);
        }
    }
}
