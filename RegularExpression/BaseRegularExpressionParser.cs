using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public abstract class BaseRegularExpressionParser : IRegularExpressionParser
    {
        protected const string EndStateLabel = "F"; 
        public abstract void Parse( List<string> lines );
    }
}
