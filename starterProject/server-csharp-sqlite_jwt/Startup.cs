using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using server_csharp_sqlite.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace server_csharp_sqlite
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      // Cors policy, this allows a front end to communicate with our API
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
      });

      // This is the database connection for Project data
      services.AddDbContext<ProjectDbContext>(options => options.UseSqlite("Data Source=project.db"));
      // This is the database connection for Identity data
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=project.db"));

      // Identity configuration, types of data being used
      services.AddIdentity<IdentityUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      // JWT token setup
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      services
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

          })
          .AddJwtBearer(cfg =>
          {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
              ValidIssuer = Configuration["JwtIssuer"],
              ValidAudience = Configuration["JwtIssuer"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
              ClockSkew = TimeSpan.Zero
            };
          });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }
      // add cors policy
      app.UseCors("CorsPolicy");
      // use authentication
      app.UseAuthentication();
      app.UseMvc();

    }
  }
}
