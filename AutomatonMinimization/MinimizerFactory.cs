using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonMinimization
{
    public static class MinimizerFactory
    {
        public static IMinimizer GetMinimizer( string minimizerType )
        {
            switch ( minimizerType )
            {
                case MinimizerType.Mealy:
                    return CreateMealyMinimizer();
                case MinimizerType.Moore:
                    return CreateMooreMinimizer();
                default:
                    throw new Exception();
            }
        }

        private static IMinimizer CreateMealyMinimizer()
        {
            return new MealyMinimizer();
        }

        private static IMinimizer CreateMooreMinimizer()
        {
            return new MooreMinimizer();
        }
    }
}
