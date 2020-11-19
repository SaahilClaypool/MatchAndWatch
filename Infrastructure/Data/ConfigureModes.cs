using System;
using System.Collections.Generic;
using System.Linq;

using Core.Models;
using Core.Models.Title;

using Infrastructure.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data {
    public static class ConfigureModels {
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
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries),
                    new ValueComparer<IEnumerable<string>>(
                        (l1, l2) => l2.Count() == l1.Count() && l1.All(item => l2.Contains(item)),
                        l => string.Join(",", l.OrderBy(s => s).ToArray()).GetHashCode()
                    ));

            modelBuilder.Entity<Session>()
              .OwnsMany(session => session.Participants)
                .HasOne(participantStatus => (ApplicationUser)participantStatus.User)
                .WithMany();
            modelBuilder
                .Entity<Session>()
                .HasMany(session => session.Ratings)
                .WithOne(rating => rating.Session)
                .HasForeignKey(rating => rating.SessionId);
            modelBuilder
              .Entity<Session>()
              .Property(e => e.Id)
              .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<Rating>()
                .HasOne(rating => (ApplicationUser)rating.User)
                .WithMany();

            modelBuilder.Entity<Session>()
                .OwnsOne(session => session.Invite)
                .WithOwner(invite => invite.Session)
                .HasForeignKey(invite => invite.SessionId);

            return modelBuilder;
        }

    }
}
