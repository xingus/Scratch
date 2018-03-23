using System.Collections.Generic;

namespace DiSumOfMultiples
{
    public interface ISumOfMultiples
    {
        int Sum(int minimum, int maximum, List<int> factors);
    }
}