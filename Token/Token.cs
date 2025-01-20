namespace pascal_lexer;

public class Token
{
    public readonly TokenType TokenType;
    
    public readonly FilePosition FilePosition;

    public readonly string Value;

    public readonly int Length;

    public Token(TokenType tokenType, FilePosition filePosition, string value)
    {
        TokenType = tokenType;
        FilePosition = filePosition;
        Value = value;
        Length = Value.Length;
    }

    public override string ToString()
    {
        return $"{TokenTypeString.GetTokenString(TokenType)} {FilePosition.ToString()} \"{Value}\"";
    }
}