using Sondor.Errors;
using Sondor.Errors.Exceptions;

namespace Sondor.ProblemResults.Constants;

/// <summary>
/// Collection of problem result constants.
/// </summary>
public class ProblemResultConstants
{
    public const string TraceKey = "traceId";
    public const string ConflictType = "https://support.sondor-technology.co.uk/problems/conflict";
    public const string ForbiddenType = "https://support.sondor-technology.co.uk/problems/forbidden";
    public const string BadRequestType = "https://support.sondor-technology.co.uk/problems/bad-request";
    public const string UnauthorizedType = "https://support.sondor-technology.co.uk/problems/unauthorized";
    public const string UnexpectedErrorType = "https://support.sondor-technology.co.uk/problems/unexpected-error";
    public const string RequestCancelledType = "https://support.sondor-technology.co.uk/problems/request-cancelled";
    public const string ResourceNotFoundType = "https://support.sondor-technology.co.uk/problems/resource-not-found";
    public const string ResourcePatchFailedType = "https://support.sondor-technology.co.uk/problems/resource-patch-failed";
    public const string ResourceDeleteFailedType = "https://support.sondor-technology.co.uk/problems/resource-delete-failed";
    public const string ResourceUpdateFailedType = "https://support.sondor-technology.co.uk/problems/resource-update-failed";
    public const string ResourceCreateFailedType = "https://support.sondor-technology.co.uk/problems/resource-creation-failed";

    /// <summary>
    /// The errors extension key.
    /// </summary>
    public const string Errors = "errors";

    /// <summary>
    /// The reasons extension key.
    /// </summary>
    public const string Reasons = "reasons";

    /// <summary>
    /// The patches extension key.
    /// </summary>
    public const string Patches = "patches";

    /// <summary>
    /// The resource extension key.
    /// </summary>
    public const string Resource = "resource";

    /// <summary>
    /// The error code extension key.
    /// </summary>
    public const string ErrorCode = "error-code";
    
    /// <summary>
    /// The error message.
    /// </summary>
    public const string ErrorMessage = "error-message";

    /// <summary>
    /// The property name extension key.
    /// </summary>
    public const string PropertyName = "property-name";

    /// <summary>
    /// The property value extension key.
    /// </summary>
    public const string PropertyValue = "property-value";

    /// <summary>
    /// The new resource extension key.
    /// </summary>
    public const string NewResource = "new-resource";

    /// <summary>
    /// The updated resource extension key.
    /// </summary>
    public const string UpdatedResource = "updated-resource";

    /// <summary>
    /// Find problem type by error code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <returns>Returns the appropriate problem result type.</returns>
    /// <exception cref="UnsupportedErrorCodeException">This exception is thrown when an unsupported error code is provided.</exception>
    public static string FindProblemTypeByErrorCode(int errorCode)
    {
        return errorCode switch
        {
            SondorErrorCodes.BadRequest => BadRequestType,
            SondorErrorCodes.ResourceAlreadyExists => ConflictType,
            SondorErrorCodes.Forbidden => ForbiddenType,
            SondorErrorCodes.Unauthorized => UnauthorizedType,
            SondorErrorCodes.TaskCancelled => RequestCancelledType,
            SondorErrorCodes.ResourceNotFound => ResourceNotFoundType,
            SondorErrorCodes.ResourcePatchFailed => ResourcePatchFailedType,
            SondorErrorCodes.ResourceDeleteFailed => ResourceDeleteFailedType,
            SondorErrorCodes.ResourceUpdateFailed => ResourceUpdateFailedType,
            SondorErrorCodes.ResourceCreateFailed => ResourceCreateFailedType,
            SondorErrorCodes.UnexpectedError => UnexpectedErrorType,
            SondorErrorCodes.ValidationFailed => BadRequestType,
            _ => throw new UnsupportedErrorCodeException(errorCode)
        };
    }
}