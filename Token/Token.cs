namespace pascal_lexer;

public class Token
{
    public readonly TokenType TokenType;
    
    public readonly FilePosition FilePosition;

    public readonly string Value;

    public Token(TokenType tokenType, FilePosition filePosition, string value)
    {
        TokenType = tokenType;
        FilePosition = filePosition;
        Value = value;
    }

    public override string ToString()
    {
        return $"{TokenType.ToString()} {FilePosition.ToString()} \"{Value}\"";
    }
}