using System;
using System.Collections.Generic;

namespace DiSumOfMultiples
{
    public class SumOfMultiplesRecurse : ISumOfMultiples
    {

        public SumOfMultiplesRecurse() { }

        /// <summary>
        /// Recurse through range, add where IsMultiple true.
        /// Is recursion still a thing?
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        public int Sum(int minimum, int maximum, List<int> factors)
        {
            if (minimum > maximum) return 0;
            return (IsMultiple(maximum, factors) ? maximum : 0)
                 + Sum(minimum, maximum - 1, factors);
        }

        /// <summary>
        /// Is X a multiple of one or more factors?
        /// Helper function for iterative and recursive methods.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        private static bool IsMultiple(int x, List<int> factors)
        {
            foreach (var f in factors)
            {
                if (0 == x % f) return true;
            }
            return false;
        }
    }
}