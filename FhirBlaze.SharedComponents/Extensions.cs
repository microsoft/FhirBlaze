using Hl7.Fhir.Model;
using System;

namespace FhirBlaze.SharedComponents;

internal static class Extensions
{
    internal static Exception ToException(this OperationOutcome outcome)
    {
        var message = string.Empty;

        foreach (var issue in outcome.Issue)
        {
            if (issue.Severity == OperationOutcome.IssueSeverity.Fatal || issue.Severity == OperationOutcome.IssueSeverity.Error)
                message += issue.Details + Environment.NewLine;
        }

        return new Exception(message);
    }
}