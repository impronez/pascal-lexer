namespace pascal_lexer;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: PascalLexer <input_file> <output_file>");
            return;
        }
        
        string inputFile = args[0];
        string outputFile = args[1];
        
        StreamWriter writer = new StreamWriter(outputFile);

        Lexer lexer = new Lexer(inputFile);
        
        while (true)
        {
            Token token = lexer.GetNextToken();
            
            if (token.TokenType == TokenType.None)
            {
                break;
            }

            if (token.TokenType is TokenType.BlockComment or TokenType.LineComment)
            {
                continue;
            }
            
            Console.WriteLine(token.ToString());
            writer.WriteLine(token.ToString());
        }
        
        writer.Close();
    }
}