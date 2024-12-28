using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarisKuafor.Controllers
{
    [Authorize(Roles ="tuna")]
    public class TunaController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
