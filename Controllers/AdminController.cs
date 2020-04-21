using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Revoow.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {
        }

        public IActionResult List()
        {
            return View();
        }
    }
}