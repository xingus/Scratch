using System.Collections.Generic;

namespace EntityResolution
{
    public class Config
    {
        public string Title { get; set; }
        public string File { get; set; }
        public HashSet<string> StateFilterSet { get; set; }
        public double ProximityMinimum { get; set; }
    }
}
