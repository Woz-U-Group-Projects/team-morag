using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using server_csharp_sqlite.Models;

namespace server_csharp_sqlite.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UploadController : ControllerBase
  {

    private readonly ProjectDbContext _context;

    public UploadController(ProjectDbContext context)
    {
      _context = context;
    }

    // UploadForm myForm is used to capture form fields 
    public IActionResult Upload([FromForm] UploadForm myForm)
    {

      try
      {
        // get the file off of the request
        var file = Request.Form.Files[0];
        // get the path of the images directory
        var folderName = Path.Combine("wwwroot", "images");
        // combine the image directory with the current directory the application is running from
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        // make sure the file has content
        if (file.Length > 0)
        {
          // get the name of the file form the request 
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          // generate a unique guid (random file name)
          var uniqueFileName = Convert.ToString(Guid.NewGuid());
          var fileExtension = Path.GetExtension(fileName);
          // combine the unique guid with the known file extension
          // ex: 6154b7c7-b3a8-4b8c-af21-f2a069d1a022.jpg
          var newFileName = uniqueFileName + fileExtension;

          // combine the pathToSave with the new file name, this is where the file will be written
          var fullPath = Path.Combine(pathToSave, newFileName);
          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            // save the file
            file.CopyTo(stream);
          }

          // create a new project object
          Project project = new Project();
          // set the name to the input from the form
          project.Name = myForm.Name;
          // set the createdBy property to the filename (storing image location)
          // http://localhost:5000/images/ + filename
          // this address will serve the files out
          // as long as we have the filename stored we can retrieve the image
          project.CreatedBy = newFileName;
          _context.Projects.Add(project);
          _context.SaveChanges();
          return Ok();
        }
        else
        {
          return BadRequest();
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine("ERROR OCCURED");
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
        return Ok();
      }
    }

  }

  public class UploadForm
  {
    public string Name { get; set; }

  }

}
