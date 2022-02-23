namespace LL1;

public class Parser
{
    private readonly TableRules _rules;

    private int _currentRuleId;
    private int _currentCharIndex;
    private Stack<int> _rulesStack = new();

    public Parser( Reader reader )
    {
        _rules = reader.Read();

        if ( _rules.Rules.Count == 0 )
        {
            throw new Exception();
        }
        else
        {
            _currentRuleId = _rules.GetFirstRuleId();
        }
    }

    public bool TryParse( string input )
    {
        if ( input is null )
        {
            return false;
        }

        InitializeState();
        Rule currentRule = _rules.GetRuleByid( _currentRuleId );

        while ( true )
        {
            string? currentChar = _currentCharIndex >= 0 ? input[ _currentCharIndex ].ToString() : null;
            bool isCharMatchRule = currentRule.GuideCharacters.Contains( currentChar ) || ( currentRule.IsAcceptNull && currentChar == null );
            if ( isCharMatchRule && currentRule.IsEnd )
            {
                return true;
            }

            if ( !isCharMatchRule && currentRule.IsError )
            {
                return false;
            }

            if ( currentRule.IsNeedStackNextRuleId )
            {
                _rulesStack.Push( ++_currentRuleId );
            }

            if ( currentRule.Shift )
            {
                bool isNotValid = _currentCharIndex == input.Length - 1 || currentChar == null;
                if ( isNotValid )
                {
                    _currentCharIndex = -1;
                }
                else
                {
                    _currentCharIndex++;
                }
            }

            if ( isCharMatchRule && currentRule.IdNextRule == null )
            {
                _currentRuleId = _rulesStack.Pop();
            }

            if ( isCharMatchRule && currentRule.IdNextRule is not null )
            {
                _currentRuleId = currentRule.IdNextRule.Value;
            }

            if ( !isCharMatchRule && !currentRule.IsError )
            {
                _currentRuleId++;
            }

            currentRule = _rules.GetRuleByid( _currentRuleId );
        }
    }

    private void InitializeState()
    {
        _currentRuleId = _rules.GetFirstRuleId();
        _currentCharIndex = 0;
    }
}
