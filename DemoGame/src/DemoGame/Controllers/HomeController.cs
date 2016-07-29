using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoGame.Filters;

namespace DemoGame.Controllers
{
    /// <summary>
    /// Example controller. You can optionally inherit `Controller` as it's not required.
    /// </summary>
    public class HomeController : Controller
    {

        // TODO: Inject IDemoService via constructor injection

        /// <summary>
        /// This MVC action method returns the Index view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// An example of using Filters with dependency injection
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// See: https://docs.asp.net/en/latest/mvc/controllers/filters.html#dependency-injection
        /// </remarks>
        [TypeFilter(typeof(InjectedFilter))]
        [HttpGet("filtered")]
        public string IndexFiltered()
        {
            // this will be replaced with output set by the filter
            return "You won't see this!";
        }

        /// <summary>
        /// All return types are now allowed on a controller. "Web API" methods can live in the same controller but that's not a best practice.
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/test")]
        public string Test()
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
