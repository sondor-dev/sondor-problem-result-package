using FluentValidation.Results;

namespace Sondor.ProblemResults.Tests;

/// <summary>
/// Tests for <see cref="ValidationError"/>.
/// </summary>
[TestFixture]
public class ValidationErrorTests
{
    /// <summary>
    /// Ensures the constructor works as expected.
    /// </summary>
    [Test]
    public void Constructor()
    {
        // arrange
        const string propertyName = "Id";
        ValidationFailure[] errors = [new (propertyName, "Test error!")];

        // act
        var validationError = new ValidationError(propertyName, errors);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(validationError.PropertyName, Is.EqualTo(propertyName));
            Assert.That(validationError.Errors, Is.EqualTo(errors));
        }
    }
}
