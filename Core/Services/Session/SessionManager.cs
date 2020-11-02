using Core;
using Core.Interfaces;
using Core.Models.Title;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Session {
    class SessionManager {
        public ITitleRepository TitleRepository { get; }
        private Models.Session Session { get; init; }
        private string CurrentUserId { get; init }

        public SessionManager(ITitleRepository TitleRepository, Models.Session session, string currentUserId) {
            this.TitleRepository = TitleRepository;
            Session = session;
            CurrentUserId = currentUserId;
        }

        public IAsyncEnumerable<Title> NextTitles() {
            var viableTitles = TitleRepository.TitleGenres()
                .Where(genre => Session.Genres.Contains(genre.Name))
                .Select(genre => genre.Title);

            var ratedByCurrentUser = Session.Ratings
                .Where(rating => rating.UserId == CurrentUserId)
                .Select(rating => rating.TitleId);

            var ratedByOthers = Session.Ratings
                .Where(rating => !ratedByCurrentUser.Contains(rating.TitleId))
                .Select(rating => rating.Title);

            var notRatedByCurrentUser = viableTitles
                .Where(title => !ratedByCurrentUser.Contains(title.Id));

            return ratedByOthers
                        .Concat(notRatedByCurrentUser)
                        .AsAsyncEnumerable();
        }
    }
}
