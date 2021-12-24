using AutomatonMinimization;
using System;
using System.IO;

namespace AutomationConverting
{
    public class Program
    {
        private static readonly string InputPath = @"../../../Input.txt";
        private static readonly string OutputPath = @"../../../Output.txt";

        public static void Main( string[] args )
        {
            IConverter minimizer;
            using ( StreamReader inputStream = new StreamReader( InputPath ) )
            {
                string minimizerType = inputStream.ReadLine()?.Trim();
                minimizer = ConverterFactory.GetConverter( minimizerType );
                minimizer.Read( inputStream );
            }

            minimizer.Convert();

            using ( StreamWriter StreamWriter = new StreamWriter( OutputPath ) )
            {
                minimizer.Write( StreamWriter );
            }
        }
    }
}
