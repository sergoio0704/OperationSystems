using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determination
{
    public interface IDeterminator
    {
        void Determinate( List<string> lines );
        Dictionary<string, List<Transition>> GetResult();
    }
}
