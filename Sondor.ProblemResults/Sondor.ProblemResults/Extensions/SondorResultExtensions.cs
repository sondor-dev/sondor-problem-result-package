using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Sondor.Errors;
using Sondor.ProblemResults.Constants;
using Sondor.Translations;

namespace Sondor.ProblemResults.Extensions;

/// <summary>
/// Collection of <see cref="SondorResult"/> extensions.
/// </summary>
public static class SondorResultExtensions
{
    /// <inheritdoc cref="ToProblemDetails"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorProblemDetails? ToProblemDetails<TResult>(this SondorResult<TResult> result,
        ISondorTranslationManager translationManager,
        HttpContext context)
    {
        var simpleResult = new SondorResult(result.Error);

        return simpleResult.ToProblemDetails(translationManager, context);
    }

    /// <summary>
    /// Converts the provided <paramref name="result"/> to a <see cref="SondorProblemDetails"/>, using the provided <paramref name="translationManager"/>
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the problem.</returns>
    public static SondorProblemDetails? ToProblemDetails(this SondorResult result,
        ISondorTranslationManager translationManager,
        HttpContext context)
    {
        if (result.IsValid || result.Error is null)
        {
            return null;
        }

        var resource = result.Error.Value.Context.TryGetValue(ProblemResultConstants.Resource, out var resourceValue) ? resourceValue?.ToString() ?? string.Empty : string.Empty;
        var newResource = result.Error.Value.Context.TryGetValue(ProblemResultConstants.NewResource, out var newResourceValue) ? newResourceValue?.ToString() ?? string.Empty : string.Empty;
        var propertyName = result.Error.Value.Context.TryGetValue(ProblemResultConstants.PropertyName, out var propertyNameValue) ? propertyNameValue?.ToString() ?? string.Empty : string.Empty;
        var propertyValue = result.Error.Value.Context.TryGetValue(ProblemResultConstants.PropertyValue, out var propertyValueValue) ? propertyValueValue?.ToString() ?? string.Empty : string.Empty;
        var errors = result.Error.Value.Context.TryGetValue(ProblemResultConstants.Errors, out var errorsValue) ? (ValidationFailure[]?)errorsValue ?? [] : [];
        var reasons = result.Error.Value.Context.TryGetValue(ProblemResultConstants.Reasons, out var reasonsValue) ? (IEnumerable<string>?)reasonsValue ?? [] : [];
        var patches = result.Error.Value.Context.TryGetValue(ProblemResultConstants.Patches, out var patchesValue) ? (IDictionary<string, string?>?)patchesValue ?? new Dictionary<string, string?>() : new Dictionary<string, string?>();
        var updatedResource = result.Error.Value.Context.TryGetValue(ProblemResultConstants.UpdatedResource, out var updatedValue) ? updatedValue : null;
        var errorMessage = result.Error.Value.Context.TryGetValue(ProblemResultConstants.ErrorMessage, out var errorMessageValue) ?  errorMessageValue?.ToString() ?? string.Empty : string.Empty;

        return result.Error.Value.ErrorCode switch
        {
            SondorErrorCodes.BadRequest => context.BadRequestProblem(
                translationManager.ProblemBadRequestTitle(),
                translationManager.ProblemBadRequest(context.Request.Method, context.Request.Path),
                errorMessage),
            SondorErrorCodes.Forbidden => context.ForbiddenProblem(
                translationManager.ProblemForbiddenTitle(),
                translationManager.ProblemForbidden(),
                result.Error.Value.ErrorDescription),
            SondorErrorCodes.ResourceCreateFailed => context.ResourceCreateFailedProblem(translationManager.ProblemResourceCreateFailedTitle(),
                translationManager.ProblemResourceCreateFailed(resource),
                result.Error.Value.ErrorDescription,
                resource,
                newResource,
                reasons),
            SondorErrorCodes.ResourceUpdateFailed => context.ResourceUpdateFailedProblem(
                translationManager.ProblemResourceUpdateFailedTitle(),
                translationManager.ProblemResourceUpdateFailed(resource),
                result.Error.Value.ErrorDescription,
                resource,
                updatedResource: updatedResource,
                reasons),
            SondorErrorCodes.ResourcePatchFailed => context.ResourcePatchFailedProblem(
                translationManager.ProblemResourcePatchFailedTitle(),
                translationManager.ProblemResourcePatchFailed(resource),
                result.Error.Value.ErrorDescription,
                resource,
                patches),
            SondorErrorCodes.ResourceDeleteFailed => context.ResourceDeleteFailedProblem(
                translationManager.ProblemResourceDeleteFailedTitle(),
                translationManager.ProblemResourceDeleteFailed(resource),
                result.Error.Value.ErrorDescription,
                resource),
            SondorErrorCodes.ResourceAlreadyExists => context.ResourceAlreadyExistsProblem(
                translationManager.ProblemResourceAlreadyExistsTitle(),
                translationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue),
                result.Error.Value.ErrorDescription,
                resource,
                propertyName,
                propertyValue),
            SondorErrorCodes.ResourceNotFound => context.ResourceNotFoundProblem(
                translationManager.ProblemResourceNotFoundTitle(),
                translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue),
                result.Error.Value.ErrorDescription,
                resource,
                propertyName,
                propertyValue),
            SondorErrorCodes.TaskCancelled => context.CancelledProblem(
                translationManager.ProblemTaskCancelledTitle(),
                translationManager.ProblemTaskCancelled(context.GetInstance()),
                result.Error.Value.ErrorDescription),
            SondorErrorCodes.Unauthorized => context.UnauthorizedProblem(
                translationManager.ProblemUnauthorizedTitle(),
                translationManager.ProblemUnauthorized(resource),
                result.Error.Value.ErrorDescription,
                resource),
            SondorErrorCodes.ValidationFailed => context.ValidationProblem(
                translationManager.ProblemValidationErrorsTitle(),
                translationManager.ProblemValidationErrors(errors.Length),
                result.Error.Value.ErrorDescription,
                errors
                ),
            _ => context.UnexpectedProblem(
                translationManager.ProblemUnexpectedErrorTitle(),
                translationManager.ProblemUnexpectedError(),
                result.Error.Value.ErrorDescription)
        };
    }
}