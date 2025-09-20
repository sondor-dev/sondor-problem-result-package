using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Sondor.ProblemResults.Extensions;

namespace Sondor.ProblemResults.Tests.Extensions;

/// <summary>
/// Collection of tests for <see cref="HttpContextExtensions"/>.
/// </summary>
[TestFixture]
public class HttpContextExtensionsTests
{
    [Test]
    public void GetUsername_ReturnsUserIdFromClaims()
    {
        // arrange
        var httpContext = new DefaultHttpContext();
        const string username = "123";
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, username)]));
        httpContext.User = claimsPrincipal;

        // act
        var actual = httpContext.GetUsername();

        // assert
        Assert.That(actual, Is.EqualTo(username));
    }
}