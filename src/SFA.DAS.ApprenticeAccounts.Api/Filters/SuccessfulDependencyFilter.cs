using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SFA.DAS.ApprenticeAccounts.Api.Filters;

public class SuccessfulDependencyFilter : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }

    public SuccessfulDependencyFilter(ITelemetryProcessor next)
    {
        this.Next = next;
    }

    public void Process(ITelemetry item)
    {
        var statusesToIgnore = new List<HttpStatusCode>{ HttpStatusCode.NotFound, HttpStatusCode.BadRequest };

        if (!OKtoSend(item)) { return; }


        if (item is RequestTelemetry request)
        {
            if (statusesToIgnore.Any(status => ((int)status).ToString() == request.ResponseCode))
            {
                return;
            }
        }

        this.Next.Process(item);
    }

    private bool OKtoSend(ITelemetry item)
    {
        var dependency = item as DependencyTelemetry;
        if (dependency == null) return true;

        return dependency.Success != true;
    }
}