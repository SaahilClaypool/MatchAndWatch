using System.Collections.Generic;

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
}
