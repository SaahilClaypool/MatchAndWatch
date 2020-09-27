using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Infrastructure.Models;

using Core.Models;

using IdentityServer4.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.Title;

namespace Infrastructure.Data {
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public DbSet<TitleAgg> TitleAggs { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
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
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(genre => genre.TitleId);
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static void UseDefaultOptions(DbContextOptionsBuilder options, bool useLogger = true) {
            options
                .UseSqlite("DataSource=../app.db;Cache=Shared", b => b.MigrationsAssembly("Infrastructure"));
            if (useLogger) {
                options.UseLoggerFactory(loggerFactory);
            }
        }
    }
}
