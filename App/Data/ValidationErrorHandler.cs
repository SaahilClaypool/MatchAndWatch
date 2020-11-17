using Core.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App {
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

    public class NotificationHandler : IActionFilter, IOrderedFilter {
        public int Order { get; } = int.MaxValue - 11;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context) {
            if (context.Exception is INotification notification) {
                context.Result = new ObjectResult(notification.Message) {
                    StatusCode = 400,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
