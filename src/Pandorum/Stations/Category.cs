using Newtonsoft.Json;
using Pandorum.Core;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Category : IGrouping<string, Genre>
    {
        private readonly IEnumerable<Genre> _genres;

        internal Category(CategoryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.CategoryName;
            _genres = dto.Stations.Select(g => new Genre(g));
        }

        public string Name { get; }

        public IEnumerator<Genre> GetEnumerator() => _genres.GetEnumerator();
        public override string ToString() => Name;

        string IGrouping<string, Genre>.Key => Name;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
