using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    //https://localhost:portnumber/api/Students
    [Route("api/[controller]")]
    [ApiController]
    //Both MVC Controller class and the ASP.NET Web API Controller class inherit from the same "Controller" base class and returns IActionResult;
    //IActionResult -> ViewResult (in MVC Controller it returns this) & JsonResult (In API Controller it retuns this)
    public class StudentsController : ControllerBase
    {
        //create action method
        [HttpGet] //using HttpGet Verb. 
        //When we hit the above URL with "GET" method. we will hit this method
        public IActionResult GetAllStudents()
        {
            string[] studentNames = { "John", "Jane", "Mark", "Emily", "David" };
            return Ok(studentNames); //OK means "200" response
        }
    }
}
