using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using gitMARATHON.Models;
using Microsoft.AspNetCore.Identity;

namespace gitMARATHON.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<PetProject> PetProjects { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Score>()
                .HasOne(s => s.Lecture)
                .WithMany()
                .HasForeignKey(s => s.LectureId);
        }

        public DbSet<gitMARATHON.Models.Task> Tasks { get; set; }

        public DbSet<MessageModel> Messages { get; set; }

    }
}