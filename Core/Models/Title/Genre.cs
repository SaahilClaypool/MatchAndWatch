namespace Core.Models.Title {
  public class Genre {
    public int Id { get; set; }
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string GenreName { get; set; }
  }
}
