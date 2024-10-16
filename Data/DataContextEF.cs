using Microsoft.EntityFrameworkCore;
using DotNetApi.Models;

namespace DotNetApi.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _congif;

        public DataContextEF(IConfiguration config)
        {
            _congif = config;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSalary> UsersSalary { get; set; }
        public virtual DbSet<UserJobInfo> UsersJobInfo { get; set; }

        protected override void onConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_congif.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.HasDefaultSchema("TutorialAppSchema");
            modelbuilder.Entity<User>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(u => u.UserId)
        
            modelbuilder.Entity<UserJobInfo>()
                .HasKey(u => u.UserId)  

            modelbuilder.Entity<UserSalary>()
                .HasKey(u => u.UserId)      
        }

    }
}