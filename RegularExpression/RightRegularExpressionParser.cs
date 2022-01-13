using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public class RightRegularExpressionParser : BaseRegularExpressionParser
    {
        public override void Parse( List<string> lines )
        {
            Dictionary<string, List<Transition>> statesToTransitions = new();

            foreach ( string line in lines )
            {
                string[] leftRightParts = line.Split( " -> " );
                string prevState = leftRightParts[0];
                List<string> transitionResults = leftRightParts[1].Split( " | " ).ToList();

                List<Transition> transitions = new();
                foreach ( string transitionResult in transitionResults )
                {
                    transitions.Add( CreateTransition( prevState, transitionResult ) );
                }

                statesToTransitions.Add( prevState, ConcatTransitionByInputSignal( transitions ) );
            }
            statesToTransitions.Add( EndStateLabel, new List<Transition>() );

            _result = GetNewStatesByValues( statesToTransitions );
        }

        private Transition CreateTransition( string prevState, string transitionResult )
        {
            if ( transitionResult.Length == 1 )
            {
                return new Transition( prevState, EndStateLabel, transitionResult[0].ToString() );
            }

            return new Transition( prevState, transitionResult[1].ToString(), transitionResult[0].ToString() );
        }
    }
}
