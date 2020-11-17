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

    public class CreateRatingDTO {

    }
}
