using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Sintatico
    {
        private List<string> parser;
        public Sintatico(List<string> _parser)
        {
            parser = _parser;
        }

        public bool Analisar()
        {
            int numberOfOpenedParenteses = 0;
            int numberOfCorrespondentesClosedParenteses = 0;
            foreach (var item in parser)
            {
                if (item.Equals("abreParenteses"))
                {
                    numberOfOpenedParenteses++;
                }
                else if (item.Equals("fechaParenteses"))
                    numberOfCorrespondentesClosedParenteses++;

                if (numberOfOpenedParenteses != numberOfCorrespondentesClosedParenteses)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
