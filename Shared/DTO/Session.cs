using System.Collections.Generic;

using DTO.Rating;

namespace DTO.Session {
    public class CreateSessionCommand {
        public IEnumerable<string> Genres { get; set; }
        public string Name { get; set; }
    }

    public class SessionDTO {
        public IEnumerable<string> Genres { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class CreateSessionResponse {
        public string Id { get; set; }
    }

    public class FullSessionDTO {
        public IEnumerable<string> Genres { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public IEnumerable<RatingDTO> Ratings { get; set; }
    }
}
