using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App.Models;

using Core.Models;

using IdentityServer4.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Data {
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
                options.UseLoggerFactory(loggerFactory);

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    }
}
