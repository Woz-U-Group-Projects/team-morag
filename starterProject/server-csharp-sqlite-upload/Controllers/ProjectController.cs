using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using server_csharp_sqlite.Models;

namespace server_csharp_sqlite.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProjectController : ControllerBase
  {

    private readonly ProjectDbContext _context;

    public ProjectController(ProjectDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public List<Project> getProjects()
    {
      return _context.Projects.ToList();
    }

    [HttpPost]
    public Project createProject(Project project)
    {
      _context.Projects.Add(project);
      _context.SaveChanges();
      return project;
    }

  }
}