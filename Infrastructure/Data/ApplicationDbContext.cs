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

namespace Infrastructure.Data {
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static void UseDefaultOptions(DbContextOptionsBuilder options) =>
            options
                .UseSqlite("DataSource=../app.db;Cache=Shared", b => b.MigrationsAssembly("Infrastructure"))
                .UseLoggerFactory(loggerFactory);
    }
}
