namespace pascal_lexer;

public static class KeyWord
{
    public static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "ARRAY", TokenType.Array },
        { "BEGIN", TokenType.Begin },
        { "ELSE", TokenType.Else },
        { "END", TokenType.End },
        { "IF", TokenType.If },
        { "OF", TokenType.Of },
        { "OR", TokenType.Or },
        { "PROGRAM", TokenType.Program },
        { "PROCEDURE", TokenType.Procedure },
        { "THEN", TokenType.Then },
        { "TYPE", TokenType.Type },
        { "VAR", TokenType.Var }
    };

    public static bool IsToken(string word)
    {
        return Keywords.ContainsKey(word.ToUpper());
    }

    public static TokenType GetTokenType(string word)
    {
        return Keywords[word.ToUpper()];
    }
}