using Newtonsoft.Json;
using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Category
    {
        [JsonConstructor]
        private Category(GenreStation[] stations, string categoryName)
        {
            Debug.Assert(stations != null && categoryName != null);

            Name = categoryName;
            Stations = stations.AsReadOnly();
        }

        public string Name { get; }
        public IEnumerable<GenreStation> Stations { get; }

        public override string ToString() => Name;
    }
}
