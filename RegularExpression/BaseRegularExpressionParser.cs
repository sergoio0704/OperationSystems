using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public abstract class BaseRegularExpressionParser : IRegularExpressionParser
    {
        protected Dictionary<string, List<Transition>> _result = new();
        protected const string EndStateLabel = "F";

        public Dictionary<string, List<Transition>> GetResult()
        {
            return _result;
        }

        public abstract void Parse( List<string> lines );

        protected Dictionary<string, List<Transition>> GetNewStatesByValues( 
            Dictionary<string, 
            List<Transition>> statesToTransitions )
        {
            var result = statesToTransitions.Aggregate( new Dictionary<string, List<Transition>>(), ( stt, next ) =>
            {
                var newStatesToTransitions = next.Value.Where( t => !statesToTransitions.ContainsKey( t.NewState ) )
                    .Select( t =>
                        new KeyValuePair<string, List<Transition>>(
                            t.NewState,
                            ConcatTransitionByInputSignal( t.NewState.Length == 1
                                ? next.Value
                                : t.NewState.SelectMany( t => statesToTransitions[t.ToString()] ).Distinct().ToList()
                            )
                        )
                    )
                    .ToList();

                foreach ( var newStateToTransition in newStatesToTransitions )
                {
                    stt.Add( newStateToTransition.Key, newStateToTransition.Value );
                }

                return stt;
            } );

            var newStatesToTransitions = statesToTransitions.Concat( result ).ToDictionary( x => x.Key, x => x.Value );
            if ( newStatesToTransitions.Count == statesToTransitions.Count )
            {
                return newStatesToTransitions;
            }

            return GetNewStatesByValues( newStatesToTransitions );
        }

        protected List<Transition> ConcatTransitionByInputSignal( List<Transition> transitions )
        {
            return transitions.Aggregate(
                    new List<Transition>(),
                    ( list, next ) => {
                        int i = list.FindIndex( item => item.InputSignal == next.InputSignal );
                        if ( i == -1 )
                        {
                            list.Add( next );
                        }
                        else
                        {
                            Transition transition = list[i];
                            list[i] = new Transition(
                                transition.PrevState,
                                transition.NewState + next.NewState,
                                transition.InputSignal );
                        }

                        return list;
                    } );
        }
    }
}
