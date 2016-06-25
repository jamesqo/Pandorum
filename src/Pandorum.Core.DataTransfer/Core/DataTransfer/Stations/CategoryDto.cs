using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public GenreStationDto2[] Stations { get; set; }
    }
}
