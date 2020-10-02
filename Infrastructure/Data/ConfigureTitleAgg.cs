using Core.Models.Title;

using Microsoft.EntityFrameworkCore;

namespace Shared {
  namespace Infrastructure.Data {
    public static class ConfigureTitleExtensions {
      public static ModelBuilder ConfigureTitle(this ModelBuilder modelBuilder) {
        modelBuilder.Entity<Title>()
            .HasKey(title => title.Id);
        modelBuilder.Entity<Title>()
            .HasMany<Genre>()
            .WithOne()
            .HasForeignKey(genre => genre.TitleId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Genre>()
            .HasOne<Title>()
            .WithMany()
            .HasForeignKey(genre => genre.TitleId);
        return modelBuilder;
      }

    }
  }
}
