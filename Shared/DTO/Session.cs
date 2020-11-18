using System.Collections.Generic;

using DTO.Rating;

namespace DTO.Session {
    public class CreateSessionCommand {
        public IEnumerable<string> Genres { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class SessionDTO {
        public IEnumerable<string> Genres { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
    }

    public class CreateSessionResponse {
        public string Id { get; set; } = null!;
    }

    public class FullSessionDTO {
        public IEnumerable<string> Genres { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public IEnumerable<RatingDTO> Ratings { get; set; } = null!;
    }
}
