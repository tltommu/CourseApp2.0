using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Auth.AspNetCore3;

namespace CourseApp2._0.Controllers
{
    public class GoogleLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
