using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public class Transition
    {
        private readonly string _prevState;
        private readonly string _newState;
        private readonly string _inputSignal;
        public Transition( string prevState, string newState, string inputSignal )
        {
            _prevState = prevState;
            _newState = newState;
            _inputSignal = inputSignal;
        }

        public string PrevState => _prevState;
        public string NewState => _newState;
        public string InputSignal => _inputSignal;
    }
}
