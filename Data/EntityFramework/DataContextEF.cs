using System.Data;
using DotNetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNetAPI.Data {

    public class DataContextEF : DbContext {
        public virtual DbSet<User> Users { get; set; }    
        public virtual DbSet<UserSalary> UserSalary { get; set; }    
        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }        
        private IConfiguration _config;
        public DataContextEF(IConfiguration config) {
            _config = config; 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.Entity<User>()
                    .ToTable("Users")
                    .HasKey(u => u.UserId);

            modelBuilder.Entity<UserSalary>()
                    .HasKey(u => u.UserId);

            modelBuilder.Entity<UserJobInfo>()
                    .HasKey(u => u.UserId);
        }
    }
}