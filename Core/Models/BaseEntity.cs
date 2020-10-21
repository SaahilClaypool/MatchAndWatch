using System;

namespace Core.Models {
  public abstract class BaseEntity {
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
  }
  public abstract class Entity : BaseEntity {
    public string Id { get; set; }
  }
}
