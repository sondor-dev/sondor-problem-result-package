using Sondor.ProblemResults.Constants;
using Sondor.Translations.Attributes;

namespace Sondor.ProblemResults;

/// <summary>
/// Collection of translations for Sondor problem results.
/// </summary>
public enum SondorProblemResultTranslations
{
    /// <summary>
    /// The default translation for unknown errors.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The bad request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.BadRequest)]
    [TranslationDefault(TranslationDefaultConstants.BadRequest)]
    BadRequest = 1,

    /// <summary>
    /// The bad request title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.BadRequestTitle)]
    [TranslationDefault(TranslationDefaultConstants.BadRequestTitle)]
    BadRequestTitle = 2,

    /// <summary>
    /// The forbidden request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.Forbidden)]
    [TranslationDefault(TranslationDefaultConstants.Forbidden)]
    Forbidden = 3,

    /// <summary>
    /// The forbidden request title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ForbiddenTitle)]
    [TranslationDefault(TranslationDefaultConstants.ForbiddenTitle)]
    ForbiddenTitle = 4,

    /// <summary>
    /// The resource already exists request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceAlreadyExists)]
    [TranslationDefault(TranslationDefaultConstants.ResourceAlreadyExists)]
    ResourceAlreadyExists = 5,

    /// <summary>
    /// The resource already exists title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceAlreadyExistsTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourceAlreadyExistsTitle)]
    ResourceAlreadyExistsTitle = 6,

    /// <summary>
    /// The resource create failed request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceCreateFailed)]
    [TranslationDefault(TranslationDefaultConstants.ResourceCreateFailed)]
    ResourceCreateFailed = 7,

    /// <summary>
    /// The resource create failed title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceCreateFailedTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourceCreateFailedTitle)]
    ResourceCreateFailedTitle = 8,

    /// <summary>
    /// The resource delete failed request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceDeleteFailed)]
    [TranslationDefault(TranslationDefaultConstants.ResourceDeleteFailed)]
    ResourceDeleteFailed = 9,

    /// <summary>
    /// The resource delete failed title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceDeleteFailedTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourceDeleteFailedTitle)]
    ResourceDeleteFailedTitle = 10,

    /// <summary>
    /// The resource not found request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceNotFound)]
    [TranslationDefault(TranslationDefaultConstants.ResourceNotFound)]
    ResourceNotFound = 11,

    /// <summary>
    /// The resource not found title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceNotFoundTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourceNotFoundTitle)]
    ResourceNotFoundTitle = 12,

    /// <summary>
    /// The resource patch request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourcePatchFailed)]
    [TranslationDefault(TranslationDefaultConstants.ResourcePatchFailed)]
    ResourcePatchFailed = 13,

    /// <summary>
    /// The resource patch title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourcePatchFailedTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourcePatchFailedTitle)]
    ResourcePatchFailedTitle = 14,

    /// <summary>
    /// The resource update request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceUpdateFailed)]
    [TranslationDefault(TranslationDefaultConstants.ResourceUpdateFailed)]
    ResourceUpdateFailed = 15,

    /// <summary>
    /// The resource update title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ResourceUpdateFailedTitle)]
    [TranslationDefault(TranslationDefaultConstants.ResourceUpdateFailedTitle)]
    ResourceUpdateFailedTitle = 16,

    /// <summary>
    /// The unauthorized request translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.Unauthorized)]
    [TranslationDefault(TranslationDefaultConstants.Unauthorized)]
    Unauthorized = 17,

    /// <summary>
    /// The unauthorized title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.UnauthorizedTitle)]
    [TranslationDefault(TranslationDefaultConstants.UnauthorizedTitle)]
    UnauthorizedTitle = 18,

    /// <summary>
    /// The unexpected error translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.UnexpectedError)]
    [TranslationDefault(TranslationDefaultConstants.UnexpectedError)]
    UnexpectedError = 19,

    /// <summary>
    /// The unexpected error title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.UnexpectedErrorTitle)]
    [TranslationDefault(TranslationDefaultConstants.UnexpectedErrorTitle)]
    UnexpectedErrorTitle = 20,

    /// <summary>
    /// The validation error translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ValidationError)]
    [TranslationDefault(TranslationDefaultConstants.ValidationError)]
    ValidationError = 21,

    /// <summary>
    /// The validation error title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ValidationErrorTitle)]
    [TranslationDefault(TranslationDefaultConstants.ValidationErrorTitle)]
    ValidationErrorTitle = 22,
    
    /// <summary>
    /// The task cancelled translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.TaskCancelled)]
    [TranslationDefault(TranslationDefaultConstants.TaskCancelled)]
    TaskCancelled = 23,

    /// <summary>
    /// The task cancelled title translation.
    /// </summary>
    [TranslationKey(TranslationKeyConstants.ProblemTaskCancelledTitle)]
    [TranslationDefault(TranslationDefaultConstants.TaskCancelledTitle)]
    TaskCancelledTitle = 24,
}