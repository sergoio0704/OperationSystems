using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatonMinimization
{
    public class MooreMinimizer : BaseMinimizer
    {
        public override void Read( StreamReader inputStream )
        {
            int actionsCounter = 0;
            string inputString;
            List<string> outputSignals = inputStream.ReadLine()?.Trim().Split( " " ).ToList();
            List<string> states = inputStream.ReadLine()?.Trim().Split( " " ).ToList();
            while ( ( inputString = inputStream.ReadLine()?.Trim() ) != null )
            {
                actionsCounter++;
                List<Action> actions = inputString.Split( " " ).Select( (actionString, index) =>
                    new Action(
                           Convert.ToString( actionsCounter ),
                           outputSignals[index],
                           actionString
                     ) ).ToList();

                for ( int i = 0; i < actions.Count; i++ )
                {
                    string key = states[i];
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

            _currentEqualClasses = CombineEqualStates( _initialStatesToActions );
            _currentStatesToActions = CreateStatesToActions( _currentEqualClasses );
        }

        public override void Write( StreamWriter outputStream )
        {
            List<string> inputSignals = _result.First().Value.Values.Select( item => item.OutputSignal ).ToList();
            outputStream.WriteLine( $"   {String.Join( "   ", inputSignals ) }" );

            List<string> heads = _result.SelectMany( a => a.Value.Select( b => b.Key ) ).Distinct().ToList();
            outputStream.WriteLine( $"   {String.Join( "  ", heads ) }" );

            foreach ( KeyValuePair<string, Dictionary<string, Action>> inputSignalToStates in _result )
            {
                outputStream.Write( $"{inputSignalToStates.Key}  " );
                foreach ( KeyValuePair<string, Action> stateToAction in inputSignalToStates.Value )
                {
                    Action action = stateToAction.Value;
                    outputStream.Write( $"{action.NewState}  " );
                }
                outputStream.WriteLine();
            }
        }
    }
}
