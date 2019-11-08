using System.Collections.Generic;

namespace Compiler
{
    public class Sintatico
    {
        private List<Token> listaTokens;
        private int tokens = 0;
        private Token tokenAtual;

        public Sintatico(List<Token> tokenList)
        {
            listaTokens = tokenList;
            tokenAtual = listaTokens[tokens];
        }

        public void Analisar()
        {
            var result = AnalisarFuncao();
        }

        private ArvoreNo AnalisarFuncao()
        {
            ProximoToken(TipoToken.Tipo);
            if (tokenAtual.Valor != "main")
                throw new SintaticoException($"Esperado função main.");
            ProximoToken(TipoToken.Identificador);
            ProximoToken(TipoToken.AbreParenteses);
            ProximoToken(TipoToken.FechaParenteses);
            ProximoToken(TipoToken.AbreChaves);

            var no = AnalisarListaComandos();

            TokenEsperado(TipoToken.FechaChaves);

            return no;
        }

        private ArvoreNo AnalisarListaComandos()
        {
            List<ArvoreNo> comandos = new List<ArvoreNo>();

            ArvoreNo no = AnalisarComando();

            while (!(no is VazioNo))
            {
                comandos.Add(no);
                no = AnalisarComando();
            }

            return new ComandosNo(comandos);
        }

        private ArvoreNo AnalisarComando()
        {
            ArvoreNo no;

            if (tokenAtual.Tipo == TipoToken.Se)
            {
                no = new VazioNo();
            }
            else if (tokenAtual.Tipo == TipoToken.Senao)
            {
                no = new VazioNo();
            }
            else if (tokenAtual.Tipo == TipoToken.Tipo)
            {
                no = AnalisarDeclaracao();
                ProximoToken(TipoToken.PontoVirgula);
            }
            else if (tokenAtual.Tipo == TipoToken.Identificador)
            {
                no = AnalisarAtribuicao();
                ProximoToken(TipoToken.PontoVirgula);
            }
            else if (tokenAtual.Tipo == TipoToken.Enquanto)
            {
                no = AnalisarLoop();
            }
            else
            {
                no = new VazioNo();
            }

            return no;
        }

        private ArvoreNo AnalisarDeclaracao()
        {
            Token tipo = ProximoToken(TipoToken.Tipo);
            Token identif = ProximoToken(TipoToken.Identificador);

            ArvoreNo atrib;
            if (tokenAtual.Tipo == TipoToken.Atribuicao)
            {
                tokenAtual = listaTokens[--tokens];
                atrib = AnalisarAtribuicao();
            }
            else
            {
                atrib = new VazioNo();
            }

            return new DeclaracaoNo(tipo, identif, atrib);
        }

        private ArvoreNo AnalisarAtribuicao()
        {
            Token identificador = ProximoToken(TipoToken.Identificador);
            Token atrib = ProximoToken(TipoToken.Atribuicao);
            ArvoreNo expressao = AnalisarExpressao();

            return new AtribuicaoNo(identificador, atrib, expressao);
        }


        private ArvoreNo AnalisarLoop()
        {
            ProximoToken(TipoToken.Enquanto);
            ProximoToken(TipoToken.AbreParenteses);
            ArvoreNo no = AnalisarBooleana();
            ProximoToken(TipoToken.FechaParenteses);

            ProximoToken(TipoToken.AbreChaves);
            var comandos = AnalisarListaComandos();
            ProximoToken(TipoToken.FechaChaves);

            return new LoopNo(no, comandos);
        }

        private ArvoreNo AnalisarBooleana()
        {
            var esq = AnalisarExpressao();
            Token op = tokenAtual;

            if (tokenAtual.Tipo == TipoToken.Igual || tokenAtual.Tipo == TipoToken.Diferente
                || tokenAtual.Tipo == TipoToken.Maior || tokenAtual.Tipo == TipoToken.Menor
                || tokenAtual.Tipo == TipoToken.MaiorIgual || tokenAtual.Tipo == TipoToken.MenorIgual)
            {
                ProximoToken(op.Tipo);
            }
            else
            {
                throw new SintaticoException($"Operador lógico esperado.");
            }

            var dir = AnalisarExpressao();
            return new BooleanaNo(op, esq, dir);
        }

        private ArvoreNo AnalisarExpressao()
        {
            ArvoreNo esq = AnalisarTermo();

            if (tokenAtual.Tipo == TipoToken.Mais || tokenAtual.Tipo == TipoToken.Menos)
            {
                Token operacao = tokenAtual;
                ProximoToken(operacao.Tipo);
                ArvoreNo dir = AnalisarTermo();
                esq = new ExpressaoNo(esq, operacao, dir);
            }

            return esq;
        }

        private ArvoreNo AnalisarTermo()
        {
            ArvoreNo fator = AnalisarFator();

            while (tokenAtual.Tipo == TipoToken.Vezes || tokenAtual.Tipo == TipoToken.Dividir)
            {
                Token operacao = tokenAtual;
                ProximoToken(operacao.Tipo);
                ArvoreNo dir = AnalisarFator();
                fator = new ExpressaoNo(fator, operacao, dir);
            }

            return fator;
        }

        private ArvoreNo AnalisarFator()
        {
            if (tokenAtual.Tipo == TipoToken.Identificador)
            {
                var id = ProximoToken(TipoToken.Identificador);
                return new FatorNo(id);
            }
            else if (tokenAtual.Tipo == TipoToken.Numero)
            {
                var id = ProximoToken(TipoToken.Numero);
                return new FatorNo(id);
            }
            else if (tokenAtual.Tipo == TipoToken.NumeroDecimal)
            {
                var id = ProximoToken(TipoToken.NumeroDecimal);
                return new FatorNo(id);
            }
            else if (tokenAtual.Tipo == TipoToken.AbreParenteses)
            {
                ProximoToken(TipoToken.AbreParenteses);
                ArvoreNo no = AnalisarExpressao();
                ProximoToken(TipoToken.FechaParenteses);
                return no;
            }
            else
            {
                var id = ProximoToken(TipoToken.Literal);
                return new FatorNo(id);
            }
        }

        private Token ProximoToken(TipoToken tipoToken)
        {
            if (tokenAtual.Tipo == tipoToken)
            {
                tokens++;
                Token token = new Token { Tipo = tokenAtual.Tipo, Valor = tokenAtual.Valor };
                tokenAtual = listaTokens[tokens];
                return token;
            }

            throw new SintaticoException($"Sintáxe inválida. {tipoToken.ToString()} esperado, " +
                $"mas {tokenAtual.Tipo.ToString()} foi recebido.");
        }

        private Token TokenEsperado(TipoToken tipoToken)
        {
            if (tokenAtual.Tipo == tipoToken)
                return tokenAtual;

            throw new SintaticoException($"Sintáxe inválida. {tipoToken.ToString()} esperado, " +
                $"mas {tokenAtual.Tipo.ToString()} foi recebido.");
        }
    }
}