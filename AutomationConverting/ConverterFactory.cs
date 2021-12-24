using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonMinimization
{
    public static class ConverterFactory
    {
        public static IConverter GetConverter( string minimizerType )
        {
            switch ( minimizerType )
            {
                case ConverterType.Mealy:
                    return CreateMealyConverter();
                case ConverterType.Moore:
                    return CreateMooreConverter();
                default:
                    throw new Exception();
            }
        }

        private static IConverter CreateMealyConverter()
        {
            return new MealyConverter();
        }

        private static IConverter CreateMooreConverter()
        {
            return new MooreConverter();
        }
    }
}
