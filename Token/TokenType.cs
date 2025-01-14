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
