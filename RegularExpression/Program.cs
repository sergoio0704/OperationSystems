using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegularExpression
{
    class Program
    {
        const string InputPath = "../../../RightGrammaInput.txt";
        const string OutputPath = "../../../Output.txt";
        static void Main( string[] args )
        {
            List<string> lines = new();
            GrammarType grammarType;
            using ( var streamReader = new StreamReader( InputPath ) )
            {
                grammarType = streamReader.ReadLine().ToEnum<GrammarType>();

                string line;
                while ( ( line = streamReader.ReadLine() ) != null )
                {
                    lines.Add( line );
                }
            }

            IRegularExpressionParser parser = RegularExpressionParserFactory.GetRegularExpressionParser( grammarType );
            parser.Parse( lines );
        }
    }
}
