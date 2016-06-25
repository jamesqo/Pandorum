using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class FeedbackDto
    {
        public RatingDto[] ThumbsUp { get; set; }
        public int TotalThumbsUp { get; set; }
        public int TotalThumbsDown { get; set; }
        public RatingDto[] ThumbsDown { get; set; }
    }
}
