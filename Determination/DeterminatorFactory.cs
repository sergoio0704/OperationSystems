using System;

namespace Determination
{
    public static class DeterminatorFactory
    {
        public static IDeterminator GetDeterminator( GrammarType grammarType )
        {
            switch ( grammarType ) 
            {
                case GrammarType.Right:
                    return new RightDeterminator();
                case GrammarType.Left:
                    return new LeftDeterminator();
                default:
                    throw new ArgumentException( $"Grammar type: {nameof( grammarType )} is unknown" );
            }
        }
    }
}
