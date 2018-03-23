using System;
using System.Collections.Generic;

namespace DiSumOfMultiples
{
    public class SumOfMultiplesTriangle : ISumOfMultiples
    {

        public SumOfMultiplesTriangle() { }

        /// <summary>
        /// Do the math with triangular numbers.
        /// Not easily extendable, but handles the example with math you could do on a napkin.
        /// This is probably the solution your high school math teacher hopes you remember.
        /// </summary>
        /// <param name="maximum"></param>
        /// <returns>Sum of multiples of 3 and 5</returns>
        public int Sum(int minimum, int maximum, List<int> factors)
        {
            // Only handles minimum of 1 and two factors.
            if (1 != minimum || 2 != factors.Count) throw new NotImplementedException();
            // Add sum of multiples of each factor, subtract sum of multiples of factors product (they were counted twice).
            return SumTriangular(maximum, factors[0]) + SumTriangular(maximum, factors[1]) - SumTriangular(maximum, factors[0] * factors[1]);
        }

        /// <summary>
        /// Sum the multiples of factor less than maximum.
        /// Use triangular number formula.
        /// </summary>
        /// <param name="maximum"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        private static int SumTriangular(int maximum, int factor)
        {
            var n = maximum / factor;
            var nThTriangular = n * (n + 1) / 2;
            return factor * nThTriangular;
        }
    }
}