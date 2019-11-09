namespace Compiler
{
    public class CondicionalNo : ArvoreNo
    {
        public ArvoreNo Booleana { get; private set; }
        public ArvoreNo Corpo { get; private set; }
        public ArvoreNo Senao { get; private set; } 

        public CondicionalNo(ArvoreNo booleana, ArvoreNo corpo, ArvoreNo senao)
        {
            Booleana = booleana;
            Corpo = corpo;
            Senao = senao;
        }

        public override object GetValor(IValor valor)
        {
            return valor.GetCondicional(Booleana, Corpo, Senao);
        }
    }
}
