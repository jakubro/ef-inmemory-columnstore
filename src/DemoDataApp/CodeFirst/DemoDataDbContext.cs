using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DemoDataApp.CodeFirst
{
    public class DemoDataDbContext : DbContext
    {
        public DemoDataDbContext()
            : base("name=demoDataDb:codeFirst")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CodeFirstDemoData>()
                .ToTable("DemoDatas");
        }

        public virtual DbSet<CodeFirstDemoData> DemoDatas { get; set; }
    }
}