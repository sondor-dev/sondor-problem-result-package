using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Sondor.ProblemResults.Extensions;

namespace Sondor.ProblemResults.Tests;

/// <summary>
/// Tests for <see cref="SondorProblemDetails"/>.
/// </summary>
[TestFixture]
public class SondorProblemDetailsTests
{
    /// <summary>
    /// Determines that <see cref="SondorProblemDetails.IsValidJson"/> works as expected.
    /// </summary>
    [Test]
    public void IsValidJson()
    {
        // arrange
        const string title = "title";
        const string detail = "detail";
        const string resource = "resource";
        const string errorMessage = "error-message";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";

        var context = new DefaultHttpContext();
        var problem =
            context.ResourceNotFoundProblem(title, detail, errorMessage, resource, propertyName, propertyValue);
        var json = JsonSerializer.Serialize(problem);

        // act
        var result = SondorProblemDetails.IsValidJson(json);

        // assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Determines that <see cref="SondorProblemDetails.IsValidJson"/> works as expected.
    /// </summary>
    [Test]
    public void IsValidJson_invalid()
    {
        // arrange
        const string invalid = "[]";

        // act
        var result = SondorProblemDetails.IsValidJson(invalid);

        // assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Determines that <see cref="SondorProblemDetails.IsValidJson"/> works as expected.
    /// </summary>
    [Test]
    public void IsValidJson_invalid_syntax()
    {
        // arrange
        const string invalid = "invalid-syntax";

        // act
        var result = SondorProblemDetails.IsValidJson(invalid);

        // assert
        Assert.That(result, Is.False);
    }
}