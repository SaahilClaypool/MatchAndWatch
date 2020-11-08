using System.Collections.Generic;

namespace DTO.Session {
    public class CreateSessionCommand {
        public IEnumerable<string> Genres { get; set; }
        public string Name { get; set; }
    }
}
