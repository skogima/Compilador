namespace Compiler
{
    public enum TipoToken
    {
        Vazio,
        #region Operadores
        Mais,
        Menos,
        Vezes,
        Dividir,
        #endregion
        #region Símbolos
        AbreParenteses,
        FechaParenteses,
        AbreChaves,
        FechaChaves,
        Ponto,
        PontoVirgula,
        Virgula,
        #endregion
        #region Operadores Lógicos
        Igual,
        Menor,
        Maior,
        MenorIgual,
        MaiorIgual,
        Diferente,
        #endregion
        #region Condicional
        Se,
        Senao,
        #endregion
        #region Repetição
        Enquanto,
        DoEnquanto,
        Para,
        #endregion

        Literal,
        FuncaoMain,
        Tipo,
        Continuar,
        Parar,
        Comutar,
        Caso,
        Identificador,
        Atribuicao,
        Numero,
        NumeroDecimal
    }
}
