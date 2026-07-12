using Microsoft.EntityFrameworkCore;

namespace CV_Builder.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data matching your CV
            modelBuilder.Entity<PersonalInfo>().HasData(new PersonalInfo { Id = 1 });
            // You can add seed data for Projects/Education too, or just add via UI later.
        }
    }
}