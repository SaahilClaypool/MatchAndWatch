using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core;
using Core.Interfaces;
using Core.Models.Title;

using Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Services.Session {
    class SessionManager {
        public ISessionRepository SessionRepository { get; }
        private ITitleRepository TitleRepository { get; }
        private Models.Session Session { get; init; }
        private string CurrentUserId { get; init; }
        public ILogger Logger { get; }

        public SessionManager(ISessionRepository sessionRepository, ITitleRepository TitleRepository, Models.Session session, string currentUserId, ILogger logger) {
            SessionRepository = sessionRepository;
            this.TitleRepository = TitleRepository;
            Session = session;
            CurrentUserId = currentUserId;
            Logger = logger;
        }

        public async Task<Title> NextTitle() {
            Logger.LogDebug($"Trying to get next rating title for {CurrentUserId} and session {Session.Name}");
            var viableTitles = TitleRepository.TitleGenres()
                .Where(genre => Session.Genres.Contains(genre.Name))
                .Select(genre => genre.Title);

            var ratings = SessionRepository.Ratings().Where(rating => rating.SessionId == Session.Id);

            var ratedByCurrentUser = ratings
                .Where(rating => rating.UserId == CurrentUserId)
                .Select(rating => rating.TitleId);

            var ratedByOthers = ratings
                .Where(rating => !ratedByCurrentUser.Contains(rating.TitleId))
                .Select(rating => rating.Title);

            var notRatedByCurrentUser = viableTitles
                .Where(title => !ratedByCurrentUser.ToList().Contains(title.Id!));


            return await ratedByOthers.Concat(notRatedByCurrentUser).FirstOrDefaultAsync();

        }
    }
}
