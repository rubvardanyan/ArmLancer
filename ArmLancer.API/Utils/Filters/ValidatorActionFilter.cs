using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArmLancer.API.Utils.Filters
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState.Values.SelectMany(
                    entry => entry.Errors.Select(error => error.ErrorMessage)).ToList());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
    }
}