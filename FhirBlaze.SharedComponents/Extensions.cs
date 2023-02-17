using Hl7.Fhir.Model;
using System;
using System.Linq;

namespace FhirBlaze.SharedComponents;

public static class Extensions
{
    public static Exception ToException(this OperationOutcome outcome)
    {
        var message = string.Join(Environment.NewLine, outcome.Issue.Where(i => i.Severity != OperationOutcome.IssueSeverity.Information).Select(i => i.Diagnostics).ToArray());

        return new Exception(message);
    }

    public static string RecurseErrorMessages(this Exception e, string message = "")
    {
        if (string.IsNullOrWhiteSpace(e?.Message))
            return message;

        if (!string.IsNullOrWhiteSpace(message))
            message += Environment.NewLine;

        message += e?.Message;

        if (string.IsNullOrWhiteSpace(e?.InnerException?.Message))
            return RecurseErrorMessages(e.InnerException, message);

        return message;
    }
}