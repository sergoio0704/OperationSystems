using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public enum GrammarType
    {
        Left = 0,
        Right = 1
    }

    public static class EnumExtenstions
    {
        public static T ToEnum<T>( this string value, bool ignoreCase = true )
        {
            return ( T ) Enum.Parse( typeof( T ), value, ignoreCase );
        }
    }
}
