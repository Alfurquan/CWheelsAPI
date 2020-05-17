using CWheelsAPI.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CWheelsAPI.Filters
{
    public class ValidationModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
