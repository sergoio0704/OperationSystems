using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonMinimization
{
    public interface IMinimizer
    {
        public void Read( StreamReader inputStream );
        public void Minimize();
        public void Write( StreamWriter outputStream );
    }
}
