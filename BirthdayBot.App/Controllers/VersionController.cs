using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayBot.App.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : Controller
{
  [HttpGet]
  public IActionResult GetVersion()
  {
    var assembly = Assembly.GetExecutingAssembly();
    var version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()
      ?.Version
      ?? "Version not found";

    return Ok(version);
  }
}