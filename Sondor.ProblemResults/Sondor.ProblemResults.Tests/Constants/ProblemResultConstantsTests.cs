using Sondor.Errors;
using Sondor.Errors.Exceptions;
using Sondor.Errors.Tests.Args;
using Sondor.ProblemResults.Constants;

namespace Sondor.ProblemResults.Tests.Constants;

/// <summary>
/// Tests for <see cref="ProblemResultConstants"/>.
/// </summary>
[TestFixture]
public class ProblemResultConstantsTests
{
    /// <summary>
    /// Ensures that <see cref="ProblemResultConstants.FindProblemTypeByErrorCode"/> throws <see cref="UnsupportedErrorCodeException"/> when an unsupported error code is provided.
    /// </summary>
    [Test]
    public void FindProblemTypeByErrorCode_throws_unsupported()
    {
        // arrange
        const int unsupportedErrorCode = 999;

        // act & assert
        Assert.Throws<UnsupportedErrorCodeException>(() => ProblemResultConstants.FindProblemTypeByErrorCode(unsupportedErrorCode));
    }

    /// <summary>
    /// Ensures that <see cref="ProblemResultConstants.FindProblemTypeByErrorCode"/> returns the correct problem type for each error code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    [TestCaseSource(typeof(SondorErrorCodeArgs))]
    public void FindProblemTypeByErrorCode(int errorCode)
    {
        // arrange
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        var expected = errorCode switch
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        {
            SondorErrorCodes.BadRequest => ProblemResultConstants.BadRequestType,
            SondorErrorCodes.ResourceAlreadyExists => ProblemResultConstants.ConflictType,
            SondorErrorCodes.Forbidden => ProblemResultConstants.ForbiddenType,
            SondorErrorCodes.Unauthorized => ProblemResultConstants.UnauthorizedType,
            SondorErrorCodes.TaskCancelled => ProblemResultConstants.RequestCancelledType,
            SondorErrorCodes.ResourceNotFound => ProblemResultConstants.ResourceNotFoundType,
            SondorErrorCodes.ResourcePatchFailed => ProblemResultConstants.ResourcePatchFailedType,
            SondorErrorCodes.ResourceDeleteFailed => ProblemResultConstants.ResourceDeleteFailedType,
            SondorErrorCodes.ResourceUpdateFailed => ProblemResultConstants.ResourceUpdateFailedType,
            SondorErrorCodes.ResourceCreateFailed => ProblemResultConstants.ResourceCreateFailedType,
            SondorErrorCodes.UnexpectedError => ProblemResultConstants.UnexpectedErrorType,
            SondorErrorCodes.ValidationFailed => ProblemResultConstants.BadRequestType,
            _ => throw new UnsupportedErrorCodeException(errorCode)
        };

        // act
        var actual = ProblemResultConstants.FindProblemTypeByErrorCode(errorCode);

        // assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}