using System;
using System.Collections.Generic;
using System.Linq;

// 
// From https://projecteuler.net/problem=1
//
// Multiples of 3 and 5
// 
// Problem 1 
//
// If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
// 
// Find the sum of all the natural number multiples of 3 or 5 below 1000.
//
// Here are five solutions in C#...
//

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"1. Triangle = {SumOfMultiplesTriangle(999)}");
            Console.WriteLine($"2. Iterate = {SumOfMultiplesIterate(1, 999, new List<int> { 3, 5 })}");
            Console.WriteLine($"3. Recurse = {SumOfMultiplesRecurse(1, 999, new List<int> { 3, 5 })}");
            Console.WriteLine($"4. LINQ = {SumOfMultiplesLinq(1, 999, new List<int> { 3, 5 })}");
            Console.WriteLine($@"5. EnumerableByPredicate = {
                SumOfEnumerableByPredicate( Enumerable.Range(1, 999)
                                          , e => new List<int> { 3, 5 }.Exists(f => 0 == e % f))}");
            Console.WriteLine($@"5b. Change predicate = {
                SumOfEnumerableByPredicate( Enumerable.Range(1, 999)
                                          , e => new List<int> { 1, 3, 5, 7 }.Exists(f => f == e % 10))}"); // change predicate: last digit is 1, 3, 5, or 7 
            Console.ReadLine();
        }

        /// <summary>
        /// Do the math with triangular numbers.
        /// Not easily extendable, but handles the example with math you could do on a napkin.
        /// This is probably the solution your high school math teacher hopes you remember.
        /// </summary>
        /// <param name="maximum"></param>
        /// <returns>Sum of multiples of 3 and 5</returns>
        private static int SumOfMultiplesTriangle(int maximum)
        {
            // Add sum of multiples of 3, then 5, subtract multiples of 15 (they were counted twice).
            return SumTriangular(maximum, 3) + SumTriangular(maximum, 5) - SumTriangular(maximum, 15);
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

        /// <summary>
        /// Iterate through range, iterate through factors, add where predicate is true.
        /// The naive solution from CS 101.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        private static int SumOfMultiplesIterate(int minimum, int maximum, List<int> factors)
        {
            var sum = 0;
            for (var x = minimum; maximum >= x; x++)
            {
                foreach (var f in factors) // TODO use IsMultiple
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

        /// <summary>
        /// Recurse through range, add where IsMultiple true.
        /// Is recursion still a thing?
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        private static int SumOfMultiplesRecurse(int minimum, int maximum, List<int> factors)
        {
            if (minimum > maximum) return 0;
            return ( IsMultiple( maximum, factors) ? maximum : 0 )
                 + SumOfMultiplesRecurse(minimum, maximum-1, factors);
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

        /// <summary>
        /// Inline LINQ
        /// Generate the range, select by predicate, sum.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="factors"></param>
        /// <returns>Sum of multiples</returns>
        private static int SumOfMultiplesLinq(int minimum, int maximum, List<int> factors)
        {
            var result = Enumerable.Range(minimum, maximum).Where(x => factors.Exists(f => 0 == x % f)).Sum();
            return result;
        }

        /// <summary>
        /// Inversion of Control LINQ
        /// Overkill demonstration.
        /// </summary>
        /// <param name="enumerator">generates range</param>
        /// <param name="predicate">should element be included in sum?</param>
        /// <returns>Sum of multiples</returns>
        private static int SumOfEnumerableByPredicate(IEnumerable<int> enumerator, Func<int, bool> predicate)
        {
            var result = enumerator.Where(e => predicate(e)).Sum();
            return result;
        }
    }
}
