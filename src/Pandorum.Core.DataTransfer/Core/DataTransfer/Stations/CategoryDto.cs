using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    internal class CategoryDto
    {
        public string CategoryName { get; set; }
        public GenreStationDto[] Stations { get; set; }
    }
}
