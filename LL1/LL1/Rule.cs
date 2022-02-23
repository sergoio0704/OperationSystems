namespace LL1;

public class Rule
{
    public Rule( int id, IEnumerable<string> guideCharacters, bool shift, bool isError, int? idNextRule, bool isNeedStackNextRuleId, bool isEnd, bool isAcceptNull )
    {
        Id = id;
        GuideCharacters = guideCharacters.ToList();
        Shift = shift;
        IsError = isError;
        IdNextRule = idNextRule;
        IsNeedStackNextRuleId = isNeedStackNextRuleId;
        IsEnd = isEnd;
        IsAcceptNull = isAcceptNull;
    }

    public int Id { get; init; }
    public List<string> GuideCharacters { get; init; }
    public bool Shift { get; init; }
    public bool IsError { get; init; }
    public int? IdNextRule { get; init; }
    public bool IsNeedStackNextRuleId { get; init; }
    public bool IsEnd { get; init; }
    public bool IsAcceptNull { get; init; }
}
