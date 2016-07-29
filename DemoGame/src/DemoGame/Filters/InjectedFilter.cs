using DemoGame.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGame.Filters
{
    /// <summary>
    /// Example of an injectable filter that takes dependencies via constructor. Use in conjunction with TypeFilterAttribute.
    /// </summary>
    public class InjectedFilter : IActionFilter
    {
        private readonly IDemoService _demoService;

        public InjectedFilter(IDemoService demoService)
        {
            _demoService = demoService;
        }

        /// <summary>
        /// Replace IActionResult with a new ContentResult containing string from service
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.Result = new ContentResult()
            {
                Content = $"Output from IDemoService.Test: {_demoService.Test()}",
                ContentType = "text/plain",
                StatusCode = 200
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
