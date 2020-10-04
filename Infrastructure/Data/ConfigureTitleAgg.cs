using Core.Models.Title;

using Microsoft.EntityFrameworkCore;

namespace Shared {
  namespace Infrastructure.Data {
    public static class ConfigureTitleExtensions {
      public static ModelBuilder ConfigureTitle(this ModelBuilder modelBuilder) {
        modelBuilder.Entity<Title>()
            .HasKey(title => title.Id);
        modelBuilder.Entity<Title>()
            .HasMany(title => title.Genres)
            .WithOne(genre => genre.Title)
            .HasForeignKey(genre => genre.TitleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        return modelBuilder;
      }

    }
  }
}
