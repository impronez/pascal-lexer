using System.Text.RegularExpressions;

namespace pascal_lexer;

public static class Literal
{
    private static readonly string IdentifierPattern = "^[@a-zA-Z_][a-zA-Z0-9_]*$";
    private static readonly string StringLiteralPattern = "'(?:''|[^'])*'";
    private static readonly string IntegerLiteralPattern = @"^\d+$";
    private static readonly string FloatLiteralPattern = @"^\d+\.\d+$";
    
    public static bool IsIdentifier(string identifier) => Regex.IsMatch(identifier, IdentifierPattern);
    public static bool IsStringLiteral(string literal) => Regex.IsMatch(literal, StringLiteralPattern);
    public static bool IsIntegerLiteral(string literal) => Regex.IsMatch(literal, IntegerLiteralPattern);
    public static bool IsFloatLiteral(string literal) => Regex.IsMatch(literal, FloatLiteralPattern);
}