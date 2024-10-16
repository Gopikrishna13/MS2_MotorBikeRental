using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Service;
using MotorBikeRental.Repository;

namespace MotorBikeRental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()  // Allow any origin
                           .AllowAnyMethod()  // Allow any HTTP method
                           .AllowAnyHeader(); // Allow any header
                });
            });

            // Database connection
            var connectionString = builder.Configuration.GetConnectionString("DbConnection");
            builder.Services.AddSingleton(new MotorBikeRental.Database.DbContext(connectionString));

            // Repository and Service registrations
            builder.Services.AddScoped<IUserRepository>(provider => new UserRepository(connectionString));
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IAdminRepository>(provider => new AdminRepository(connectionString));
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<IBikeRepository>(provider => new BikeRepository(connectionString));
            builder.Services.AddScoped<IBikeService, BikeService>();

              builder.Services.AddScoped<IRentalRepository>(provider => new RentalRepository(connectionString));
              builder.Services.AddScoped<IRentalService, RentalService>();


              builder.Services.AddScoped<IReportRepository>(provider => new ReportRepository(connectionString));
              builder.Services.AddScoped<IReportService, ReportService>();

            var app = builder.Build();

            // Apply the CORS policy
            app.UseCors("AllowAllOrigins");

            // Ensure tables are created in the database
            var database = app.Services.GetService<MotorBikeRental.Database.DbContext>();
            database.CreateTables();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Uncomment to enable HTTPS redirection in production
            // app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
