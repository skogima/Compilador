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
        #region Tipos primitivos
        Int,
        Float,
        Char,
        Void,
        #endregion
        #region Símbolos
        AbreParenteses,
        FechaParenteses,
        AbreChaves,
        FechaChaves,
        Ponto,
        PontoVirgula,
        Virgula,
        Apostrofo,
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

        Identificador,
        Atribuicao,
        Numero
    }
}
