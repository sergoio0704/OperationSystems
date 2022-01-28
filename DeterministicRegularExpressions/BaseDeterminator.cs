using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determination
{
    public abstract class BaseDeterminator : IDeterminator
    {
        protected Dictionary<string, List<Transition>> _result = new();
        protected const string EndStateLabel = "F";

        public Dictionary<string, List<Transition>> GetResult()
        {
            return _result;
        }

        public abstract void Determinate( List<string> lines );

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
                                : t.NewState.SelectMany( c => c.ToString() ).Distinct().SelectMany( t => statesToTransitions[t.ToString()] ).ToList()
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

        protected Dictionary<string, List<Transition>> DeleteZeroTransitions(
            Dictionary<string, List<Transition>> statesToTransitions )
        {
            Dictionary<string, List<string>> statesToTransitionsWithEpsilon = new Dictionary<string, List<string>>();
            Dictionary<string, List<Transition>> newStatesToTransitions = new Dictionary<string, List<Transition>>();

            foreach ( KeyValuePair<string, List<Transition>> stateToTransition in statesToTransitions )
            {
                statesToTransitionsWithEpsilon[stateToTransition.Key] = new() { stateToTransition.Key };
                List<string> statesWithEpsilon = stateToTransition.Value.Where( ti => ti.InputSignal == "e" )
                    .Select( ti => ti.NewState )
                    .Distinct()
                    .ToList();
                statesToTransitionsWithEpsilon[stateToTransition.Key].AddRange( statesWithEpsilon );
                
                List<string> tempAchievables = statesWithEpsilon;
                while ( tempAchievables.Any() )
                {
                    List<string> newStatesWithEpsilon = new List<string>();
                    foreach ( var tempAchivable in tempAchievables )
                    {
                        newStatesWithEpsilon.AddRange( statesToTransitions[tempAchivable]
                            .Where( ti => ti.InputSignal == "e" )
                            .Select( ti => ti.NewState )
                            .Distinct() );
                    }

                    tempAchievables = newStatesWithEpsilon.Distinct().ToList();
                    statesToTransitionsWithEpsilon[stateToTransition.Key].AddRange( tempAchievables ); 
                }

                HashSet<Transition> newTransitions = new HashSet<Transition>();
                foreach ( var stateWithEpsilon in statesToTransitionsWithEpsilon[stateToTransition.Key] )
                {
                    List<Transition> transitionsOfAchievableState = statesToTransitions[stateWithEpsilon].Where( ti => ti.InputSignal != "e" ).ToList();
                    IEnumerable<Transition> b = transitionsOfAchievableState.Select( t => new Transition( stateWithEpsilon, t.NewState, t.InputSignal ) );
                    newTransitions.UnionWith( b );
                }

                newStatesToTransitions[stateToTransition.Key] = newTransitions.ToList();
            }   

            return newStatesToTransitions;
        }

        protected List<Transition> ConcatTransitionByInputSignal( List<Transition> transitions )
        {
            return transitions.Aggregate(
                    new List<Transition>(),
                    ( list, next ) =>
                    {
                        int i = list.FindIndex( item => item.InputSignal == next.InputSignal );
                        if ( i == -1 )
                        {
                            list.Add( next );
                        }
                        else
                        {
                            Transition transition = list[i];
                            if ( !transition.NewState.Contains( next.NewState ) )
                            {
                                list[i] = new Transition(
                                transition.PrevState,
                                transition.NewState + next.NewState,
                                transition.InputSignal );
                            }
                        }

                        return list;
                    } );
        }
    }
}
