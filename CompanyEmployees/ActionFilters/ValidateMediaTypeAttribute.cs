using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateMediaTypeAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var AcceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");

            if (!AcceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept Heaader is missing.");
                return;
            }

            var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue outMediaType))
            {
                context.Result = new BadRequestObjectResult("Media type not present, Please add Accept header with the required media type.");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
