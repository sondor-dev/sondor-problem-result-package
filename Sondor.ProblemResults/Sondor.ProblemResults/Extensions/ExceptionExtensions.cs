using Microsoft.AspNetCore.Http;
using Sondor.Errors.Exceptions;
using System;
using System.Linq;
using FluentValidation;
using Sondor.Translations;

namespace Sondor.ProblemResults.Extensions;

/// <summary>
/// Collection of exception extensions.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Converts a <see cref="Exception"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this Exception exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemUnexpectedErrorTitle();
        detail ??= translationManager.ProblemUnexpectedError();

        return context.UnexpectedProblem(title,
            detail,
            exception.Message);
    }

    /// <summary>
    /// Converts a <see cref="UnsupportedErrorCodeException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this UnsupportedErrorCodeException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemUnexpectedErrorTitle();
        detail ??= translationManager.ProblemUnexpectedError();

        return context.UnsupportedErrorCodeProblem(title,
            detail,
            exception.Message);
    }

    /// <summary>
    /// Converts a <see cref="ResourceNotFoundException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourceNotFoundException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemResourceNotFoundTitle();
        detail ??= translationManager.ProblemResourceNotFound(exception.Resource,
            exception.PropertyName,
            exception.PropertyValue ?? string.Empty);

        return context.ResourceNotFoundProblem(title,
            detail,
            exception.Message,
            exception.Resource,
            exception.PropertyName,
            exception.PropertyValue ?? string.Empty);
    }

    /// <summary>
    /// Converts a <see cref="ValidationException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The HTTP context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ValidationException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemValidationErrorsTitle();
        detail ??= translationManager.ProblemValidationErrors(exception.Errors.Count());

        title = string.Format(title, exception.Errors.Count());

        return context.ValidationProblem(title, detail, exception.Message, exception.Errors);
    }

    /// <summary>
    /// Converts a <see cref="OperationCanceledException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this OperationCanceledException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemTaskCancelledTitle();
        detail ??= translationManager.ProblemTaskCancelled(context.GetInstance());

        return context.CancelledProblem(title, detail, exception.Message);
    }

    /// <summary>
    /// Converts a <see cref="ResourceCreateFailedException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourceCreateFailedException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemResourceCreateFailedTitle();
        detail ??= translationManager.ProblemResourceCreateFailed(exception.Resource);

        return context.ResourceCreateFailedProblem(title,
            detail,
            exception.Message,
            exception.Resource,
            exception.NewResource,
            exception.Reasons);
    }

    /// <summary>
    /// Converts a <see cref="ResourceUpdateFailedException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourceUpdateFailedException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemResourceUpdateFailedTitle();
        detail ??= translationManager.ProblemResourceUpdateFailed(exception.Resource);

        return context.ResourceUpdateFailedProblem(title,
            detail,
            exception.Message,
            exception.Resource,
            exception.NewResource,
            exception.Reasons);
    }

    /// <summary>
    /// Converts a <see cref="ResourcePatchFailedException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourcePatchFailedException exception,
        HttpContext context,
        ISondorTranslationManager translationManager,
        string? title = null,
        string? detail = null)
    {
        title ??= translationManager.ProblemResourcePatchFailedTitle();
        detail ??= translationManager.ProblemResourcePatchFailed(exception.Resource);

        return context.ResourcePatchFailedProblem(title,
            detail,
            exception.Message,
            exception.Resource,
            exception.Patches);
    }

    /// <summary>
    /// Converts a <see cref="ResourceDeleteFailedException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourceDeleteFailedException exception,
        HttpContext context,
        ISondorTranslationManager translationManager)
    {
        var title = translationManager.ProblemResourceDeleteFailedTitle();
        var detail = translationManager.ProblemResourceDeleteFailed(exception.Resource);

        return context.ResourceDeleteFailedProblem(title,
            detail,
            exception.Message,
            exception.Resource);
    }

    /// <summary>
    /// Converts a <see cref="ResourceAlreadyExistsException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ResourceAlreadyExistsException exception,
        HttpContext context,
        ISondorTranslationManager translationManager)
    {
        var title = translationManager.ProblemResourceAlreadyExistsTitle();
        var detail = translationManager.ProblemResourceAlreadyExists(exception.Resource,
            exception.PropertyName,
            exception.PropertyValue ?? string.Empty);

        return context.ResourceAlreadyExistsProblem(title,
            detail,
            exception.Message,
            exception.Resource,
            exception.PropertyName,
            exception.PropertyValue);
    }

    /// <summary>
    /// Converts a <see cref="UnauthorisedException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this UnauthorisedException exception,
        HttpContext context,
        ISondorTranslationManager translationManager)
    {
        var title = translationManager.ProblemUnauthorizedTitle();
        var detail = translationManager.ProblemUnauthorized(context.GetInstance());

        return context.UnauthorizedProblem(title,
            detail,
            exception.Message);
    }

    /// <summary>
    /// Converts a <see cref="ForbiddenException"/> into <see cref="SondorProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="context">The context.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the problem details.</returns>
    public static SondorProblemDetails ToProblemDetails(this ForbiddenException exception,
        HttpContext context,
        ISondorTranslationManager translationManager)
    {
        var title = translationManager.ProblemForbiddenTitle();
        var detail = translationManager.ProblemForbidden();

        return context.ForbiddenProblem(title,
            detail,
            exception.Message);
    }
}
