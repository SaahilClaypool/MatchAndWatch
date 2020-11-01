using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationErrorHandler : IActionFilter, IOrderedFilter {
    public int Order { get; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context) {
        if (context.Exception is FluentValidation.ValidationException exception) {
            context.Result = new ObjectResult(exception.Errors) {
                StatusCode = 400,
            };
            context.ExceptionHandled = true;
        }
    }
}
