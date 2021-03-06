using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonMinimization
{
    public abstract class BaseMinimizer : IMinimizer
    {
        protected Dictionary<string, List<Action>> _initialStatesToActions;
        protected Dictionary<string, List<Action>> _currentStatesToActions;
        protected Dictionary<string, List<string>> _prevEqualClasses;
        protected Dictionary<string, List<string>> _currentEqualClasses;
        protected Dictionary<string, Dictionary<string, Action>> _result;
        protected Dictionary<string, string> _hashToEqualClassNames;

        protected BaseMinimizer()
        {
            _initialStatesToActions = new Dictionary<string, List<Action>>();
            _prevEqualClasses = new Dictionary<string, List<string>>();
            _currentEqualClasses = new Dictionary<string, List<string>>();
            _result = new Dictionary<string, Dictionary<string, Action>>();
            _hashToEqualClassNames = new Dictionary<string, string>();
        }

        public void Minimize()
        {
            while ( !CheckEqualityStates() )
            {
                MinimizeStep();
            }

            if ( _currentEqualClasses.Count > 0 )
            {
                _result = BuildMinimizeResult();
            }
            else
            {
                throw new Exception();
            }
        }

        public abstract void Read( StreamReader inputStream );

        public abstract void Write( StreamWriter outputStream );

        protected Dictionary<string, Dictionary<string, Action>> BuildMinimizeResult()
        {
            Dictionary<string, Dictionary<string, Action>> result = new Dictionary<string, Dictionary<string, Action>>();

            int counter = 0;
            foreach ( var equalClass in _currentEqualClasses )
            {
                _hashToEqualClassNames.Add( equalClass.Key, $"S{counter}" );
                counter++;
            }

            foreach ( KeyValuePair<string, List<string>> equalClass in _currentEqualClasses )
            {
                string resultEqualClass = _hashToEqualClassNames[equalClass.Key];
                string resultEqualClassState = equalClass.Value.First();
                List<Action> actions = _initialStatesToActions[resultEqualClassState];
                foreach ( Action action in actions )
                {
                    string resultEqualClassNewStateHashCode = _currentEqualClasses
                            .Where( a => a.Value.Contains( action.NewState ) )
                            .Select( a => a.Key ).First();
                    string resultEqualClassNewState = _hashToEqualClassNames[resultEqualClassNewStateHashCode];

                    if ( !result.ContainsKey( action.InputSignal ) )
                    {
                        result.Add( action.InputSignal, new Dictionary<string, Action>() );

                    }

                    result[action.InputSignal].Add(
                            resultEqualClass,
                            new Action( action.InputSignal, action.OutputSignal, resultEqualClassNewState ) );
                }
            }

            return result;
        }

        protected bool CheckEqualityStates()
        {
            if ( _currentEqualClasses.Count == 0 || _prevEqualClasses.Count == 0 )
            {
                return false;
            }

            return _prevEqualClasses.Count == _currentEqualClasses.Count;
        }

        protected void MinimizeStep()
        {
            _prevEqualClasses = _currentEqualClasses;
            _currentEqualClasses = CombineEqualStates( _currentStatesToActions );
            _currentStatesToActions = CreateStatesToActions( _currentEqualClasses );
        }

        protected Dictionary<string, List<Action>> CreateStatesToActions( Dictionary<string, List<string>> equalClasses )
        {
            Dictionary<string, List<Action>> statesToActions = new Dictionary<string, List<Action>>();
            foreach ( KeyValuePair<string, List<string>> equalClass in equalClasses )
            {
                foreach ( string state in equalClass.Value )
                {
                    List<Action> actions;
                    if ( _initialStatesToActions.TryGetValue( state, out actions ) )
                    {
                        foreach ( Action action in actions )
                        {
                            if ( !statesToActions.ContainsKey( state ) )
                            {
                                statesToActions.Add( state, new List<Action>() );
                            }

                            string newOutputSignal = equalClasses.Where( a => a.Value.Contains( action.NewState ) ).Select( a => a.Key ).First();
                            statesToActions[state].Add( new Action( action.InputSignal, newOutputSignal, action.NewState ) );
                        }
                    }
                }
            }

            return statesToActions;
        }

        protected Dictionary<string, List<string>> CombineEqualStates( Dictionary<string, List<Action>> statesToActions )
        {
            Dictionary<string, List<string>> equalClasses = new Dictionary<string, List<string>>();
            foreach ( KeyValuePair<string, List<Action>> stateToActions in statesToActions )
            {
                string actionsHash = Action.GetActionsHashCode( stateToActions.Value ).ToString();
                if ( _prevEqualClasses.Count > 0 )
                {
                    string prevEqualClass = _prevEqualClasses.Where( a => a.Value.Contains( stateToActions.Key ) ).Select( a => a.Key ).First();
                    actionsHash += prevEqualClass;
                }

                if ( !equalClasses.ContainsKey( actionsHash ) )
                {
                    equalClasses.Add( actionsHash, new List<string>() );
                }
                equalClasses[actionsHash].Add( stateToActions.Key );
            }

            return equalClasses;
        }
    }
}
