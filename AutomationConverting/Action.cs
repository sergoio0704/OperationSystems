using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomatonMinimization
{
    public class Action
    {
        private readonly string _inputSignal;
        private readonly string _outputSignal;
        private readonly string _newState;

        public Action( string inputSignal, string outputSignal, string newState )
        {
            _inputSignal = inputSignal;
            _outputSignal = outputSignal;
            _newState = newState;
        }

        public string InputSignal => _inputSignal;
        public string OutputSignal => _outputSignal;
        public string NewState => _newState;

        public override bool Equals( object obj )
        {
            return obj is Action action
                && InputSignal == action.InputSignal
                && OutputSignal == action.OutputSignal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine( InputSignal, OutputSignal );
        }

        public static int GetActionsHashCode( List<Action> actions )
        {
            return actions.Select( a => a.GetHashCode() ).Last();
        }
    }
}
