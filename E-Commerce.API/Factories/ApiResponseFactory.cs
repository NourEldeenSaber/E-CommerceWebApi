using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState
                                    .Where(e => e.Value?.Errors.Any() == true)
                                    .Select(e => new ValidationError()
                                    {
                                        Field = e.Key,
                                        Errors = e.Value?.Errors.Select(error => error.ErrorMessage) ?? new List<string>()
                                    });
            var response = new ValidationErrorResponse()
            {
                Errors = errors,
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One Or More Validation Error Happened"
            };
            return new BadRequestObjectResult(response);
        } 
    }
}
