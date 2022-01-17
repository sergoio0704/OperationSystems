using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determination
{
    public class LeftDeterminator : BaseDeterminator
    {
        public override void Determinate( List<string> lines )
        {
            Dictionary<string, List<Transition>> statesToTransitions = new();

            foreach ( string line in lines )
            {
                string[] leftRightParts = line.Split( " -> " );
                string newState = leftRightParts[0];
                List<string> transitionResults = leftRightParts[1].Split( " | " ).ToList();

                List<Transition> transitions = new();
                foreach ( string transitionResult in transitionResults )
                {
                    Transition transition = CreateTransition( newState, transitionResult );
                    if ( !statesToTransitions.ContainsKey( transition.PrevState ) )
                    {
                        statesToTransitions.Add( transition.PrevState, new List<Transition> { transition } );
                    }
                    else
                    {
                        int index = statesToTransitions[transition.PrevState].FindIndex( t => t.InputSignal == transition.InputSignal );
                        if ( index == -1 )
                        {
                            statesToTransitions[transition.PrevState].Add( transition );
                            continue;
                        }

                        Transition equalTransition = statesToTransitions[transition.PrevState][index];
                        statesToTransitions[transition.PrevState][index] = new Transition( 
                            transition.PrevState, 
                            transition.NewState + equalTransition.NewState,
                            transition.InputSignal );
                    }
                }
            }

            _result = GetNewStatesByValues( statesToTransitions );
        }

        private Transition CreateTransition( string newState, string transitionResult )
        {
            if ( transitionResult.Length == 1 )
            {
                return new Transition( EndStateLabel, newState, transitionResult[0].ToString() );
            }

            return new Transition( transitionResult[0].ToString(), newState, transitionResult[1].ToString() );
        }
    }
}
