using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpression
{
    public interface IRegularExpressionParser
    {
        void Parse( List<string> lines );
    }
}
