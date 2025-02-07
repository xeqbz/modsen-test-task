using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(e => e.Key, e => e.Value.Errors.Select(er => er.ErrorMessage));

                context.Result = new BadRequestObjectResult(new { Errors = errors });
            }
        }

        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
