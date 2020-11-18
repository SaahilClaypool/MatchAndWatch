using System.IO;

using Core.Models;

namespace Core.Services.Session {
    public static class InviteService {
        public static string GenerateToken(this InviteLink link) {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.Substring(0, 8);
        }
    }
}
