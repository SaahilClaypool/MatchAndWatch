using DTO.Session;

namespace Blazor.Helpers {
    public static class DTOExtensions {
        public static string Link(this SessionDTO self) => $"Session/{self.Id}/show";
    }
}
