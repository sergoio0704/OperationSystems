using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpLexer
{
    class Program
    {
        const string InputFilePath = "../../../InputFile.txt";
        const string OutputFilePath = "../../../OutputFile.txt";
        static void Main(string[] args)
        {
            List<string> lines = File.ReadAllLines( InputFilePath ).ToList();

            List<Token> tokens = new Tokenizer()
                .Tokenize( lines )
                .GetResult();

            using (StreamWriter outputStream = new StreamWriter( OutputFilePath ) )
            {
                foreach (Token token in tokens)
                {
                    outputStream.WriteLine( token.ToString() );
                }
            }
        }
    }
}