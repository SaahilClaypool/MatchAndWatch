using System;
using System.Collections.Generic;

using Core.Interfaces;
namespace Core.Models {
    public class Session {
        public IUser Host { get; set; }
        public IEnumerable<SessionUser> Participants { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
    }
}
