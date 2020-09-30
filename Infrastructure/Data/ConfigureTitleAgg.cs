using Core.Models.Title;

using Microsoft.EntityFrameworkCore;

namespace Shared {
    namespace Infrastructure.Data {
        public static class ConfigureTitleAggExtensions {
            public static ModelBuilder ConfigureTitleAgg(this ModelBuilder modelBuilder) {
                modelBuilder.Entity<TitleAgg>()
                    .HasKey(title => title.Id);
                modelBuilder.Entity<TitleAgg>()
                    .HasMany<Genre>()
                    .WithOne()
                    .HasForeignKey(genre => genre.TitleId)
                    .OnDelete(DeleteBehavior.Cascade);
                modelBuilder.Entity<Genre>()
                    .HasOne<TitleAgg>()
                    .WithMany()
                    .HasForeignKey(genre => genre.TitleId);

                modelBuilder.Entity<TitleAgg>()
                    .HasOne<Rating>()
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey<Rating>(rating => rating.TitleId);
                modelBuilder.Entity<Rating>()
                    .HasKey(rating => rating.TitleId);

                return modelBuilder;
            }

        }
    }
}
