using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    // TODO: Implement IComparable/IEquatable?
    public struct SearchResult
    {
        internal SearchResult(string name, ICreatableSeed seed, int score)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));
            
            Name = name;
            Seed = seed;
            Score = score;
        }

        public int Score { get; }
        public string Name { get; }
        public ICreatableSeed Seed { get; }
    }
}
