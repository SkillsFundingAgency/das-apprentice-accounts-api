using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Api
{
    public static class ProblemDetailsExtensions
    {
        public static ValidationProblemDetails ToProblemDetails(this ValidationException ex)
            => new ValidationProblemDetails(
                ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest
            };

        public static ProblemDetails ToProblemDetails(this DomainException ex)
            => new ProblemDetails { Detail = ex.Message, Status = StatusCodes.Status400BadRequest };

        public static ProblemDetails ToProblemDetails(this EntityNotFoundException ex)
            => new ProblemDetails { Detail = ex.Message, Status = StatusCodes.Status404NotFound };

        public static ProblemDetails ToProblemDetails(this InvalidInputException ex)
            => new ProblemDetails { Detail = ex.Message, Status = StatusCodes.Status400BadRequest };
    }
}