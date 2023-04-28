using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SFA.DAS.ApprenticeAccounts.Api.Filters;

public class ResponseCodeTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        var requestTelemetry = telemetry as RequestTelemetry;

        if (requestTelemetry == null) return;
        int code;
        bool parsed = Int32.TryParse(requestTelemetry.ResponseCode, out code);
        if (!parsed) return;

        var statusesNotRaisingExceptions = new List<HttpStatusCode> { HttpStatusCode.NotFound, HttpStatusCode.BadRequest };

        foreach (var status in statusesNotRaisingExceptions.Where(status => code == (int)status))
        {
            // If we set the Success property, the SDK won't change it:
            requestTelemetry.Success = true;

            // Allow us to filter these requests in the portal:
            requestTelemetry.Properties["Overridden400s"] = "true";
        }
    }
}
