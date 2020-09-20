using Core.Interfaces;

namespace Core.Models {
    public class Rating {
        public Movie Movie { get; set; }

        /// TODO: figure out if we should
        /// store a key here as well
        public IUser User { get; set; }

        public Score Score { get; set; }
    }

    public enum Score {
        DOWNVOTE = 0,
        UPVOTE = 1,
    }
}
