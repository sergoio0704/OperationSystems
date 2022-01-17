using System.Collections.Generic;
using System.IO;

namespace Determination
{
    class Program
    {
        const string InputPath = "../../../LeftGrammaInput.txt";
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

            IDeterminator parser = DeterminatorFactory.GetDeterminator( grammarType );
            parser.Determinate( lines );

            using ( var streamWriter = new StreamWriter( OutputPath ) )
            {
                foreach ( var stateToTransitions in parser.GetResult() )
                {
                    streamWriter.Write( $"{stateToTransitions.Key }  " );
                    foreach ( var transition in stateToTransitions.Value )
                    {
                        streamWriter.Write( $"{transition.NewState}({transition.InputSignal}) " );
                    }
                    streamWriter.WriteLine();
                }
            }
        }
    }
}
