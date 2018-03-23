using System.Collections.Generic;
using System.Linq;

namespace DiSumOfMultiples
{
    public class SumOfMultiplesLinq : ISumOfMultiples
    {

        public SumOfMultiplesLinq() { }

        /// <summary>
        /// Inline LINQ
        /// Generate the range, select by predicate, sum.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        public int Sum(int minimum, int maximum, List<int> factors)
        {
            var result = Enumerable.Range(minimum, maximum).Where(x => factors.Exists(f => 0 == x % f)).Sum();
            return result;
        }
    }
}