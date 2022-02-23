namespace LL1;

public class Reader
{
    private const string DELIMETER = " ";
    private const string NULL = "null";

    private Stream _inputStream;

    public Reader( Stream inputStream )
    {
        _inputStream = inputStream;
    }

    public TableRules Read()
    {
        using StreamReader stream = new( _inputStream );

        List<Rule> rules = new List<Rule>();

        string? line = "";
        while ( ( line = stream.ReadLine() ) is not null )
        {
            string[] tokens = line.Split( DELIMETER );

            if ( tokens.Length != 8 )
            {
                throw new FormatException();
            }

            int id = Convert.ToInt32( tokens[ 0 ] );
            IEnumerable<string> guideCharacters = tokens[ 1 ].Select( c => c.ToString() );
            bool shift = Convert.ToBoolean( tokens[ 2 ] );
            bool isError = Convert.ToBoolean( tokens[ 3 ] );
            int? idNextRule = tokens[ 4 ] == NULL ? null : Convert.ToInt32( tokens[ 4 ] );
            bool isNeedStackNextRuleId = Convert.ToBoolean( tokens[ 5 ] );
            bool isEnd = Convert.ToBoolean( tokens[ 6 ] );
            bool isAcceptNull = Convert.ToBoolean( tokens[ 7 ] );

            rules.Add( new Rule( id, guideCharacters, shift, isError, idNextRule, isNeedStackNextRuleId, isEnd, isAcceptNull ) );
        }

        return new TableRules( rules );
    }
}