using Core.Interfaces;

using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Microsoft.OpenApi.Models;
using Core.UseCases;
using FluentValidation;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace App {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddMemoryCache();
      services.AddMiniProfiler(options => {
        options.RouteBasePath = "/profiler";
        options.EnableDebugMode = true;
      }).AddEntityFramework();
      services.AddControllers();

      services.AddDbContext<ApplicationDbContext>(options => ApplicationDbContext.UseDefaultOptions(options));
      services.AddScoped<ISessionRepository, SessionRepository>();
      services.AddScoped<IGenreRepository, GenreRepository>();
      services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

      services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
          .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddIdentityServer()
          .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

      services.AddAuthentication()
          .AddIdentityServerJwt();

      // https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-3.1
      services.AddControllersWithViews(options => options.Filters.Add(new ValidationErrorHandler()))
      ;
      services.AddRazorPages();

      services.AddMediatR(typeof(Core.UseCases.Session.Create));
      AssemblyScanner.FindValidatorsInAssembly(typeof(Core.UseCases.Session.Create).Assembly)
        .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
      // For all the validators, register them with dependency injection as scoped



      _ = services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo {
          Title = "My API",
          Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          In = ParameterLocation.Header,
          Description = "Please insert JWT with Bearer into field",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
         {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
            },
            System.Array.Empty<string>()
          }
        });
      });



      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration => {
        configuration.RootPath = "ClientApp/build";
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c => {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
      }
      else {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseMiniProfiler();
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "api/{controller}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });

      app.UseSpa(spa => {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment()) {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
