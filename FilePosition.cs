namespace pascal_lexer;

public struct FilePosition(int line, int column)
{
    public int Line = line;
    public int Column = column;
    
    public override string ToString() => $"({Line}, {Column})";
}