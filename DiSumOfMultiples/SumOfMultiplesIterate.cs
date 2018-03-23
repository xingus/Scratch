using System.Collections.Generic;

namespace DiSumOfMultiples
{
    public class SumOfMultiplesIterate : ISumOfMultiples
    {

        public SumOfMultiplesIterate() { }

        /// <summary>
        /// Iterate through range, iterate through factors, add where predicate is true.
        /// The naive solution from CS 101.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        public int Sum(int minimum, int maximum, List<int> factors)
        {
            var sum = 0;
            for (var x = minimum; maximum >= x; x++)
            {
                foreach (var f in factors) // TODO use IsMultiple like SumOfMultiplesRecurse
                {
                    if (0 == x % f)
                    {
                        sum += x;
                        break;
                    }
                }
            }
            return sum;
        }
    }
}