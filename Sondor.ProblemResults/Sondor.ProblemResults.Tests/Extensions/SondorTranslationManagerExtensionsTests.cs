using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Sondor.ProblemResults.Constants;
using Sondor.ProblemResults.Extensions;
using Sondor.Translations.Args;
using Sondor.Tests.Args;
using Sondor.Translations;
using Sondor.Translations.Extensions;
using Sondor.Translations.Options;

namespace Sondor.ProblemResults.Tests.Extensions;

/// <summary>
/// The test class for <see cref="SondorTranslationManagerExtensions"/>.
/// </summary>
[TestFixtureSource(typeof(LanguageArgs))]
public class SondorTranslationManagerExtensionsTests
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
    public SondorTranslationManagerExtensionsTests(string language)
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
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemBadRequest"/> throws <see cref="ArgumentException"/> for invalid path.
    /// </summary>
    /// <param name="path"></param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemBadRequest_ShouldThrowExceptionForInvalidPath(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            Assert.Pass("Valid path!");
            
            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemBadRequest(HttpMethod.Get.Method, path!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemBadRequest"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemBadRequest_ShouldReturnTranslation()
    {
        // arrange
        const string path = "/api/test";
        var method = HttpMethod.Get;

        var defaultTranslation = string.Format(TranslationDefaultConstants.BadRequest, method, path);
        
        // act
        var result = _translationManager.ProblemBadRequest(method.Method, path);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));
            
            return;
        }
        
        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemBadRequestTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemBadRequestTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.BadRequestTitle;

        // act
        var result = _translationManager.ProblemBadRequestTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemForbidden"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemForbidden_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.Forbidden;

        // act
        var result = _translationManager.ProblemForbidden();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemForbiddenTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemForbiddenTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ForbiddenTitle;

        // act
        var result = _translationManager.ProblemForbiddenTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceAlreadyExists"/> throws <see cref="ArgumentException"/> for invalid resource.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceAlreadyExists_ShouldThrowExceptionForInvalidResource(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceAlreadyExists(resource!, "id", "1"));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceAlreadyExists"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceAlreadyExists_ShouldThrowExceptionForInvalidIdentifier(string? identifier)
    {
        if (!string.IsNullOrWhiteSpace(identifier))
        {
            Assert.Pass("Valid identifier!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceAlreadyExists("resource", identifier!, "1"));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceAlreadyExists"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="value">The value.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceAlreadyExists_ShouldThrowExceptionForInvalidValue(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Assert.Pass("Valid value!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceAlreadyExists("resource", "id", value!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceAlreadyExists"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceAlreadyExists_ShouldReturnTranslation()
    {
        // arrange
        const string id = "id";
        const string value = "1";
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourceAlreadyExists, resource, id, value);

        // act
        var result = _translationManager.ProblemResourceAlreadyExists(resource, id, value);
        
        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceAlreadyExistsTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceAlreadyExistsTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourceAlreadyExistsTitle;

        // act
        var result = _translationManager.ProblemResourceAlreadyExistsTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceCreateFailed"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceCreateFailed_ShouldThrowExceptionForInvalidValue(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceCreateFailed(resource!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceCreateFailed"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceCreateFailed_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourceCreateFailed, resource);

        // act
        var result = _translationManager.ProblemResourceCreateFailed(resource);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceCreateFailedTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceCreateFailedTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourceCreateFailedTitle;

        // act
        var result = _translationManager.ProblemResourceCreateFailedTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceCreateFailed"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceDeleteFailed_ShouldThrowExceptionForInvalidValue(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceDeleteFailed(resource!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceDeleteFailed"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceDeleteFailed_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourceDeleteFailed, resource);

        // act
        var result = _translationManager.ProblemResourceDeleteFailed(resource);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceDeleteFailedTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceDeleteFailedTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourceDeleteFailedTitle;

        // act
        var result = _translationManager.ProblemResourceDeleteFailedTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceNotFound"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceNotFound_ShouldThrowExceptionForInvalidResource(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceNotFound(resource!, "Property Name", "test"));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceNotFound"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceNotFound_ShouldThrowExceptionForInvalidPropertyName(string? propertyName)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.Pass("Valid property name!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceNotFound("resource", propertyName!, "test"));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceNotFound"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="propertyValue">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceNotFound_ShouldThrowExceptionForInvalidPropertyValue(string? propertyValue)
    {
        if (!string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.Pass("Valid property value!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceNotFound("resource", "property name", propertyValue!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceNotFound"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceNotFound_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        const string propertyName = "Property Name";
        const string propertyValue = "test";

        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourceNotFound, resource, propertyName, propertyValue);

        // act
        var result = _translationManager.ProblemResourceNotFound(resource, propertyName, propertyValue);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceNotFoundTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceNotFoundTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourceNotFoundTitle;

        // act
        var result = _translationManager.ProblemResourceNotFoundTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceUpdateFailed"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourcePatchFailed_ShouldThrowExceptionForInvalidResource(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourcePatchFailed(resource!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourcePatchFailed"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourcePatchFailed_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourcePatchFailed, resource);

        // act
        var result = _translationManager.ProblemResourcePatchFailed(resource);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourcePatchFailedTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourcePatchFailedTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourcePatchFailedTitle;

        // act
        var result = _translationManager.ProblemResourcePatchFailedTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceUpdateFailed"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemResourceUpdateFailed_ShouldThrowExceptionForInvalidResource(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemResourceUpdateFailed(resource!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceUpdateFailed"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceUpdateFailed_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ResourceUpdateFailed, resource);

        // act
        var result = _translationManager.ProblemResourceUpdateFailed(resource);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceUpdateFailedTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemResourceUpdateFailedTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ResourceUpdateFailedTitle;

        // act
        var result = _translationManager.ProblemResourceUpdateFailedTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemResourceUpdateFailed"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemUnauthorized_ShouldThrowExceptionForInvalidResource(string? resource)
    {
        if (!string.IsNullOrWhiteSpace(resource))
        {
            Assert.Pass("Valid resource!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemUnauthorized(resource!));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemUnauthorized"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemUnauthorized_ShouldReturnTranslation()
    {
        // arrange
        const string resource = "test";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.Unauthorized, resource);

        // act
        var result = _translationManager.ProblemUnauthorized(resource);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemUnauthorizedTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemUnauthorizedTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.UnauthorizedTitle;

        // act
        var result = _translationManager.ProblemUnauthorizedTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemUnexpectedError"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemUnexpectedError_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.UnexpectedError;

        // act
        var result = _translationManager.ProblemUnexpectedError();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemUnexpectedErrorTitle"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemUnexpectedErrorTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.UnexpectedErrorTitle;

        // act
        var result = _translationManager.ProblemUnexpectedErrorTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemValidationErrors"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="totalErrors">The total validation errors.</param>
    [TestCaseSource(typeof(IntArgs))]
    public void ProblemValidationErrors_ShouldThrowExceptionForInvalidResource(int totalErrors)
    {
        if (totalErrors >0)
        {
            Assert.Pass("Valid total errors!");

            return;
        }

        Assert.Throws<ArgumentOutOfRangeException>(() => _translationManager.ProblemValidationErrors(totalErrors));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemValidationErrors"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemValidationErrors_ShouldReturnTranslation()
    {
        // arrange
        const int totalErrors = 10;
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ValidationError, totalErrors);

        // act
        var result = _translationManager.ProblemValidationErrors(totalErrors);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemValidationErrorsTitle"/> returns the correct translation.
    /// </summary>
    [Test]
    public void ProblemValidationErrorsTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ValidationErrorTitle;

        // act
        var result = _translationManager.ProblemValidationErrorsTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemTaskCancelled"/> throws <see cref="ArgumentException"/> for invalid identifier.
    /// </summary>
    /// <param name="task">The task.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ProblemTaskCancelled_ShouldThrowExceptionForInvalidResource(string task)
    {
        if (!string.IsNullOrWhiteSpace(task))
        {
            Assert.Pass("Valid task!");

            return;
        }

        Assert.Throws<ArgumentException>(() => _translationManager.ProblemTaskCancelled(task));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemTaskCancelled"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ProblemTaskCancelled_ShouldReturnTranslation()
    {
        // arrange
        const string task = "task";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.TaskCancelled, task);

        // act
        var result = _translationManager.ProblemTaskCancelled(task);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemTaskCancelledTitle"/> returns the correct translation.
    /// </summary>
    [Test]
    public void ProblemTaskCancelledTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.TaskCancelledTitle;

        // act
        var result = _translationManager.ProblemTaskCancelledTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ProblemTaskCancelled"/> returns the correct translation for a valid path.
    /// </summary>
    [Test]
    public void ServerNotFound_ShouldReturnTranslation()
    {
        // arrange
        const string uri = "uri";
        var defaultTranslation =
            string.Format(TranslationDefaultConstants.ServerNotFound, uri);

        // act
        var result = _translationManager.ServerNotFound(uri);

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }

    /// <summary>
    /// Ensures that <see cref="SondorTranslationManagerExtensions.ServerNotFoundTitle"/> returns the correct translation.
    /// </summary>
    [Test]
    public void ServerNotFoundTitle_ShouldReturnTranslation()
    {
        // arrange
        const string defaultTranslation = TranslationDefaultConstants.ServerNotFoundTitle;

        // act
        var result = _translationManager.ServerNotFoundTitle();

        // assert
        if (_language.Equals("en"))
        {
            Assert.That(result, Is.EqualTo(defaultTranslation));

            return;
        }

        Assert.That(result, Is.Not.EqualTo(defaultTranslation));
    }
}