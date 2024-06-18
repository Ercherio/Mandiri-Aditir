using MerchantService.Context;
using MerchantService.Handlers;
using MerchantService.Repositories.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MerchantService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // database connection
            var connection_merchant= builder.Configuration.GetConnectionString("database_merchant");
            builder.Services.AddDbContext<MerchantContext>(options => options.UseSqlServer(connection_merchant));


            builder.Services.AddScoped<AuthAssignmentRepository>();
            builder.Services.AddScoped<AuthItemChildRepository>();
            builder.Services.AddScoped<AuthItemRepository>();
            builder.Services.AddScoped<CashierMerchantRepository>();
            builder.Services.AddScoped<LoggingRepository>();
            builder.Services.AddScoped<MerchantRepository>();
            builder.Services.AddScoped<OwnerMerchantRepository>();
            builder.Services.AddScoped<UserRepository>();

            // Register Hashing service
            builder.Services.AddScoped<Hashing>();

            // Configure JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //Usually, this is application base url
                        ValidateAudience = false,
                        //ValidAudience = builder.Configuration["JWT:Audience"],
                        // If the JWT is created using web service, then this could be the consumer URL
                        ValidateIssuer = false,
                        //ValidIssuer = builder.Configuration["JWT:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Project Web Merchant",
                    Description = "ASP.NET Core API 8.0"
                });
            });


             var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
