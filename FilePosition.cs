namespace pascal_lexer;

public readonly struct FilePosition(int line, int column)
{
    public readonly int Line = line;
    public readonly int Column = column;
    
    public override string ToString() => $"({Line}, {Column})";
}