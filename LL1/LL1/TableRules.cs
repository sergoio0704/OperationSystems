namespace LL1;

public class TableRules
{
    private Dictionary<int, Rule> _ruleById;

    public IReadOnlyList<Rule> Rules => _ruleById.Values.ToList();

    public TableRules( IEnumerable<Rule> rules )
    {
        _ruleById = rules.ToDictionary( r => r.Id );
    }

    public Rule GetRuleByid( int id )
    {
        return _ruleById[ id ];
    }

    public int GetFirstRuleId()
    {
        return _ruleById.First().Value.Id;
    }
}
