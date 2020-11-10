using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models {
    public abstract class BaseEntity {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public abstract class Entity : BaseEntity {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
    }
}
