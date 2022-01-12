using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatonMinimization
{
    public class Program
    {
        private static readonly string InputPath = @"../../../MooreInput.txt";
        private static readonly string OutputPath = @"../../../Output.txt";

        public static void Main( string[] args )
        {
            IMinimizer minimizer;
            using ( StreamReader inputStream = new StreamReader( InputPath ) )
            {
                string minimizerType = inputStream.ReadLine()?.Trim();
                minimizer = MinimizerFactory.GetMinimizer( minimizerType );
                minimizer.Read( inputStream );
            }

            minimizer.Minimize();

            using ( StreamWriter StreamWriter = new StreamWriter( OutputPath ) )
            {
                minimizer.Write( StreamWriter );
            }
        }
    }
}
