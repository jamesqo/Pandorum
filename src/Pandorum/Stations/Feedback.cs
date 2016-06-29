using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pandorum.Core.DataTransfer.Stations;

namespace Pandorum.Stations
{
    public class Feedback
    {
        internal Feedback(FeedbackDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var thumbsUp = dto.ThumbsUp.Select(r => new Rating(r));
            ThumbsUp = new RatingCollection(thumbsUp, dto.TotalThumbsUp);

            var thumbsDown = dto.ThumbsDown.Select(r => new Rating(r));
            ThumbsDown = new RatingCollection(thumbsDown, dto.TotalThumbsDown);
        }

        public RatingCollection ThumbsUp { get; }
        public RatingCollection ThumbsDown { get; }
    }
}
