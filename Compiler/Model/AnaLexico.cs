using System.Collections.Generic;

namespace Compiler
{
    public class AnaLexico
    {
        private string codigo;
        private int posAtual;
        private char caractereAtual;
        private List<Token> listaTokens;

        public AnaLexico(string document)
        {
            codigo = document;
            listaTokens = new List<Token>();

            posAtual = -1;
            MoverCaracter();
        }

        public List<Token> Analisar()
        {
            int linha = 1;
            while (!caractereAtual.Equals(char.MinValue))
            {
                switch (caractereAtual)
                {
                    #region Ignorados
                    case ' ':
                        while (caractereAtual != char.MinValue && caractereAtual.Equals(' '))
                            MoverCaracter();
                        if (caractereAtual.Equals(char.MinValue))
                            listaTokens.Add(new Token());
                        else if (caractereAtual != ' ')
                            RecuarCaractere();
                        break;
                    case '\r':
                        break;
                    case '\n':
                        linha++;
                        break;
                    case '\t':
                        break;
                    #endregion
                    #region Símbolos
                    case '+':
                        listaTokens.Add(AddToken(TipoToken.Mais));
                        break;
                    case '-':
                        listaTokens.Add(AddToken(TipoToken.Menos));
                        break;
                    case '*':
                        listaTokens.Add(AddToken(TipoToken.Vezes));
                        break;
                    case '/':
                        listaTokens.Add(AddToken(TipoToken.Dividir));
                        break;
                    case '(':
                        listaTokens.Add(AddToken(TipoToken.AbreParenteses));
                        break;
                    case ')':
                        listaTokens.Add(AddToken(TipoToken.FechaParenteses));
                        break;
                    case '{':
                        listaTokens.Add(AddToken(TipoToken.AbreChaves));
                        break;
                    case '}':
                        listaTokens.Add(AddToken(TipoToken.FechaChaves));
                        break;
                    case ';':
                        listaTokens.Add(AddToken(TipoToken.PontoVirgula));
                        break;
                    case '.':
                        listaTokens.Add(AddToken(TipoToken.Ponto));
                        break;
                    case '<':
                        MoverCaracter();
                        if (caractereAtual.Equals('='))
                            listaTokens.Add(new Token { Tipo = TipoToken.MenorIgual, Valor = "<=" });
                        else
                        {
                            RecuarCaractere();
                            listaTokens.Add(AddToken(TipoToken.Menor));
                        }
                        break;
                    case '>':
                        MoverCaracter();
                        if (caractereAtual.Equals('='))
                            listaTokens.Add(new Token { Tipo = TipoToken.MaiorIgual, Valor = ">=" });
                        else
                        {
                            RecuarCaractere();
                            listaTokens.Add(AddToken(TipoToken.Maior));
                        }
                        break;
                    case '=':
                        MoverCaracter();
                        if (caractereAtual.Equals('='))
                            listaTokens.Add(new Token { Tipo = TipoToken.Igual, Valor = "==" });
                        else
                        {
                            RecuarCaractere();
                            listaTokens.Add(AddToken(TipoToken.Atribuicao));
                        }
                        break;
                    case '!':
                        MoverCaracter();
                        if (caractereAtual.Equals('='))
                            listaTokens.Add(new Token { Tipo = TipoToken.Diferente, Valor = "!=" });
                        else
                        {
                            RecuarCaractere();
                            throw new LexicoException($"Caractere inesperado", linha, caractereAtual);
                        }
                        break;
                    case '\'':
                        MoverCaracter();
                        if (char.IsLetterOrDigit(caractereAtual))
                        {
                            char c = caractereAtual;
                            MoverCaracter();
                            if (caractereAtual.Equals('\''))
                                listaTokens.Add(new Token { Tipo = TipoToken.Literal, Valor = c.ToString()});
                            else
                            {
                                throw new LexicoException($"Caractere inesperado", linha, caractereAtual);
                            }
                        }
                        break;
                    #endregion
                    default:
                        #region Palavras
                        if (char.IsLetter(caractereAtual))
                        {
                            string palavra = string.Empty;
                            palavra += caractereAtual;
                            MoverCaracter();

                            while (char.IsLetterOrDigit(caractereAtual))
                            {
                                palavra += caractereAtual;
                                MoverCaracter();
                            }

                            if (palavra.Equals("if"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Se, Valor = palavra });
                            else if (palavra.Equals("else"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Senao, Valor = palavra });
                            else if (palavra.Equals("while"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Enquanto, Valor = palavra });
                            else if (palavra.Equals("for"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Para, Valor = palavra });
                            else if (palavra.Equals("do"))
                                listaTokens.Add(new Token { Tipo = TipoToken.DoEnquanto, Valor = palavra });
                            else if (palavra.Equals("break"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Parar, Valor = palavra });
                            else if (palavra.Equals("continue"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Continuar, Valor = palavra });
                            else if (palavra.Equals("switch"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Comutar, Valor = palavra });
                            else if (palavra.Equals("case"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Caso, Valor = palavra });
                            else if (palavra.Equals("int") || palavra.Equals("float") ||
                                palavra.Equals("char") || palavra.Equals("void"))
                                listaTokens.Add(new Token { Tipo = TipoToken.Tipo, Valor = palavra });
                            else
                                listaTokens.Add(new Token { Tipo = TipoToken.Identificador, Valor = palavra });

                            RecuarCaractere();
                        }
                        #endregion
                        #region Identificação de Números
                        else if (char.IsDigit(caractereAtual))
                        {
                            string num = string.Empty;
                            while (char.IsDigit(caractereAtual))
                            {
                                num += caractereAtual;
                                MoverCaracter();
                            }

                            if (caractereAtual.Equals('.'))
                            {
                                num += caractereAtual;
                                MoverCaracter();
                                if (char.IsDigit(caractereAtual))
                                {
                                    while (char.IsDigit(caractereAtual))
                                    {
                                        num += caractereAtual;
                                        MoverCaracter();
                                    }
                                    listaTokens.Add(new Token { Tipo = TipoToken.NumeroDecimal, Valor = num });
                                    RecuarCaractere();
                                }
                                else
                                {
                                    throw new LexicoException($"Caractere inesperado", linha, caractereAtual); 
                                }
                            }
                            else
                            {
                                listaTokens.Add(new Token { Tipo = TipoToken.Numero, Valor = num });
                                RecuarCaractere();
                            }
                        }
                        #endregion
                        else
                            throw new LexicoException($"Caractere inesperado", linha, caractereAtual);
                        break;
                }
                MoverCaracter();
            }
            return listaTokens;
        }

        private void MoverCaracter()
        {
            posAtual++;

            if (posAtual < codigo.Length)
            {
                caractereAtual = codigo[posAtual];
            }
            else
            {
                caractereAtual = char.MinValue;
            }
        }

        private void RecuarCaractere()
        {
            posAtual--;

            if (posAtual >= 0)
            {
                caractereAtual = codigo[posAtual];
            }
            else
            {
                caractereAtual = char.MinValue;
            }
        }

        private Token AddToken(TipoToken tipo) => new Token { Tipo = tipo, Valor = caractereAtual.ToString() };
    }
}