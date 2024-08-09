using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Steps;

[Binding]
[Scope(Feature = "UpdateGovIdentifier")]
public class UpdateGovLoginIdentifierSteps
{
    private readonly TestContext _context;
    private Fixture _fixture = new Fixture();
    private Apprentice _apprentice;
    private string? _mailAddress;
    private string? _govUkIdentifier;
    
    public UpdateGovLoginIdentifierSteps(TestContext context)
    {
        _context = context;
    }
    [Given(@"we have an existing apprentice")]
    public void GivenWeHaveAnExistingApprentice()
    {
        _apprentice = _fixture.Build<Apprentice>()
            .Without(a => a.TermsOfUseAccepted)
            .With(a => a.UpdatedOn, DateTime.UtcNow.AddDays(-1))
            .Create();

        _context.DbContext.Apprentices.Add(_apprentice);
        _context.DbContext.SaveChanges();
    }
      
    [Given(@"we Update gov login identifier with an valid value")]
    public void GivenChangeGovLoginIdentifier()
    {
        _govUkIdentifier = _fixture.Create<string>();
    }
    
    [When(@"we add the apprentice's GOV login identifier")]
    public async Task WhenWeAddTheApprenticesGovLoginIdentifier()
    {
        var patch = new JsonPatchDocument<ApprenticePatchDto>().Replace(x => x.GovUkIdentifier, _govUkIdentifier);
        await _context.Api.Patch($"apprentices/{_apprentice.Id}", patch);
    }
    
    [Then(@"the apprentice record GOV login identifier is updated")]
    public void ThenTheApprenticeRecordGovUkIdentifierIsUpdated()
    {
        _context.Api.Response.Should().Be2XXSuccessful();
        _context.DbContext.Apprentices.Should().ContainEquivalentOf(new
        {
            _apprentice.Id,
            GovUkIdentifier = _govUkIdentifier,
        });
    }
}