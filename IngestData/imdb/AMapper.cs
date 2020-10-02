namespace IngestData.imdb {
  public abstract class AMapper<From, To> {
    public abstract To Map(From from);
  }
}
