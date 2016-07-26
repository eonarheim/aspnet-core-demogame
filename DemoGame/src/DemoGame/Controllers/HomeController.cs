using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DemoGame.Controllers
{
    /// <summary>
    /// Example controller. You can optionally inherit `Controller` as it's not required.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// This MVC action method returns the Index view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Web API methods can live in the same controller but that's not a best practice.
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/test")]
        public string GetTest()
        {
            return "I'm a test endpoint";
        }

        /// <summary>
        /// A generic error handler
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
