using System.Text.RegularExpressions;

namespace pascal_lexer;

public class Lexer(string filePath)
{
    private readonly StreamReader _reader = new (filePath);
    
    private int _lineNumber;
    private int _columnNumber;
    
    private Stack<int> _numberStack = new();
    
    private string? _line = "";

    public Token GetNextToken()
    {
        ReadNextLineIfNeed();
        
        if (_line == null)
            return GetEndOfFileToken();

        string tokenValue = GetTokenFromLine();
        TokenType type = GetTokenType(tokenValue);
        FilePosition filePosition = GetFilePosition(tokenValue);

        return new Token(type, filePosition, tokenValue);
    }

    private string GetTokenFromLine()
    {
        string value = "";

        for (; _columnNumber < _line!.Length; _columnNumber++)
        {
            if (string.IsNullOrEmpty(value) && IsBlockCommentStart(_line[_columnNumber]))
            {
                value = GetBlockComment();
                break;
            }

            if (string.IsNullOrEmpty(value) && IsStringLiteralStart(_line[_columnNumber]))
            {
                value = GetStringLiteral();
                break;
            }
            
            if (IsLineComment(value, _line[_columnNumber]))
            {
                value += _line.Substring(_columnNumber, _line.Length - _columnNumber);
                _columnNumber = _line.Length;
                break;
            }
            
            if (string.IsNullOrEmpty(value) && IsNumber(_line[_columnNumber].ToString()))
            {
                value = ParseNumber(_line);
                break;
            }
            
            if (IsOperatorByDoubleSymbols(value, _line[_columnNumber]))
            {
                value += _line[_columnNumber];
                continue;
            }

            if (Operator.IsToken(value) && !char.IsWhiteSpace(_line[_columnNumber]) && !Operator.IsToken(_line[_columnNumber].ToString()))
            {
                break;
            }
            
            bool shouldBreak = 
                !string.IsNullOrEmpty(value) && (
                    IsBlockCommentStart(_line[_columnNumber]) ||  // Начало блочного комментария
                    (!IsNumber(value) && IsNumber(_line[_columnNumber].ToString())) || // Число после другого типа токена
                    Operator.IsToken(_line[_columnNumber].ToString()) ||  // Оператор
                    IsStringLiteralStart(_line[_columnNumber]) ||
                    (char.IsWhiteSpace(_line[_columnNumber]) && !string.IsNullOrEmpty(value)) // Пробел после накопленного значения
                );

            if (shouldBreak)
            {
                break;
            }

            if (!char.IsWhiteSpace(_line[_columnNumber]))
            {
                value += _line[_columnNumber];
            }
        }
        
        return value;
    }

    private string GetStringLiteral()
    {
        var match = Regex.Match(_line, @"'[^']*'");
        
        // Если найдена подходящая подстрока, вернуть её
        if (match.Success)
        {
            _columnNumber += match.Value.Length;
            return match.Value;
        }
        
        _columnNumber = _line.Length;
        return _line.Substring(_columnNumber, _line.Length - _columnNumber + 1);
    }

    private bool IsStringLiteralStart(char ch)
    {
        return ch == '\'';
    }

    private FilePosition GetFilePosition(string value)
    {
        int lineNumber = _lineNumber;
        int columnNumber = _columnNumber - value.Length + 1;
        if (_numberStack.Count != 0)
        {
            lineNumber = _numberStack.Pop();
            columnNumber = _numberStack.Pop();
        }
        
        return new FilePosition(lineNumber, columnNumber);
    }

    private string GetBlockComment()
    {
        string value = "";
        _numberStack.Push(_columnNumber);
        _numberStack.Push(_lineNumber);

        while (true)
        {
            if (_line!.Contains('}'))
            {
                value += _line.Substring(_columnNumber, _line.IndexOf('}') - _columnNumber + 1);
                _columnNumber = _line.IndexOf('}') + 1;
                break;
            }
            else
            {
                value += _line.Substring(_columnNumber, _line.Length - _columnNumber);
                _columnNumber = _line.Length;
                ReadNextLineIfNeed();

                if (_line == null)
                {
                    break;
                }
            }
        }
        
        return value;
    }
    
    private bool IsBlockCommentStart(char ch)
    {
        return ch == '{';
    }

    private bool IsLineComment(string line, char ch)
    {
        return line + ch == "//";
    }

    private string ParseNumber(string line)
    {
        string value = "";
        
        for (; _columnNumber < line.Length; _columnNumber++)
        {
            if (!char.IsWhiteSpace(line[_columnNumber]))
            {
                value += line[_columnNumber];
            }
            else
            {
                break;
            }
        }
        
        var match = Regex.Match(value, @"^\d+(\.\d+)?");
        if (match.Success)
        {
            _columnNumber += match.Value.Length - value.Length;
            return match.Value;
        }

        return value;
    }

    private void ReadNextLineIfNeed()
    {
        while (_line != null && IsReadLineNeed())
        {
            _line  = _reader.ReadLine();
            _lineNumber++;
            _columnNumber = 0;
        }
    }

    private static bool IsOperatorByDoubleSymbols(string value, char operatorSymbol)
    {
        return Operator.IsToken(value + operatorSymbol);
    }

    private static bool IsNumber(string value)
    {
        return Regex.IsMatch(value, @"\d+(\.\d+)?");
    }

    private TokenType GetTokenType(string tokenValue)
    {
        if (KeyWord.IsToken(tokenValue))
        {
            return KeyWord.GetTokenType(tokenValue);
        }

        if (Operator.IsToken(tokenValue))
        {
            return Operator.Operators[tokenValue];
        }

        if (Literal.IsIdentifier(tokenValue))
        {
            return TokenType.Identifier;
        }

        if (Literal.IsIntegerLiteral(tokenValue))
        {
            return TokenType.Integer;
        }

        if (Literal.IsFloatLiteral(tokenValue))
        {
            return TokenType.Float;
        }

        if (Literal.IsStringLiteral(tokenValue))
        {
            return TokenType.String;
        }

        if (Comment.IsLineComment(tokenValue))
        {
            return TokenType.LineComment;
        }

        if (Comment.IsBlockComment(tokenValue))
        {
            return TokenType.BlockComment;
        }
        
        return TokenType.Bad;
    }

    private Token GetEndOfFileToken()
    {
        return new Token(TokenType.None, new FilePosition(_lineNumber, _columnNumber), "");
    }

    private bool IsReadLineNeed()
    {
        return _columnNumber == _line!.Length || string.IsNullOrEmpty(_line.Trim());
    }
}