using System;
using Lexer;

public class Class1
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments:
        Lexer lolo = new Lexer();
        lolo.Analyze();
        System.Console.WriteLine(args.Length);
    }
}
