using System;
using System.Linq;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;

namespace Compiler
{
    public class AnaLexico
    {
        private int token;

        public AnaLexico() { }

        public string Analisar(string linha)
        {
            token = 0;
            var lista = AnalisarLinha(linha);
            string retorno = string.Empty;

            lista.ForEach(x => retorno += x + " ");

            if (string.IsNullOrEmpty(retorno))
                return null;
            return retorno;
        }

        private List<string> AnalisarLinha(string linha)
        {
            string palavraAtual = string.Empty;
            List<string> parser = new List<string>();
            int posLinha = 1;

            for (int index = 0; index < linha.Length; index++)
            {
                char c = linha[index];

                switch (token)
                {
                    case 0:
                        switch (c)
                        {
                            //TODO: Operadores (&&, ||, ...)
                            case ';':
                                parser.Add("pontoVirgula");
                                break;
                            case '+':
                                parser.Add("mais");
                                break;
                            case '-':
                                parser.Add("menos");
                                break;
                            case '*':
                                parser.Add("vezes");
                                break;
                            case '/':
                                parser.Add("dividir");
                                break;
                            case ',':
                                parser.Add("virgula");
                                break;
                            case '.':
                                parser.Add("ponto");
                                break;
                            case '(':
                                parser.Add("abreParenteses");
                                break;
                            case ')':
                                parser.Add("fechaParenteses");
                                break;
                            case '{':
                                parser.Add("abreChaves");
                                break;
                            case '}':
                                parser.Add("fechaChaves");
                                break;
                            case ':':
                                parser.Add("doisPontos");
                                break;
                            case '<':
                                token = 1;
                                break;
                            case '>':
                                token = 2;
                                break;
                            case '=':
                                token = 3;
                                break;
                            case '!':
                                token = 4;
                                break;
                            case ' ':
                                break;
                            case '\r':
                                break;
                            case '\n':
                                posLinha++;
                                break;
                            case '\t':
                                break;
                            default:
                                if (char.IsLetter(c))
                                {
                                    token = 5;
                                    index--;
                                }
                                else if (char.IsDigit(c))
                                {
                                    token = 6;
                                    index--;
                                }
                                else
                                {
                                    throw new Exception($"Caracter inesperado: linha {posLinha}");
                                }
                                break;
                        }
                        break;
                    case 1: // menor
                        if (c.Equals('='))
                            parser.Add("menorIgual");
                        else
                        {
                            parser.Add("menor");
                            index--;
                        }
                        token = 0;
                        break;
                    case 2:
                        if (c.Equals('='))
                            parser.Add("maiorIgual");
                        else
                        {
                            parser.Add("maior");
                            index--;
                        }
                        token = 0;
                        break;
                    case 3:
                        if (c.Equals('='))
                            parser.Add("igual");
                        else
                        {
                            parser.Add("atribuicao");
                            index--;
                        }
                        token = 0;
                        break;
                    case 4:
                        if (c.Equals('='))
                            parser.Add("diferente");
                        else
                        {
                            parser.Add("negacao");
                            index--;
                        }
                        token = 0;
                        break;
                    case 5: // Palavra
                        if (char.IsLetterOrDigit(c))
                            palavraAtual += c;
                        var proximo = linha.ElementAtOrDefault(index + 1);
                        if (!char.IsLetterOrDigit(proximo))
                        {
                            parser.Add(VerificarPalavraReservada(palavraAtual));

                            token = 0;
                            palavraAtual = string.Empty;
                        }
                        break;
                    case 6: // número
                        proximo = linha.ElementAtOrDefault(index + 1);
                        if (char.IsDigit(c))
                        {
                            palavraAtual += c;
                            if (proximo.Equals('.') || char.IsDigit(proximo))
                                continue;
                        }
                        else if (c.Equals('.'))
                        {
                            palavraAtual += c;
                            token = 7;
                            continue;
                        }
                        if (!char.IsDigit(proximo))
                        {
                            parser.Add($"numero {palavraAtual}");
                            token = 0;
                            palavraAtual = string.Empty;
                        }
                        break;
                    case 7:
                        if (char.IsDigit(c))
                            palavraAtual += c;
                        proximo = linha.ElementAtOrDefault(index + 1);
                        if (!char.IsDigit(proximo))
                        {
                            parser.Add($"numeroDecimal {palavraAtual}");
                            palavraAtual = string.Empty;
                            token = 0;
                        }
                        break;
                }
            }
            return parser;
        }

        private string VerificarPalavraReservada(string palavra)
        {
            if (palavra.Equals("if"))
                return "se";
            else if (palavra.Equals("else"))
                return "senao";
            else if (palavra.Equals("while"))
                return "enquanto";
            else if (palavra.Equals("for"))
                return "para";
            else if (palavra.Equals("do"))
                return "faca";
            else if (palavra.Equals("break"))
                return "pare";
            else if (palavra.Equals("continue"))
                return "paranao";
            else if (palavra.Equals("switch"))
                return "comute";
            else if (palavra.Equals("case"))
                return "caso";
            else if (palavra.Equals("int") || palavra.Equals("float") || palavra.Equals("char"))
                return $"tipo {palavra}";
            else
                return $"identificador {palavra}";
        }
    }
}