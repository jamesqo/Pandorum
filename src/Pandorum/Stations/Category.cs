using Newtonsoft.Json;
using Pandorum.Core;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Category
    {
        internal Category(CategoryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.CategoryName;
            Stations = dto.Stations.Select(s => new GenreStation(s));
        }

        public string Name { get; }
        public IEnumerable<GenreStation> Stations { get; }

        public override string ToString() => Name;
    }
}
