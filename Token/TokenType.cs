namespace pascal_lexer;

public enum TokenType
{
    // Ключевые слова
    Array,
    Begin,
    Else,
    End,
    If,
    Of,
    Or,
    Program,
    Procedure,
    Then,
    Type,
    Var,

    // Операторы и знаки пунктуации
    Multiplication, // *
    Plus,           // +
    Minus,          // -
    Divide,         // /
    Semicolon,      // ;
    Comma,          // ,
    LeftParen,      // (
    RightParen,     // )
    LeftBracket,    // [
    RightBracket,   // ]
    Eq,             // =
    Greater,        // >
    Less,           // <
    LessEq,         // <=
    GreaterEq,      // >=
    NotEq,          // <>
    Colon,          // :
    Assign,         // :=
    Dot,            // .

    // Литералы и идентификаторы
    Identifier,     // Идентификатор
    String,         // Строка
    Integer,        // Целое число
    Float,          // Вещественное число

    // Комментарии
    LineComment,    // Линейный комментарий //
    BlockComment,   // Блочный комментарий {}

    // Некорректные лексемы
    Bad,             // Ошибочная лексема
    
    // Специальный токен
    None
}

public static class TokenTypeString
{
    // Словарь для хранения соответствий
    private static readonly Dictionary<TokenType, string> TokenStrings = new ()
    {
        // Ключевые слова
        { TokenType.Array, "ARRAY" },
        { TokenType.Begin, "BEGIN" },
        { TokenType.Else, "ELSE" },
        { TokenType.End, "END" },
        { TokenType.If, "IF" },
        { TokenType.Of, "OF" },
        { TokenType.Or, "OR" },
        { TokenType.Program, "PROGRAM" },
        { TokenType.Procedure, "PROCEDURE" },
        { TokenType.Then, "THEN" },
        { TokenType.Type, "TYPE" },
        { TokenType.Var, "VAR" },

        // Операторы и знаки пунктуации (с описанием словами и верхний регистр)
        { TokenType.Multiplication, "MULTIPLICATION" }, // *
        { TokenType.Plus, "PLUS" },           // "+"
        { TokenType.Minus, "MINUS" },         // "-"
        { TokenType.Divide, "DIVIDE" },       // "/"
        { TokenType.Semicolon, "SEMICOLON" }, // ";"
        { TokenType.Comma, "COMMA" },         // ","
        { TokenType.LeftParen, "LEFT_PAREN" },// "("
        { TokenType.RightParen, "RIGHT_PAREN" }, // ")"
        { TokenType.LeftBracket, "LEFT_BRACKET" }, // "["
        { TokenType.RightBracket, "RIGHT_BRACKET" }, // "]"
        { TokenType.Eq, "EQ" },          // "="
        { TokenType.Greater, "GREATER" },    // ">"
        { TokenType.Less, "LESS" },          // "<"
        { TokenType.LessEq, "LESS_EQ" }, // "<="
        { TokenType.GreaterEq, "GREATER_EQ" }, // ">="
        { TokenType.NotEq, "NOT_EQ" },    // "<>"
        { TokenType.Colon, "COLON" },        // ":"
        { TokenType.Assign, "ASSIGN" },      // ":="
        { TokenType.Dot, "DOT" },            // "."

        // Литералы и идентификаторы
        { TokenType.Identifier, "IDENTIFIER" },
        { TokenType.String, "STRING" },
        { TokenType.Integer, "INTEGER" },
        { TokenType.Float, "FLOAT" },

        // Комментарии
        { TokenType.LineComment, "LINE_COMMENT" },
        { TokenType.BlockComment, "BLOCK_COMMENT" },

        // Некорректные лексемы
        { TokenType.Bad, "BAD" },
    
        // Специальный токен
        { TokenType.None, "NONE" }
    };

    // Метод для получения строкового представления типа токена
    public static string GetTokenString(TokenType tokenType)
    {
        // Проверка на существование токена в словаре
        if (TokenStrings.TryGetValue(tokenType, out var tokenString))
        {
            return tokenString;
        }

        // Возвращаем пустую строку или можно выбросить исключение, если токен не найден
        return string.Empty; // Или throw new ArgumentException("Invalid TokenType");
    }
}