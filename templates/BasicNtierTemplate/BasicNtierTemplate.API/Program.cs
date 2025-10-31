using System.Diagnostics;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Mappings;
using BasicNtierTemplate.Service.Services;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				Debug.WriteLine(">>> API: Starting application...");

				// Create a WebApplication builder to configure services and middleware
				var builder = WebApplication.CreateBuilder(args);

				#region Service addition

				// Connection string "BasicNtierTemplateConnection" is pulled from configuration (appsettings.json).
				var connectionString = builder.Configuration.GetConnectionString("BasicNtierTemplateConnection")
				?? throw new InvalidOperationException("Connection string" + "'BasicNtierTemplateConnection' not found.");

				// Register DbContext with SQL Server as the database provider.
				builder.Services.AddDbContext<BasicNtierTemplateDbContext>(options =>
					options.UseSqlServer(connectionString));

				// TODO https://youtu.be/kC9qrUcy2Js?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&t=199
				builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
				{
					options.Password.RequireDigit = true;
					options.Password.RequireUppercase = true;
					options.Password.RequireLowercase = true;
					options.Password.RequiredLength = 6;
					options.SignIn.RequireConfirmedEmail = false;
					//options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
					// TODO https://youtu.be/jHRWR36UC2s?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU
					options.Lockout.MaxFailedAccessAttempts = 5;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
				})
					.AddEntityFrameworkStores<BasicNtierTemplateDbContext>()
					.AddDefaultTokenProviders();

				// CORS Configuration to allow requests from frontend applications (for production, configure appropriately).
				/* Not required when running frontend and backend in the same solution with proper proxy setup
				builder.Services.AddCors(options =>
				{
					options.AddPolicy("AllowFrontend", policy =>
					{
						policy.WithOrigins("http://localhost:3000", "http://localhost:4200") // Your frontend URLs
							  .AllowAnyHeader()
							  .AllowAnyMethod()
							  .AllowCredentials(); // Important for cookies/auth
					});
				});
				*/

				builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

				builder.Services.AddAutoMapper(
					cfg => { },
					typeof(SampleProfile).Assembly
				);

				// Register application services for dependency injection.
				builder.Services.AddScoped<ISampleService, SampleService>(); // SAMPLE
																			 // Identity Services
				builder.Services.AddScoped<IEmailService, EmailService>();
				builder.Services.AddScoped<IUserService, UserService>();
				builder.Services.AddScoped<IRegistrationService, RegistrationService>();

				// Add controller support (enables MVC-style controllers).
				builder.Services.AddControllers().AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNamingPolicy = null;
				});

				// Register services required for minimal APIs (endpoint metadata exploration).
				builder.Services.AddEndpointsApiExplorer();

				// Register Swagger generator for API documentation.
				builder.Services.AddSwaggerGen();

				#endregion

				// Build the application with the configured services.
				var app = builder.Build();

				#region Middleware addition

				// Configure the HTTP request pipeline (middleware execution order).
				if (app.Environment.IsDevelopment())
				{
					// Show detailed error pages during development.
					// app.UseDeveloperExceptionPage(); // Not required after NET6

					// Enable Swagger UI in development for API testing and documentation.
					app.UseSwagger();
					app.UseSwaggerUI();
				}
				else
				{
					app.UseExceptionHandler("/error");
					app.UseStatusCodePagesWithReExecute("/error/{0}");
				}

				// Redirect all HTTP requests to HTTPS for security.
				app.UseHttpsRedirection();

				// Enable CORS using the defined policy to allow frontend access.
				// Not required when running frontend and backend in the same solution with proper proxy setup
				// app.UseCors("AllowFrontend");

				// Enable authorization middleware (validates user access).
				app.UseAuthentication();
				app.UseAuthorization();

				// Map controllers to endpoints (routes controllers’ actions to HTTP requests).
				app.MapControllers();

				app.MapFallbackToFile("/index.html");

				#endregion

				// Run the application and start listening for incoming HTTP requests.
				app.Run();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex, "API: Stopped program because of exception");
				throw;
			}
		}
	}
}
