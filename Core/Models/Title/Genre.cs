namespace Core.Models.Title {
  public class Genre {
    public int Id { get; set; }
    public string TitleId { get; set; }
    public TitleAgg TitleAgg { get; set; }
    public string GenreName { get; set; }
  }
}
