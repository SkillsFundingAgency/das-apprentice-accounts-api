using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Features;

[Binding]
[Scope(Feature = "GetApprentices")]
public class GetApprentices
{
    private readonly TestContext _context;
    private readonly Fixture _fixture = new Fixture();
    private Apprentice _apprentice;

    public GetApprentices(TestContext context)
    {
        _context = context;
    }

    [Given(@"there are apprentices with updates")]
    public async Task GivenThereAreApprenticesWithUpdates()
    {
        _apprentice = _fixture.Build<Apprentice>().Create();
        _apprentice.UpdatedOn = DateTime.UtcNow.AddDays(1);
        _context.DbContext.Apprentices.AddRange(new List<Apprentice>() { _apprentice });
        await _context.DbContext.SaveChangesAsync();
    }

    [When(@"we try to retrieve apprentices for sync by ids only")]
    public async Task WhenWeTryToRetrieveTheApprenticesForSync()
    {
        await SyncApprentices(new Guid[] { _apprentice.Id }, null);
    }

    [When(@"we try to retrieve apprentices for sync by a past date")]
    public async Task WhenWeTryToRetrieveApprenticesForSyncByAPastDate()
    {
        await SyncApprentices(new Guid[] { _apprentice.Id }, DateTime.Now.AddDays(-1));
    }

    [When(@"we try to retrieve apprentices for sync by a future date")]
    public async Task WhenWeTryToRetrieveApprenticesForSyncByAFutureDate()
    {
        await SyncApprentices(new Guid[] { _apprentice.Id }, DateTime.Now.AddDays(2));
    }

    [When("we try to retrieve apprentices for sync by empty parameters")]
    public async Task WhenWeTryToRetrieveApprenticesForSyncByEmptyParameters()
    {
        await SyncApprentices(Array.Empty<Guid>(), null);
    }

    private async Task SyncApprentices(Guid[] apprenticeIds, DateTime? updatedSinceDate)
    {
        await _context.Api.Post($"apprentices/sync?updatedSinceDate={updatedSinceDate?.ToString("yyyy-MM-dd")}", apprenticeIds);
    }

    private async Task<ApprenticeSyncResponseDto> ParseSyncResponse()
    {
        var responseData = await _context.Api.Response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ApprenticeSyncResponseDto>(responseData);
    }

    [Then("the response should contain the expected apprentice")]
    public async Task ThenTheResponseShouldContainTheExpectedApprentice()
    {
        var syncResponse = await ParseSyncResponse();
        var apprenticeIds = syncResponse.Apprentices.Select(a => a.ApprenticeID);
        apprenticeIds.Should().Contain(_apprentice.Id);
    }

    [Then("the response should have no apprentices")]
    public async Task ThenTheResponseShouldBeNoApprenticesReturned()
    {
        var syncResponse = await ParseSyncResponse();
        syncResponse.Apprentices.Should().BeEmpty();
    }

    [Then(@"the result should return ok")]
    public void ThenTheResultShouldReturnOk() => _context.Api.Response.Should().Be200Ok();
}
