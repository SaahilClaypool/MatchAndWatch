namespace DTO.Rating {
    public class MovieInformationQueryDTO {
        public string SessionId { get; set; }
    }

    public class MovieInformationResponseDTO {
        public string MovieTitle { get; set; }
        public string MovieId { get; set; }
        public string MovieSummary { get; set; }
        public string PosterPartialPath { get; set; }
    }

    public class RatingDTO {
        public const string Upvote = "Upvote";
        public const string Downvote = "Downvote";
        public const string Pass = "Pass";
        public string Type { get; set; }
        public string MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class CreateRatingResponseDTO {
        
    }
}
