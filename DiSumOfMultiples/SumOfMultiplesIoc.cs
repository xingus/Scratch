using System;
using System.Collections.Generic;
using System.Linq;

namespace DiSumOfMultiples
{
    public class SumOfMultiplesIoc : ISumOfMultiples
    {

        public SumOfMultiplesIoc() { }

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
            return SumOfEnumerableByPredicate(Enumerable.Range(minimum, maximum), e => factors.Exists(f => 0 == e % f));
        }

        /// <summary>
        /// Inversion of Control LINQ
        /// Overly contrived demonstration.
        /// </summary>
        /// <param name="enumerator">generates range</param>
        /// <param name="predicate">should element be included in sum?</param>
        /// <returns>Sum of multiples</returns>
        public int SumOfEnumerableByPredicate(IEnumerable<int> enumerator, Func<int, bool> predicate)
        {
            var result = enumerator.Where(e => predicate(e)).Sum();
            return result;
        }

    }
}