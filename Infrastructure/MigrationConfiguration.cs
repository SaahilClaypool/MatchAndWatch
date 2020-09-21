using IdentityServer4.EntityFramework.Options;

using Infrastructure.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure {
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> {
        public ApplicationDbContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ApplicationDbContext.UseDefaultOptions(optionsBuilder);
            var storeOptions = new OperationalStoreOptions();
            IOptions<OperationalStoreOptions> optionParameter = Options.Create(storeOptions);
            return new ApplicationDbContext(optionsBuilder.Options, optionParameter);
        }
    }
}
