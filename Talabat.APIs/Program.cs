using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Repositories.Data;
using Talabat.Repositories.Repositories;
using Talabat.APIs.Extentions;
using StackExchange.Redis;
using Talabat.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.ServiceContracts;
using Talabat.Core;
using Talabat.Repositories;
using Microsoft.AspNetCore.Http.Features;
namespace Talabat.APIs

{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfile()));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", config =>
                {
                    config.AllowAnyHeader();
                    config.AllowAnyMethod();
                    config.AllowAnyOrigin();
                    //config.WithOrigins(builder.Configuration["FrontendBaseUrl"]);

                });
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // Example limit of 100MB
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<StoreDBContext>(option => {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            builder.Services.AddDbContext<AppIdentityDbcontext>(option => {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });


            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var configurations = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configurations);
            });

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                              {
                                  options.TokenValidationParameters = new TokenValidationParameters()
                                  {
                                      ValidateIssuer = true,
                                      ValidIssuer = builder.Configuration["JWT:validIssuer"],
                                      ValidateAudience = true,
                                      ValidAudience = builder.Configuration["JWT:validAudience"],
                                      ValidateLifetime = true,
                                      ValidateIssuerSigningKey = true,
                                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))


                                  };
                              });




            builder.Services.AddScoped<IToken,TokenServices>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork> ();


            builder.Services.AddApplicationServices();
            builder.Services.AddIdentity<AppUser, IdentityRole>((options) =>
            {

            }).AddEntityFrameworkStores<AppIdentityDbcontext>();

            #endregion









            #region Configurations
            var app = builder.Build();

          using  var scope=app.Services.CreateScope();
          var services=  scope.ServiceProvider;
            var loggerfactor = services.GetRequiredService<ILoggerFactory>();

                var _context= services.GetRequiredService<StoreDBContext>();
            var _IdentityContext = services.GetRequiredService<AppIdentityDbcontext>();
            app.UseMiddleware<ExceptionMiddleware>();
            try
            {

           await _context.Database.MigrateAsync();
               await StoreDBContextSeed.SeedAsync(_context);
                await _IdentityContext.Database.MigrateAsync();//Update-Database Identity
                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbcontextSeed.SeedUserAsync(_userManager);

            }
            catch (Exception ex) 
            {
                var logg = loggerfactor.CreateLogger<Program>();
                logg.LogError(ex, "An error has been occured");
                    }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithRedirects("/error/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
