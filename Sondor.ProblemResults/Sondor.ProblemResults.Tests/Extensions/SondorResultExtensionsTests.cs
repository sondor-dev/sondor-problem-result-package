using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sondor.Errors;
using Sondor.Errors.Tests.Args;
using Sondor.ProblemResults.Constants;
using Sondor.ProblemResults.Extensions;
using Sondor.Translations.Args;
using Sondor.Translations;
using Sondor.Translations.Options;
using System.Globalization;
using FluentValidation.Results;
using Sondor.Translations.Extensions;

namespace Sondor.ProblemResults.Tests.Extensions;

/// <summary>
/// Tests for <see cref="SondorResultExtensions"/>.
/// </summary>
[TestFixtureSource(typeof(LanguageArgs))]
public class SondorResultExtensionsTests
{
    /// <summary>
    /// The language.
    /// </summary>
    private readonly string _language;

    /// <summary>
    /// The trace identifier.
    /// </summary>
    private const string _trace_identifier = "trace-id";

    /// <summary>
    /// The translation manager.
    /// </summary>
    private readonly ISondorTranslationManager _translationManager;

    /// <summary>
    /// Create a new instance of <see cref="SondorResultExtensionsTests"/>.
    /// </summary>
    public SondorResultExtensionsTests(string language)
    {
        _language = language;

        var services = new ServiceCollection()
            .AddLogging()
            .AddTestTranslation(new SondorTranslationOptions
            {
                DefaultCulture = "en",
                SupportedCultures = TranslationConstants.SupportedCultures,
                UseKeyAsDefaultValue = true
            }, "Test:Translation");
        var provider = services.BuildServiceProvider();

        _translationManager = provider.GetRequiredService<ISondorTranslationManager>();
    }

    /// <summary>
    /// Test setup.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var culture = new CultureInfo(_language);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultExtensions.ToProblemDetails"/> works correctly when the result is valid.
    /// </summary>
    [Test]
    public void ToProblemDetailsGeneric_ValidResult()
    {
        // arrange
        var context = CreateHttpContext();
        var result = new SondorResult<int>();

        // act
        var problemDetails = result.ToProblemDetails(_translationManager, context);

        // assert
        Assert.That(problemDetails, Is.Null);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultExtensions.ToProblemDetails"/> works correctly when the result is valid.
    /// </summary>
    [Test]
    public void ToProblemDetails_ValidResult()
    {
        // arrange
        var context = CreateHttpContext();
        var result = new SondorResult();

        // act
        var problemDetails = result.ToProblemDetails(_translationManager, context);

        // assert
        Assert.That(problemDetails, Is.Null);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultExtensions.ToProblemDetails"/> works correctly.
    /// </summary>
    [Test]
    public void ToProblemDetails_TaskCancelled()
    {
        // arrange
        const int errorCode = SondorErrorCodes.TaskCancelled;
        var context = CreateHttpContext();
        var result = FromErrorCode(errorCode, context);
        var expected = ProblemFromErrorCode(errorCode, context);

        // act
        var problemDetails = result.ToProblemDetails(_translationManager, context);

        // assert
        SondorProblemDetails.Assert(problemDetails, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultExtensions.ToProblemDetails"/> works correctly.
    /// </summary>
    [Test]
    public void ToProblemDetails_Validation()
    {
        // arrange
        const int errorCode = SondorErrorCodes.ValidationFailed;
        var context = CreateHttpContext();
        var result = FromErrorCode(errorCode, context);
        var expected = ProblemFromErrorCode(errorCode, context);

        // act
        var problemDetails = result.ToProblemDetails(_translationManager, context);

        // assert
        SondorProblemDetails.Assert(problemDetails, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultExtensions.ToProblemDetails"/> works correctly.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    [TestCaseSource(typeof(SondorErrorCodeArgs))]
    public void ToProblemDetails(int errorCode)
    {
        // arrange
        var context = CreateHttpContext();
        var result = FromErrorCode(errorCode, context);
        var expected = ProblemFromErrorCode(errorCode, context);

        // act
        var problemDetails = result.ToProblemDetails(_translationManager, context);
        
        // assert
        SondorProblemDetails.Assert(problemDetails, expected);
    }

    /// <summary>
    /// Create default HTTP context.
    /// </summary>
    /// <returns>Returns the HTTP context.</returns>
    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext
        {
            Request =
            {
                Method = HttpMethod.Get.Method,
                Path = "/test",
                Host = new HostString("localhost"),
                Protocol = "HTTP/1.1"
            },
            TraceIdentifier = _trace_identifier
        };

        return context;
    }

    /// <summary>
    /// Create a <see cref="SondorResult"/> from the provided <paramref name="errorCode"/>.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the result.</returns>
    private SondorResult FromErrorCode(int errorCode,
        HttpContext context)
    {
        const string resource = "test";
        const string newResource = "new-resource";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";
        const string updatedResource = "updated-resource";

        ValidationFailure[] validationErrors = [new ("test", "error")];
        var patches = new Dictionary<string, string?>([new KeyValuePair<string, string?>("patch-1", "value")]);
        string[] reasons = ["reason-1"];

        return errorCode switch
        {
            SondorErrorCodes.BadRequest => new SondorResult(new SondorError(SondorErrorCodes.BadRequest,
                ErrorType(errorCode),
                _translationManager.ProblemBadRequest(context.Request.Method, context.Request.Path),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemBadRequest(context.Request.Method, context.Request.Path) }
                })),
            SondorErrorCodes.Forbidden => new SondorResult(new SondorError(SondorErrorCodes.Forbidden,
                ErrorType(errorCode),
                _translationManager.ProblemForbidden(),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemForbidden() },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                    { ProblemResultConstants.Resource, context.GetInstance() }
                })),
            SondorErrorCodes.ResourceAlreadyExists => new SondorResult(new SondorError(SondorErrorCodes.ResourceAlreadyExists,
                ErrorType(errorCode),
                _translationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, _trace_identifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                })),
            SondorErrorCodes.ResourceCreateFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceCreateFailed,
                ErrorType(errorCode),
                _translationManager.ProblemResourceCreateFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, _trace_identifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceCreateFailed(resource) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.NewResource, newResource }
                })),
            SondorErrorCodes.ResourceDeleteFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceDeleteFailed,
                ErrorType(errorCode),
                _translationManager.ProblemResourceDeleteFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceDeleteFailed(resource) },
                    { ProblemResultConstants.Reasons, reasons },
                    { ProblemResultConstants.Resource, resource }
                })),
            SondorErrorCodes.ResourceNotFound => new SondorResult(new SondorError(SondorErrorCodes.ResourceNotFound,
                ErrorType(errorCode),
                _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, _trace_identifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                })),
            SondorErrorCodes.ResourcePatchFailed => new SondorResult(new SondorError(errorCode,
                ErrorType(errorCode),
                _translationManager.ProblemResourcePatchFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.Patches, patches },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourcePatchFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourcePatchFailed(resource) },
                    { ProblemResultConstants.Resource, resource }
                })),
            SondorErrorCodes.ResourceUpdateFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceUpdateFailed,
                ErrorType(errorCode),
                _translationManager.ProblemResourceUpdateFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceUpdateFailed(resource) },
                    { ProblemResultConstants.Reasons, reasons },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.UpdatedResource, updatedResource }
                })),
            SondorErrorCodes.TaskCancelled => new SondorResult(new SondorError(SondorErrorCodes.TaskCancelled,
                ErrorType(errorCode),
                _translationManager.ProblemTaskCancelled(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.TaskCancelled },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemTaskCancelled(resource) }
                })),
            SondorErrorCodes.ValidationFailed => new SondorResult(new SondorError(SondorErrorCodes.ValidationFailed,
                SondorErrorTypes.BadRequestType,
                _translationManager.ProblemValidationErrors(1),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemValidationErrors(1) },
                    { ProblemResultConstants.Errors, validationErrors }
                })),
            SondorErrorCodes.Unauthorized => new SondorResult(new SondorError(SondorErrorCodes.Unauthorized,
                ErrorType(errorCode),
                _translationManager.ProblemUnauthorized(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemUnauthorized(resource) }
                })),
            _ => new SondorResult(new SondorError(SondorErrorCodes.UnexpectedError,
                SondorErrorTypes.UnexpectedErrorType,
                _translationManager.ProblemUnexpectedError(),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemUnexpectedError() }
                })),
        };
    }

    /// <summary>
    /// Gets the appropriate error type for the provided <paramref name="errorCode"/>.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <returns>Returns the error type.</returns>
    private static string ErrorType(int errorCode)
    {
        return errorCode switch
        {
            SondorErrorCodes.Unauthorized => ProblemResultConstants.UnauthorizedType,
            SondorErrorCodes.Forbidden => ProblemResultConstants.ForbiddenType,
            SondorErrorCodes.ResourceNotFound => ProblemResultConstants.ResourceNotFoundType,
            SondorErrorCodes.BadRequest => ProblemResultConstants.BadRequestType,
            SondorErrorCodes.ResourceAlreadyExists => ProblemResultConstants.ConflictType,
            SondorErrorCodes.ResourceCreateFailed => ProblemResultConstants.ResourceCreateFailedType,
            SondorErrorCodes.ResourceUpdateFailed => ProblemResultConstants.ResourceUpdateFailedType,
            SondorErrorCodes.ResourcePatchFailed => ProblemResultConstants.ResourcePatchFailedType,
            SondorErrorCodes.ResourceDeleteFailed => ProblemResultConstants.ResourceDeleteFailedType,
            SondorErrorCodes.TaskCancelled => ProblemResultConstants.RequestCancelledType,
            _ => ProblemResultConstants.UnexpectedErrorType
        };
    }

    /// <summary>
    /// Gets the expected <see cref="SondorProblemDetails"/> from the provided <paramref name="errorCode"/>.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="context">The context.</param>
    /// <returns>Returns the problem.</returns>
    private SondorProblemDetails ProblemFromErrorCode(int errorCode, HttpContext context)
    {
        const string resource = "test";
        const string newResource = "new-resource";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";
        const string updatedResource = "updated-resource";

        List<ValidationError> errors = [new("test", [new ValidationFailure("test", "error")])];
        var patches = new Dictionary<string, string?>([new KeyValuePair<string, string?>("patch-1", "value")]);
        string[] reasons = ["reason-1"];

        return errorCode switch
        {
            SondorErrorCodes.BadRequest => new SondorProblemDetails
            {
                Title = _translationManager.ProblemBadRequestTitle(),
                Detail = _translationManager.ProblemBadRequest(context.Request.Method, context.Request.Path),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status400BadRequest,
                Type = ProblemResultConstants.BadRequestType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.BadRequest },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemBadRequest(context.Request.Method, context.Request.Path) }
                }
            },
            SondorErrorCodes.Forbidden => new SondorProblemDetails
            {
                Title = _translationManager.ProblemForbiddenTitle(),
                Detail = _translationManager.ProblemForbidden(),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status403Forbidden,
                Type = ProblemResultConstants.ForbiddenType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemForbidden() },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                    { ProblemResultConstants.Resource, context.GetInstance() }
                }
            },
            SondorErrorCodes.ResourceAlreadyExists => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourceAlreadyExistsTitle(),
                Detail = _translationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status409Conflict,
                Type = ProblemResultConstants.ConflictType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceAlreadyExists },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                }
            },
            SondorErrorCodes.ResourceCreateFailed => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourceCreateFailedTitle(),
                Detail = _translationManager.ProblemResourceCreateFailed(resource),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemResultConstants.ResourceCreateFailedType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceCreateFailed(resource) },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceCreateFailed },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.NewResource, newResource },
                    { ProblemResultConstants.Reasons, reasons }
                }
            },
            SondorErrorCodes.ResourceDeleteFailed => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourceDeleteFailedTitle(),
                Detail = _translationManager.ProblemResourceDeleteFailed(resource),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status404NotFound,
                Type = ProblemResultConstants.ResourceDeleteFailedType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceDeleteFailed(resource) },
                    { ProblemResultConstants.Resource, resource }
                }
            },
            SondorErrorCodes.ResourceNotFound => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourceNotFoundTitle(),
                Detail = _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status404NotFound,
                Type = ProblemResultConstants.ResourceNotFoundType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceNotFound },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                }
            },
            SondorErrorCodes.ResourcePatchFailed => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourcePatchFailedTitle(),
                Detail = _translationManager.ProblemResourcePatchFailed(resource),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemResultConstants.ResourcePatchFailedType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.Patches, patches },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourcePatchFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourcePatchFailed(resource) },
                    { ProblemResultConstants.Resource, resource }
                }
            },
            SondorErrorCodes.ResourceUpdateFailed => new SondorProblemDetails
            {
                Title = _translationManager.ProblemResourceUpdateFailedTitle(),
                Detail = _translationManager.ProblemResourceUpdateFailed(resource),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemResultConstants.ResourceUpdateFailedType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemResourceUpdateFailed(resource) },
                    { ProblemResultConstants.Reasons, reasons },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.UpdatedResource, updatedResource }
                }
            },
            SondorErrorCodes.TaskCancelled => new SondorProblemDetails
            {
                Title = _translationManager.ProblemTaskCancelledTitle(),
                Detail = _translationManager.ProblemTaskCancelled(context.GetInstance()),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status499ClientClosedRequest,
                Type = ProblemResultConstants.RequestCancelledType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.TaskCancelled },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemTaskCancelled(resource) }
                }
            },
            SondorErrorCodes.ValidationFailed => new SondorProblemDetails
            {
                Title = _translationManager.ProblemValidationErrorsTitle(),
                Detail = _translationManager.ProblemValidationErrors(1),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status400BadRequest,
                Type = ProblemResultConstants.BadRequestType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemValidationErrors(errors.Count) },
                    { ProblemResultConstants.Errors, errors }
                }
            },
            SondorErrorCodes.Unauthorized => new SondorProblemDetails
            {
                Title = _translationManager.ProblemUnauthorizedTitle(),
                Detail = _translationManager.ProblemUnauthorized(resource),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status401Unauthorized,
                Type = ProblemResultConstants.UnauthorizedType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.Unauthorized },
                    { ProblemResultConstants.Resource, context.GetInstance() },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemUnauthorized(resource) }
                }
            },
            _ => new SondorProblemDetails
            {
                Title = _translationManager.ProblemUnexpectedErrorTitle(),
                Detail = _translationManager.ProblemUnexpectedError(),
                Instance = context.GetInstance(),
                Status = StatusCodes.Status500InternalServerError,
                Type = ProblemResultConstants.UnexpectedErrorType,
                Extensions = new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                    { ProblemResultConstants.ErrorMessage, _translationManager.ProblemUnexpectedError() }
                }
            }
        };
    }
}
