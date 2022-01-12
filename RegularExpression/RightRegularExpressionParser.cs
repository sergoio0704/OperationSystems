using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public class RightRegularExpressionParser : BaseRegularExpressionParser
    {
        private readonly Dictionary<string, List<Transition>> _stateToTransitions = new();
        private readonly Dictionary<string, List<string>> _stateToTransitionResults = new();

        public override void Parse( List<string> lines )
        {
            //Dictionary<string, List<Transition>> stateToTransitions = new();
            //foreach ( string line in lines )
            //{
            //    string[] leftRightParts = line.Split( " -> " );
            //    string prevState = leftRightParts[0];
            //    List<string> transitionResults = leftRightParts[1].Split( " | " ).ToList();

            //    List<Transition> transitions = new();
            //    foreach ( string transitionResult in transitionResults )
            //    {
            //        Transition transition = CreateTransition( prevState, transitionResult );

            //    }
            //}
            foreach ( string line in lines )
            {
                string[] leftRightParts = line.Split( " -> " );
                string prevState = leftRightParts[0];
                List<string> transitionResults = leftRightParts[1].Split( " | " ).ToList();
                if ( !_stateToTransitionResults.TryGetValue( prevState, out List<string> value ) )
                {
                    _stateToTransitionResults.Add( prevState, transitionResults );
                }
            }

            foreach ( var stateToTranslationResult in _stateToTransitionResults )
            {
                ParseStateToTransitions( stateToTranslationResult );
            }
            
        }

        private void ParseStateToTransitions( KeyValuePair<string, List<string>> stateToTranslationResult ) 
        {
            foreach ( var translationResult in stateToTranslationResult.Value )
            {
                
            }
        }

        private Transition CreateTransition( string prevState, string transitionResult )
        {
            string newState = transitionResult[1].ToString();
            string inputSignal = transitionResult[0].ToString();
            if ( transitionResult.Length == 1 )
            {
                return new Transition( prevState, EndStateLabel, inputSignal );
            }

            return new Transition( prevState, newState, inputSignal );
        }
    }
}
