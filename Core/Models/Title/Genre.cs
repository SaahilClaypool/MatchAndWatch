namespace Core.Models.Title {
    public class Genre : Entity {
        public string TitleId { get; set; }
        public Title Title { get; set; }
        public string Name { get; set; }
    }
}
