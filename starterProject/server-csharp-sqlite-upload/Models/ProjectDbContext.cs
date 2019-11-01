using Microsoft.EntityFrameworkCore;

namespace server_csharp_sqlite.Models
{
  public class ProjectDbContext : DbContext {
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options){}
    
    public DbSet<Project> Projects {get;set;}

  }
}