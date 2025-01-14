using System.Text.RegularExpressions;

namespace pascal_lexer;

public static class Comment
{
    private static string _lineCommentPattern = "//.*";
    private static string _blockCommentPattern = @"\{(.*?)\}";

    public static bool IsLineComment(string line)
    {
        return Regex.IsMatch(line, _lineCommentPattern);
    }
    
    public static bool IsBlockComment(string line)
    {
        return Regex.IsMatch(line, _blockCommentPattern);
    }
}