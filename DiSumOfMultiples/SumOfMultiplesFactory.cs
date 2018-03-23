using System;
using System.Collections.Generic;
using System.Linq;

namespace DiSumOfMultiples
{

    public interface ISumOfMultiplesFactory
    {
        ISumOfMultiples GetSumOfMultiples(String sumOfMultiplesMethod); // TODO maybe change to enum
    }

    class SumOfMultiplesFactory : ISumOfMultiplesFactory
    {
        private readonly IEnumerable<ISumOfMultiples> _candidates;

        public SumOfMultiplesFactory(
            List<ISumOfMultiples> candidates)
        {
            _candidates = candidates;
        }

        public ISumOfMultiples GetSumOfMultiples(String sumOfMultiplesMethod)
        {
            return (from c in _candidates
                    let t = c.GetType()
                    where t.Name.EndsWith(sumOfMultiplesMethod)
                    select c).First();
        }
    }
}