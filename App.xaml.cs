using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Services;
using BasicVendorInventoryPlatform.Views;
using Microsoft.Extensions.Logging;
using System.IO;
using BasicVendorInventoryPlatform.Models;
using System.Threading.Tasks;

namespace BasicVendorInventoryPlatform
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();

                try
                {
                    logger.LogInformation("Starting database initialization...");
                    await dbContext.Database.EnsureCreatedAsync();
                    await dbContext.Database.MigrateAsync();
                    await SeedDataAsync(dbContext, logger);
                    logger.LogInformation("Database initialization completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while initializing the database.");
                    MessageBox.Show($"Failed to initialize the database. Error: {ex.Message}\n\nPlease check the logs for more information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=vendorinventory.db");
            });

            services.AddLogging(configure => configure.AddConsole());

            // Register your services here
            services.AddSingleton<LoggingService>();
            services.AddTransient<AuthService>();
            services.AddTransient<SearchService>();
            services.AddTransient<VendorService>();
            services.AddTransient<ProductService>();
            services.AddTransient<DocumentService>();
            services.AddTransient<ReportService>();
            services.AddTransient<PdfService>();

            // Register your main window and views
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<VendorPage>();
            services.AddTransient<ProductPage>();
            services.AddTransient<SearchPage>();
            services.AddSingleton<PdfService>();
        }

        private async Task SeedDataAsync(AppDbContext context, ILogger logger)
        {
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("Seeding initial user data...");
                var adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    UserPermissions = Permissions.ManageUsers | Permissions.ManageVendors | Permissions.ManageProducts
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
                logger.LogInformation("Initial user data seeded successfully.");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _serviceProvider?.Dispose();
        }
    }
}