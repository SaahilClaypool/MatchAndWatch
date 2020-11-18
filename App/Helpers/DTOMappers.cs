using System;

using DTO.Rating;
using DTO.Session;

namespace App.Helpers {
    public static class DTOMappers {
        public static RatingDTO ToDTO(this Core.Models.Rating rating) {
            return new RatingDTO() {
                MovieTitle = rating.Title.Name,
                MovieId = rating.Title.Id,
                UserId = rating.UserId,
                UserName = rating.User.UserName,
                Type = ToDTO(rating.Score)
            };
        }

        public static string ToDTO(this Core.Models.Rating.ScoreType score) =>
            score switch {
                Core.Models.Rating.ScoreType.DOWN => RatingDTO.Downvote,
                Core.Models.Rating.ScoreType.UP => RatingDTO.Upvote,
                Core.Models.Rating.ScoreType.UNDECIDED => RatingDTO.Pass,
                _ => throw new NotImplementedException("Can't map score to dto")
            };

        public static SessionDTO ToDTO(this Core.Models.Session session) =>
            new() {
                Id = session.Id,
                Genres = session.Genres,
                Name = session.Name,
                HostName = session.Creater.UserName
            };
    }
}
