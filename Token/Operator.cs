namespace pascal_lexer;

public static class Operator
{
    public static readonly Dictionary<string, TokenType> Operators = new()
    {
        { "*", TokenType.Multiplication },
        { "+", TokenType.Plus },
        { "-", TokenType.Minus },
        { "/", TokenType.Divide },
        { ";", TokenType.Semicolon },
        { ",", TokenType.Comma },
        { "(", TokenType.LeftParen },
        { ")", TokenType.RightParen },
        { "[", TokenType.LeftBracket },
        { "]", TokenType.RightBracket },
        { "=", TokenType.Eq },
        { ">", TokenType.Greater },
        { "<", TokenType.Less },
        { "<=", TokenType.LessEq },
        { ">=", TokenType.GreaterEq },
        { "<>", TokenType.NotEq },
        { ":", TokenType.Colon },
        { ":=", TokenType.Assign },
        { ".", TokenType.Dot }
    };

    public static bool IsToken(string token)
    {
        return Operators.ContainsKey(token);
    }
}