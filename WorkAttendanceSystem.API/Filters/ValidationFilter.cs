using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkAttendanceSystem.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Items["ModelStateErrors"] = context.ModelState;

                context.Result = new BadRequestResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
