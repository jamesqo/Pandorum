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
    public class Category : IGrouping<string, GenreStation>
    {
        private readonly IEnumerable<GenreStation> _stations;

        internal Category(CategoryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.CategoryName;
            _stations = dto.Stations.Select(s => new GenreStation(s));
        }

        public string Name { get; }

        string IGrouping<string, GenreStation>.Key => Name;
        IEnumerator<GenreStation> IEnumerable<GenreStation>.GetEnumerator() => _stations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _stations.GetEnumerator();

        public override string ToString() => Name;
    }
}
