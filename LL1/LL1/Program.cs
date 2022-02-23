using LL1;

FileStream inputFileStream = File.OpenRead( Path.Combine( Environment.CurrentDirectory, "../../../TableRules.txt" ) );
Reader reader = new Reader( inputFileStream );
Parser parser = new( reader );

string? input;
while ( ( input = Console.ReadLine() ) != null )
{
    bool isMatch = parser.TryParse( input );
    Console.WriteLine( ResolveMessage( input, isMatch ) );
}

static string ResolveMessage( string input, bool isMatch )
{
    if ( isMatch )
    {
        return $"Expression {input} is match";
    }

    return $"Expression {input} is not match";
}
