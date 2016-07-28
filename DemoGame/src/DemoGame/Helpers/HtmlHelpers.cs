using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace DemoGame.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString ToJson(this IHtmlHelper helper, object o)
        {
            // MvcJsonOptions are injectable through IOptions support
            var jsonOptions = helper.ViewContext.HttpContext.RequestServices.GetService<IOptions<MvcJsonOptions>>();

            if (jsonOptions == null)
            {
                throw new ArgumentNullException(nameof(jsonOptions), "Could not find MvcJsonOptions");
            }

            return new HtmlString(JsonConvert.SerializeObject(o, jsonOptions.Value.SerializerSettings));
        }
    }
}
