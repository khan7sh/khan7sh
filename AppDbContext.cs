using System;
using Microsoft.EntityFrameworkCore;
using BasicVendorInventoryPlatform.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BasicVendorInventoryPlatform.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=vendorinventory.db");
            }
        }


        //On Model creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.UserPermissions)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Website);
                entity.Property(e => e.LastReviewDate);
                entity.Property(e => e.Rating);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.Rating);
                entity.HasOne(p => p.Vendor)
                    .WithMany(v => v.Products)
                    .HasForeignKey(p => p.VendorId);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.FilePath).IsRequired();
                entity.Property(e => e.ContentType);
                entity.Property(e => e.Content);
                entity.HasOne(d => d.Vendor)
                    .WithMany(v => v.Documents)
                    .HasForeignKey(d => d.VendorId);
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.ProductId);
            });
        }


        public async Task EnsureDatabaseCreatedAndMigratedAsync()
        {
            _logger.LogInformation("Ensuring database is created and migrated...");
            await Database.MigrateAsync();
            _logger.LogInformation("Database is up to date.");
            await SeedDataAsync();
        }


        //thise one seeds data
        private async Task SeedDataAsync()
        {
            _logger.LogInformation("Checking if initial data needs to be seeded...");
            if (!await Users.AnyAsync())
            {
                _logger.LogInformation("No users found. Attempting to create initial user...");
                try
                {
                    var user = new User
                    {
                        Username = "user",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user"),
                        UserPermissions = Permissions.ManageUsers
                    };
                    await Users.AddAsync(user);
                    await SaveChangesAsync();
                    _logger.LogInformation($"Initial user created successfully. Username: {user.Username}, Permissions: {user.UserPermissions}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating initial user: {ex.Message}");
                }
            }
            else
            {
                _logger.LogInformation("Users already exist. Skipping seed.");
            }
        }
    }
}