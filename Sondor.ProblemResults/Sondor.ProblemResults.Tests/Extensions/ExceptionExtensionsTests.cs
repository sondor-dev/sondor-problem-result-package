using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sondor.Errors;
using Sondor.Errors.Exceptions;
using Sondor.ProblemResults.Constants;
using Sondor.ProblemResults.Extensions;
using Sondor.Translations.Args;
using Sondor.Translations;
using Sondor.Translations.Extensions;
using Sondor.Translations.Options;
using System.Globalization;

namespace Sondor.ProblemResults.Tests.Extensions;

/// <summary>
/// Tests for <see cref="ExceptionExtensions"/>.
/// </summary>
[TestFixtureSource(typeof(LanguageArgs))]
public class ExceptionExtensionsTests
{
    /// <summary>
    /// The language.
    /// </summary>
    private readonly string _language;

    /// <summary>
    /// The translation manager.
    /// </summary>
    private readonly ISondorTranslationManager _translationManager;

    /// <summary>
    /// Create a new instance of <see cref="SondorTranslationManagerExtensionsTests"/>.
    /// </summary>
    public ExceptionExtensionsTests(string language)
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
    /// Ensures that the exception extension methods work as expected.
    /// </summary>
    [Test]
    public void Exception()
    {
        // arrange
        var exception = new Exception("Test exception message.");
        var title = _translationManager.ProblemUnexpectedErrorTitle();
        var detail = _translationManager.ProblemUnexpectedError();
        var context = CreateHttpContext();
        var extensions = new Dictionary<string, object?>
        {
            { ProblemResultConstants.TraceKey, context.TraceIdentifier },
            { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
            { ProblemResultConstants.ErrorMessage, exception.Message }
        };
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.UnexpectedErrorType,
            StatusCodes.Status500InternalServerError,
            extensions);

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the unsupported error code exception extension methods work as expected.
    /// </summary>
    [Test]
    public void UnsupportedErrorCodeException()
    {
        // arrange
        var exception = new UnsupportedErrorCodeException(int.MinValue);
        var title = _translationManager.ProblemUnexpectedErrorTitle();
        var detail = _translationManager.ProblemUnexpectedError();
        var context = CreateHttpContext();
        var extensions = new Dictionary<string, object?>
        {
            { ProblemResultConstants.TraceKey, context.TraceIdentifier },
            { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
            { ProblemResultConstants.ErrorMessage, exception.Message }
        };
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.UnexpectedErrorType,
            StatusCodes.Status500InternalServerError,
            extensions);

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource not found error code exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourceNotFoundException()
    {
        // arrange
        const string resource = "resource";
        const string propertyName = "property";
        const string propertyValue = "value";
        var exception = new ResourceNotFoundException(resource, propertyName, propertyValue);
        var title = _translationManager.ProblemResourceNotFoundTitle();
        var detail = _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue);
        var context = CreateHttpContext();
        var extensions = new Dictionary<string, object?>
        {
            { ProblemResultConstants.TraceKey, context.TraceIdentifier },
            { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceNotFound },
            { ProblemResultConstants.ErrorMessage, exception.Message },
            { ProblemResultConstants.Resource, exception.Resource },
            { ProblemResultConstants.PropertyName, exception.PropertyName },
            { ProblemResultConstants.PropertyValue, exception.PropertyValue }
        };
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ResourceNotFoundType,
            StatusCodes.Status404NotFound,
            extensions);

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the validation exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ValidationException()
    {
        // arrange
        const string name = "name";
        const string alias = "alias";
        const string invalidName = "invalid-name";
        const string invalidAlias = "invalid-alias";
        
        var exception = new ValidationException([
            new ValidationFailure(name, invalidName, "test"),
            new ValidationFailure(alias, invalidAlias, "test")
        ]);
        var title = _translationManager.ProblemValidationErrorsTitle();
        var detail = _translationManager.ProblemValidationErrors(exception.Errors.Count());
        var errors = exception.Errors.GroupBy(prop => prop.PropertyName, (key, failure) => new ValidationError(
                key,
                failure
            ))
            .ToList();
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.BadRequestType,
            StatusCodes.Status400BadRequest,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.Errors, errors }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the task cancelled exception extension methods work as expected.
    /// </summary>
    [Test]
    public void OperationCanceledException()
    {
        // arrange
        const string message = "test";
        var exception = new OperationCanceledException(message);
        var context = CreateHttpContext();
        var title = _translationManager.ProblemTaskCancelledTitle();
        var detail = _translationManager.ProblemTaskCancelled(context.GetInstance());
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.RequestCancelledType,
            499,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.TaskCancelled },
                { ProblemResultConstants.ErrorMessage, exception.Message }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource create failed exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourceCreateFailedException()
    {
        // arrange
        const string resource = "resource";
        var newResource = new
        {
            Name = "test",
            Alias = "alias"
        };
        string[] reasons =
        [
            "reason-1",
            "reason-2"
        ];
        var exception = new ResourceCreateFailedException(resource, newResource, reasons);
        var title = _translationManager.ProblemResourceCreateFailedTitle();
        var detail = _translationManager.ProblemResourceCreateFailed(resource);
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ResourceCreateFailedType,
            StatusCodes.Status422UnprocessableEntity,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceCreateFailed },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.Reasons, reasons },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.NewResource, newResource }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource update exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourceUpdateFailedException()
    {
        // arrange
        const string resource = "resource";
        var newResource = new
        {
            Name = "test",
            Alias = "alias"
        };
        string[] reasons =
        [
            "reason-1",
            "reason-2"
        ];
        var exception = new ResourceUpdateFailedException(resource, newResource, reasons);
        var title = _translationManager.ProblemResourceUpdateFailedTitle();
        var detail = _translationManager.ProblemResourceUpdateFailed(resource);
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ResourceUpdateFailedType,
            StatusCodes.Status422UnprocessableEntity,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.Reasons, exception.Reasons },
                { ProblemResultConstants.Resource, exception.Resource },
                { ProblemResultConstants.UpdatedResource, exception.NewResource }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource patch exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourcePatchFailedException()
    {
        // arrange
        const string resource = "resource";
        var patches = new Dictionary<string, string?>
        {
            { "name", "value" },
            { "alias", "value" }
        };
        var exception = new ResourcePatchFailedException(resource, patches);
        var title = _translationManager.ProblemResourcePatchFailedTitle();
        var detail = _translationManager.ProblemResourcePatchFailed(resource);
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ResourcePatchFailedType,
            StatusCodes.Status422UnprocessableEntity,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.Patches, exception.Patches },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourcePatchFailed },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.Resource, exception.Resource }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource delete failed exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourceDeleteFailedException()
    {
        // arrange
        const string resource = "resource";
        string[] reasons =
        [
            "reason-1",
            "reason-2"
        ];
        var exception = new ResourceDeleteFailedException(resource, reasons);
        var title = _translationManager.ProblemResourceDeleteFailedTitle();
        var detail = _translationManager.ProblemResourceDeleteFailed(resource);
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ResourceDeleteFailedType,
            StatusCodes.Status404NotFound,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.Resource, exception.Resource }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the resource already exists exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ResourceAlreadyExistsException()
    {
        // arrange
        const string resource = "resource";
        const string identifier = "id";
        const string identifierValue = "value";
        var exception = new ResourceAlreadyExistsException(resource, identifier, identifierValue);
        var title = _translationManager.ProblemResourceAlreadyExistsTitle();
        var detail = _translationManager.ProblemResourceAlreadyExists(resource, identifier, identifierValue);
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ConflictType,
            StatusCodes.Status409Conflict,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.Resource, exception.Resource },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceAlreadyExists },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.PropertyName, exception.PropertyName },
                { ProblemResultConstants.PropertyValue, exception.PropertyValue }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the unauthorized exception extension methods work as expected.
    /// </summary>
    [Test]
    public void UnauthorisedException()
    {
        // arrange
        const string message = "test";
        var context = CreateHttpContext();
        var exception = new UnauthorisedException(message);
        var title = _translationManager.ProblemUnauthorizedTitle();
        var detail = _translationManager.ProblemUnauthorized(context.GetInstance());
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.UnauthorizedType,
            StatusCodes.Status401Unauthorized,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Unauthorized },
                { ProblemResultConstants.Resource, context.GetInstance() }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
    }

    /// <summary>
    /// Ensures that the forbidden exception extension methods work as expected.
    /// </summary>
    [Test]
    public void ForbiddenException()
    {
        // arrange
        const string message = "test";
        var exception = new ForbiddenException(message);
        var title = _translationManager.ProblemForbiddenTitle();
        var detail = _translationManager.ProblemForbidden();
        var context = CreateHttpContext();
        var expected = ExpectedProblemDetails(title,
            detail,
            ProblemResultConstants.ForbiddenType,
            StatusCodes.Status403Forbidden,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                { ProblemResultConstants.ErrorMessage, exception.Message },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                { ProblemResultConstants.Resource, context.GetInstance() }
            });

        // act
        var actual = exception.ToProblemDetails(context, _translationManager);

        // assert
        SondorProblemDetails.Assert(actual, expected);
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
            TraceIdentifier = "trace-id"
        };
        
        return context;
    }

    /// <summary>
    /// Construct the expected problem details.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="detail">The detail.</param>
    /// <param name="type">The type.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="extensions">The extensions.</param>
    /// <returns>Returns the problem.</returns>
    private static SondorProblemDetails ExpectedProblemDetails(string title,
        string detail,
        string type,
        int statusCode,
        Dictionary<string, object?> extensions)
    {
        var context = CreateHttpContext();
        
        return new SondorProblemDetails
        {
            Type = type,
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = context.GetInstance(),
            Extensions = extensions
        };
    }
}