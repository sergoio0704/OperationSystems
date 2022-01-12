using System;

namespace AutomatonMinimization
{
    public static class ConverterFactory
    {
        public static IConverter GetConverter( string minimizerType )
        {
            switch ( minimizerType )
            {
                case ConverterType.Mealy:
                    return CreateMealyMooreConverter();
                case ConverterType.Moore:
                    return CreateMooreMealyConverter();
                default:
                    throw new Exception();
            }
        }

        private static IConverter CreateMealyMooreConverter()
        {
            return new MealyMooreConverter();
        }

        private static IConverter CreateMooreMealyConverter()
        {
            return new MooreMealyConverter();
        }
    }
}
