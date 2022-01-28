using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Determination
{
    class Program
    {
        const string InputPath = "../../../Input.txt";
        const string OutputPath = "../../../Output.txt";

        static void Main( string[] args )
        {
            List<string> lines = File.ReadAllLines( InputPath ).ToList();

            IDeterminator determinator = new Determinator();
            determinator.Determinate( lines );

            using ( var streamWriter = new StreamWriter( OutputPath ) )
            {
                foreach ( var stateToTransitions in determinator.GetResult() )
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
