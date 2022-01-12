using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public static class RegularExpressionParserFactory
    {
        public static IRegularExpressionParser GetRegularExpressionParser( GrammarType grammarType )
        {
            switch ( grammarType ) 
            {
                case GrammarType.Right:
                    return new RightRegularExpressionParser();
                default:
                    throw new ArgumentException( $"Grammar type: {nameof( grammarType )} is unknown" );
            }
        }
    }
}
