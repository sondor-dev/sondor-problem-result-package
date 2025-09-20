using System.Collections.Generic;
using FluentValidation.Results;

namespace Sondor.ProblemResults;

/// <summary>
/// Validation error.
/// </summary>
/// <remarks>
/// Create a new instance of <see cref="ValidationError"/>.
/// </remarks>
/// <param name="propertyName">The property name.</param>
/// <param name="errors">The property errors.</param>
public class ValidationError(string propertyName,
    IEnumerable<ValidationFailure> errors)
{
    /// <summary>
    /// The property name.
    /// </summary>
    public string PropertyName => propertyName;

    /// <summary>
    /// The property errors.
    /// </summary>
    public IEnumerable<ValidationFailure> Errors => errors;
}