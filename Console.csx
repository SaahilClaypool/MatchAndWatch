#! "net5.0"
#r "nuget: Microsoft.AspNetCore.ApiAuthorization.IdentityServer, 5.0.0-preview.8.20414.8"
#r "nuget: Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore, 5.0.0-preview.8.20414.8"
#r "nuget: Microsoft.AspNetCore.Identity.EntityFrameworkCore, 5.0.0-preview.8.20414.8"
#r "nuget: Microsoft.AspNetCore.Identity.UI, 5.0.0-preview.8.20414.8"
#r "nuget: Microsoft.EntityFrameworkCore.Relational, 5.0.0-preview.8.20407.4"
#r "nuget: Microsoft.EntityFrameworkCore.Sqlite, 5.0.0-preview.8.20407.4"
#r "nuget: Microsoft.EntityFrameworkCore.Tools, 5.0.0-preview.8.20407.4"
#r "Infrastructure/bin/Debug/net5.0/Shared.dll"
#r "Infrastructure/bin/Debug/net5.0/Core.dll"
#r "Infrastructure/bin/Debug/net5.0/Infrastructure.dll"

using Infrastructure;

var factory = DbContextFactory.CreateDbContextWithOptions(new FactoryOptions());