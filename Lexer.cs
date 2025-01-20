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
            if (!string.IsNullOrEmpty(value) && IsEndSeparator(_line[_columnNumber]))
            {
                break;
            }
            
            if (string.IsNullOrEmpty(value) && IsBlockCommentStart(_line[_columnNumber]))
            {
                value = GetBlockComment();
                break;
            }

            if (string.IsNullOrEmpty(value) && char.IsLetter(_line[_columnNumber]))
            {
                value = GetIdentifier(_line);
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
            
            if (!string.IsNullOrEmpty(value) && IsOperatorByDoubleSymbols(value, _line[_columnNumber]))
            {
                value += _line[_columnNumber];
                continue;
            }

            if (Operator.IsToken(value) && !char.IsWhiteSpace(_line[_columnNumber]) &&
                !Operator.IsToken(_line[_columnNumber].ToString()))
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

    private string GetIdentifier(string line)
    {
        string value = "";

        for (; _columnNumber < line.Length; _columnNumber++)
        {
            if (char.IsLetter(line[_columnNumber]) 
                || char.IsDigit(line[_columnNumber])
                || line[_columnNumber] == '_')
            {
                value += line[_columnNumber];
            }
            else
            {
                break;
            }
        }

        return value;
    }

    private bool IsEndSeparator(char ch)
    {
        return ch is ')' or ']' or ';';
    }

    private string GetStringLiteral()
    {
        var line = _line.Substring(_columnNumber, _line.Length - _columnNumber);
        var match = Regex.Match(line, @"'[^']*'");
        
        // Если найдена подходящая подстрока, вернуть её
        if (match.Success)
        {
            _columnNumber += match.Value.Length;
            return match.Value;
        }

        int start = _columnNumber;
        _columnNumber = _line.Length;
        return _line.Substring(start, _line.Length - start);
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
        bool seenDot = false;
        bool seenExponent = false;

        for (; _columnNumber < line.Length; _columnNumber++)
        {
            char currentChar = line[_columnNumber];

            if (char.IsDigit(currentChar))
            {
                value += currentChar;
            }
            else if (currentChar == '.' && !seenDot && !seenExponent)
            {
                value += currentChar;
                seenDot = true; 
            }
            else if ((currentChar == 'e' || currentChar == 'E') && !seenExponent)
            {
                value += currentChar;
                seenExponent = true;
            }
            else if ((currentChar == '+' || currentChar == '-') && seenExponent && value[^1] is 'e' or 'E')
            {
                value += currentChar;
            }
            else
            {
                break;
            }
        }

        if (Literal.IsIntegerLiteral(value) || Literal.IsFloatLiteral(value))
        {
            return value;
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

        if (tokenValue[0] == '\'' && tokenValue[^1] == '\'')
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