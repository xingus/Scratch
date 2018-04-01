using System;

namespace EntityResolution
{
    struct LinkKey : IEquatable<LinkKey>
    {
        readonly int key1;
        readonly int key2;

        public LinkKey(int n1, int n2)
        {
            key1 = n1 < n2 ? n1 : n2;
            key2 = n1 < n2 ? n2 : n1;
        }

        public int Key1 { get { return key1; } }
        public int Key2 { get { return key2; } }

        public override int GetHashCode()
        {
            return key1.GetHashCode() ^ key2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((LinkKey)obj);
        }

        public bool Equals(LinkKey other)
        {
            return other.key1 == key1 && other.key2 == key2;
        }

        public override string ToString()
        {
            return $"{key1} : {key2}";
        }
    }
}
