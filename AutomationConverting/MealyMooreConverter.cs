using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatonMinimization
{
    public class MealyMooreConverter : IConverter
    {
        private Dictionary<string, List<Action>> _initialStatesToActions;
        private Dictionary<string, Dictionary<Tuple<string, string>, Action>> _result;
        private SortedDictionary<Tuple<string, string>, string> _mooreStates;
        private int _mooreStateCounter = 0;

        public MealyMooreConverter()
        {
            _initialStatesToActions = new Dictionary<string, List<Action>>();
            _mooreStates = new SortedDictionary<Tuple<string, string>, string>();
            _result = new Dictionary<string, Dictionary<Tuple<string, string>, Action>>();
        }

        public void Convert()
        {
            BuildMooreStates();
            if ( _mooreStates.Count == 0 )
            {
                throw new Exception( "moore states was empty" );
            }

            BuildConvertingResult();
        }

        public void Write( StreamWriter outputStream )
        {
            foreach ( var moreState in _mooreStates )
            {
                outputStream.WriteLine( $"{{{moreState.Key.Item1};{moreState.Key.Item2}}} -> {{{moreState.Value}}}" );
            }
            outputStream.WriteLine();

            var heads = _result.SelectMany( a => a.Value.Select( b => $"{b.Key.Item1}/{b.Key.Item2}" ) ).Distinct().ToList();
            outputStream.WriteLine( $"   {String.Join( " ", heads ) }" );

            foreach ( var inputSignalToStates in _result )
            {
                outputStream.Write( $"{inputSignalToStates.Key}  " );
                foreach ( var stateToAction in inputSignalToStates.Value )
                {
                    Action action = stateToAction.Value;
                    outputStream.Write( $"{action.NewState}   " );
                }
                outputStream.WriteLine();
            }

        }

        public void Read( StreamReader inputStream )
        {
            int actionsCounter = 0;
            string inputString;
            while ( ( inputString = inputStream.ReadLine()?.Trim() ) != null )
            {
                actionsCounter++;
                List<Action> actions = inputString.Split( " " ).Select( actionString =>
                    new Action(
                           System.Convert.ToString( actionsCounter ),
                           actionString.Split( "/" ).Last(),
                           actionString.Split( "/" ).First()
                     ) ).ToList();

                for ( int i = 0; i < actions.Count; i++ )
                {
                    string key = ( i + 1 ).ToString();
                    if ( !_initialStatesToActions.ContainsKey( key ) )
                    {
                        List<Action> stateActions = new List<Action> { actions[i] };
                        _initialStatesToActions.Add( key, stateActions );
                    }
                    else
                    {
                        _initialStatesToActions[key].Add( actions[i] );
                    }
                }
            }
        }

        private void BuildConvertingResult()
        {
            foreach ( var mooreState in _mooreStates )
            {
                var actions = _initialStatesToActions[mooreState.Key.Item1];
                foreach ( var action in actions )
                {
                    var key = Tuple.Create( action.NewState, action.OutputSignal );
                    var nextMooreState = _mooreStates[key];
                    if ( !_result.ContainsKey( action.InputSignal ) )
                    {
                        _result.Add( action.InputSignal, new Dictionary<Tuple<string, string>, Action>() );
                    }

                    _result[action.InputSignal].Add(
                            Tuple.Create( mooreState.Value, mooreState.Key.Item2 ),
                            new Action( action.InputSignal, action.OutputSignal, nextMooreState )
                        );
                }
            }
        }

        private void BuildMooreStates()
        {
            foreach ( var stateToAction in _initialStatesToActions )
            {
                foreach ( var action in stateToAction.Value )
                {
                    var key = Tuple.Create( action.NewState, action.OutputSignal );
                    if ( !_mooreStates.ContainsKey( key ) )
                    {
                        _mooreStates.Add( key, GenerateMooreState() );
                    }
                }
            }
        }

        private string GenerateMooreState()
        {
            _mooreStateCounter++;
            return $"B{_mooreStateCounter}";
        }
    }
}
