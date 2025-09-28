using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Sondor.Errors;
using Sondor.ProblemResults.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Sondor.ProblemResults.Extensions;

/// <summary>
/// Collection of <see cref="HttpContext"/> extensions.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the username.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the username.</returns>
    public static string GetUsername(this HttpContext? context)
    {
        return context?.User.FindFirst(ClaimTypes.Name)?.Value ?? "anonymous";
    }

    /// <summary>
    /// Gets the instance, using the provided <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the instance.</returns>
    public static string GetInstance(this HttpContext context)
    {
        return $"{context.Request.Protocol} {context.Request.Method} {context.Request.Path}";
    }

    /// <summary>
    /// The unexpected problem.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails UnexpectedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemResultConstants.UnexpectedErrorType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                { ProblemResultConstants.ErrorMessage, errorMessage }
            }
        };
    }

    /// <summary>
    /// Unsupported error code problem.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails UnsupportedErrorCodeProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemResultConstants.UnexpectedErrorType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                { ProblemResultConstants.ErrorMessage, errorMessage }
            }
        };
    }

    /// <summary>
    /// Resource not found error problem.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="context">The HTTP context.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourceNotFoundProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource,
        string propertyName,
        string propertyValue)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status404NotFound,
            Type = ProblemResultConstants.ResourceNotFoundType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceNotFound },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.PropertyName, propertyName },
                { ProblemResultConstants.PropertyValue, propertyValue }
            }
        };
    }

    /// <summary>
    /// Validation error problem.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="context">The HTTP context.</param>
    /// <param name="failures">The validation failures.</param>
    /// <returns>Returns the validation problem.</returns>
    public static SondorProblemDetails ValidationProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        IEnumerable<ValidationFailure> failures)
    {
        var errors = failures.GroupBy(prop => prop.PropertyName, (key, failure) => new ValidationError(
                key,
                failure
            ))
            .ToList();

        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status400BadRequest,
            Type = ProblemResultConstants.BadRequestType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Errors, errors }
            }
        };
    }

    /// <summary>
    /// Cancelled problem.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the cancelled problem.</returns>
    public static SondorProblemDetails CancelledProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = 499,
            Type = ProblemResultConstants.RequestCancelledType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.TaskCancelled },
                { ProblemResultConstants.ErrorMessage, errorMessage }
            }
        };
    }

    /// <summary>
    /// Resource create failed problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="newResource">The new resource.</param>
    /// <param name="reasons">The reasons.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourceCreateFailedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource,
        object? newResource,
        IEnumerable<string> reasons)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status422UnprocessableEntity,
            Type = ProblemResultConstants.ResourceCreateFailedType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceCreateFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Reasons, reasons },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.NewResource, newResource }
            }
        };
    }

    /// <summary>
    /// Resource update failed problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="updatedResource">The updated resource.</param>
    /// <param name="reasons">The reasons.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourceUpdateFailedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource,
        object? updatedResource,
        IEnumerable<string> reasons)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status422UnprocessableEntity,
            Type = ProblemResultConstants.ResourceUpdateFailedType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Reasons, reasons },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.UpdatedResource, updatedResource }
            }
        };
    }

    /// <summary>
    /// Resource patch failed problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="patches">The patches.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourcePatchFailedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource,
        IDictionary<string, string?> patches)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status422UnprocessableEntity,
            Type = ProblemResultConstants.ResourcePatchFailedType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.Patches, patches },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourcePatchFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, resource }
            }
        };
    }

    /// <summary>
    /// Resource delete failed problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourceDeleteFailedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status404NotFound,
            Type = ProblemResultConstants.ResourceDeleteFailedType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, resource }
            }
        };
    }

    /// <summary>
    /// Resource already exists problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ResourceAlreadyExistsProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource,
        string propertyName,
        string? propertyValue)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status409Conflict,
            Type = ProblemResultConstants.ConflictType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceAlreadyExists },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.PropertyName, propertyName },
                { ProblemResultConstants.PropertyValue, propertyValue }
            }
        };
    }

    /// <summary>
    /// Unauthorized problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails UnauthorizedProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage,
        string resource)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status401Unauthorized,
            Type = ProblemResultConstants.UnauthorizedType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Unauthorized },
                { ProblemResultConstants.Resource, context.GetInstance() }
            }
        };
    }

    /// <summary>
    /// Forbidden problem.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails ForbiddenProblem(this HttpContext context,
        string title,
        string detail,
        string errorMessage)
    {
        return new SondorProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = StatusCodes.Status403Forbidden,
            Type = ProblemResultConstants.ForbiddenType,
            Instance = context.GetInstance(),
            Extensions = new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                { ProblemResultConstants.Resource, context.GetInstance() }
            }
        };
    }
}
