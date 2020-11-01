using System;
using System.Collections.Generic;

using Core.Models;
using Core.Models.Title;

using Infrastructure.Models;

using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<Genre>()
              .ToTable("Genre")
              .HasIndex(genre => genre.Name);
            return modelBuilder;
        }

        public static ModelBuilder ConfigureSession(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<Session>()
                .HasKey(session => session.Id);
            modelBuilder.Entity<Session>()
              .HasOne(session => (ApplicationUser)session.Creater)
              .WithMany();

            modelBuilder.Entity<Session>()
                .Property(session => session.Genres)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Session>()
              .OwnsMany(session => session.Participants)
                .HasOne(participantStatus => (ApplicationUser)participantStatus.User)
                .WithMany();
            modelBuilder
              .Entity<Session>()
              .Property(e => e.Id)
              .ValueGeneratedOnAdd();

            return modelBuilder;
        }

    }
}
