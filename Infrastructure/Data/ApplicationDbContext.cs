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

using Shared.Infrastructure.Data;

namespace Infrastructure.Data {
  public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

    public DbSet<TitleAgg> TitleAggs { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ConfigureTitleAgg();
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
