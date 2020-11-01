using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Models;
using Core.Models.Title;

using IdentityServer4.EntityFramework.Options;

using Infrastructure.Models;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data {
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public DbSet<Title> Titles { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder
              .ConfigureSession()
              .ConfigureTitle();
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static void UseDefaultOptions(DbContextOptionsBuilder options, bool useLogger = true) {
            options
                .UseSqlite("DataSource=../app.db;Cache=Shared", b => b.MigrationsAssembly("Infrastructure"));
            if (useLogger) {
                options.UseLoggerFactory(loggerFactory);
                options.EnableSensitiveDataLogging();
            }
        }

        public override int SaveChanges() {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries) {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added) {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
