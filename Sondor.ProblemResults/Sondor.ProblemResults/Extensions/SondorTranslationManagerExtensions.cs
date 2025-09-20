using System;
using Sondor.ProblemResults.Constants;
using Sondor.Translations;
using Sondor.Translations.Extensions;

namespace Sondor.ProblemResults.Extensions;

/// <summary>
/// Collection of extension methods for <see cref="ISondorTranslationManager"/>.
/// </summary>
public static class SondorTranslationManagerExtensions
{
    /// <summary>
    /// Sondor problem result translation.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="translation">The translation.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Returns the problem result translation.</returns>
    public static string ProblemResultTranslation(this ISondorTranslationManager translationManager,
        SondorProblemResultTranslations translation,
        params object[] parameters)
    {
        var key = translation.GetTranslationKey();
        var defaultTranslation = translation.GetTranslationDefault();

        var value = translationManager.Translate(key: key,
            location: TranslationConstants.Location,
            resource: TranslationConstants.Resource,
            defaultValue: defaultTranslation,
            parameters);
        
        return value;
    }

    /// <summary>
    /// Get the translation for a bad request problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="method">The request HTTP method.</param>
    /// <param name="path">The request path.</param>
    /// <returns>Returns the bad request translation.</returns>
    public static string ProblemBadRequest(this ISondorTranslationManager translationManager,
        string method,
        string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));
        }
        
        return translationManager.ProblemResultTranslation(
            SondorProblemResultTranslations.BadRequest,
            method.ToUpperInvariant(),
            path);
    }

    /// <summary>
    /// Get the translation for a bad request title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the bad request translation.</returns>
    public static string ProblemBadRequestTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(
            SondorProblemResultTranslations.BadRequestTitle);
    }

    /// <summary>
    /// Get the translation for a forbidden problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the forbidden translation.</returns>
    public static string ProblemForbidden(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.Forbidden);
    }

    /// <summary>
    /// Get the translation for a forbidden title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the forbidden translation.</returns>
    public static string ProblemForbiddenTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ForbiddenTitle);
    }

    /// <summary>
    /// Get the translation for a resource already exists problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource type.</param>
    /// <param name="propertyName">The resource identifier.</param>
    /// <param name="propertyValue">The resource identifier value.</param>
    /// <returns>Returns the resource already exists translation.</returns>
    public static string ProblemResourceAlreadyExists(this ISondorTranslationManager translationManager,
        string resource,
        string propertyName,
        string propertyValue)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }
        
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Identifier cannot be null or whitespace.", nameof(propertyName));
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new ArgumentException("Identifier value cannot be null or whitespace.", nameof(propertyValue));
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceAlreadyExists,
            resource,
            propertyName,
            propertyValue);
    }

    /// <summary>
    /// Get the translation for a resource already exists title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource already exists title translation.</returns>
    public static string ProblemResourceAlreadyExistsTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceAlreadyExistsTitle);
    }

    /// <summary>
    /// Get the translation for a resource creation failed problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the resource creation failed translation.</returns>
    public static string ProblemResourceCreateFailed(this ISondorTranslationManager translationManager,
        string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }
        
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceCreateFailed,
            resource);
    }

    /// <summary>
    /// Get the translation for a resource creation failed title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource creation failed translation.</returns>
    public static string ProblemResourceCreateFailedTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceCreateFailedTitle);
    }

    /// <summary>
    /// Get the translation for a resource deletion failed problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the resource creation failed translation.</returns>
    public static string ProblemResourceDeleteFailed(this ISondorTranslationManager translationManager,
        string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }
        
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceDeleteFailed,
            resource);
    }

    /// <summary>
    /// Get the translation for a resource deletion failed title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource creation failed translation.</returns>
    public static string ProblemResourceDeleteFailedTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceDeleteFailedTitle);
    }

    /// <summary>
    /// Get the translation for a resource not found problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <returns>Returns the resource not found translation.</returns>
    public static string ProblemResourceNotFound(this ISondorTranslationManager translationManager,
        string resource,
        string propertyName,
        string propertyValue)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }
        
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Property name cannot be null or whitespace.", nameof(propertyName));
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new ArgumentException("Property value cannot be null or whitespace.", nameof(propertyValue));
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceNotFound,
            resource,
            propertyName,
            propertyValue);
    }

    /// <summary>
    /// Get the translation for a resource not found title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource not found title translation.</returns>
    public static string ProblemResourceNotFoundTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceNotFoundTitle);
    }

    /// <summary>
    /// Get the translation for a resource patch failed problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the resource patch failed translation.</returns>
    public static string ProblemResourcePatchFailed(this ISondorTranslationManager translationManager,
        string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourcePatchFailed,
            resource);
    }

    /// <summary>
    /// Get the translation for a resource patch failed title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource patch failed translation.</returns>
    public static string ProblemResourcePatchFailedTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourcePatchFailedTitle);
    }

    /// <summary>
    /// Get the translation for a resource update failed problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the resource update failed translation.</returns>
    public static string ProblemResourceUpdateFailed(this ISondorTranslationManager translationManager,
        string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceUpdateFailed,
            resource);
    }

    /// <summary>
    /// Get the translation for a resource update failed title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the resource update failed translation.</returns>
    public static string ProblemResourceUpdateFailedTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ResourceUpdateFailedTitle);
    }

    /// <summary>
    /// Get the translation for an unauthorized request problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the unauthorized translation.</returns>
    public static string ProblemUnauthorized(this ISondorTranslationManager translationManager,
        string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException("Resource cannot be null or whitespace.", nameof(resource));
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.Unauthorized,
            resource);
    }

    /// <summary>
    /// Get the translation for an unauthorized request title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the unauthorized translation.</returns>
    public static string ProblemUnauthorizedTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.UnauthorizedTitle);
    }

    /// <summary>
    /// Get the translation for an unexpected error problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the unexpected error translation.</returns>
    public static string ProblemUnexpectedError(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.UnexpectedError);
    }

    /// <summary>
    /// Get the translation for an unexpected error title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the unexpected error title translation.</returns>
    public static string ProblemUnexpectedErrorTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.UnexpectedErrorTitle);
    }

    /// <summary>
    /// Get the translation for validation errors problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="totalErrors">The total number of errors.</param>
    /// <returns>Returns the validation errors translation.</returns>
    public static string ProblemValidationErrors(this ISondorTranslationManager translationManager,
        int totalErrors)
    {
        if (totalErrors <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalErrors), "Total errors must be greater than 0.");
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ValidationError,
            totalErrors);
    }

    /// <summary>
    /// Get the translation for a validation errors title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the validation errors title translation.</returns>
    public static string ProblemValidationErrorsTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.ValidationErrorTitle);
    }

    /// <summary>
    /// Get the translation for task cancelled problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <param name="task">The task.</param>
    /// <returns>Returns the task cancelled translation.</returns>
    public static string ProblemTaskCancelled(this ISondorTranslationManager translationManager,
        string task)
    {
        if (string.IsNullOrWhiteSpace(task))
        {
            throw new ArgumentException("The task parameter must not but empty, null or white space.");
        }

        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.TaskCancelled,
            task);
    }

    /// <summary>
    /// Get the translation for a task cancelled title problem result.
    /// </summary>
    /// <param name="translationManager">The translation manager.</param>
    /// <returns>Returns the task cancelled title translation.</returns>
    public static string ProblemTaskCancelledTitle(this ISondorTranslationManager translationManager)
    {
        return translationManager.ProblemResultTranslation(SondorProblemResultTranslations.TaskCancelledTitle);
    }
}