using System.IO;

namespace AutomatonMinimization
{
    public interface IConverter
    {
        public void Read( StreamReader inputStream );
        public void Convert();
        public void Write( StreamWriter outputStream );
    }
}
