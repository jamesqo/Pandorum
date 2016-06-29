using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class RatingCollection : IReadOnlyCollection<Rating>
    {
        private readonly IEnumerable<Rating> _ratings;

        public RatingCollection(IEnumerable<Rating> ratings, int count)
        {
            if (ratings == null)
                throw new ArgumentNullException(nameof(ratings));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Debug.Assert(count == ratings.Count());

            _ratings = ratings;
            Count = count;
        }

        public int Count { get; }

        public IEnumerator<Rating> GetEnumerator() => _ratings.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
