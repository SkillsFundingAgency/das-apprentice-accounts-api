using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipsQuery;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprenticeships")]
    public class GetApprenticeshipsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private readonly Apprentice _apprentice;

        public GetApprenticeshipsSteps(TestContext context)
        {
            _context = context;

            _apprentice = _fixture.Build<Apprentice>()
                .Create();
            _apprentice.AddApprenticeship(_fixture.Create<Revision>());
        }

        [Given("there is one apprenticeship")]
        public async Task GivenThereIsOneApprenticeship()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("there are no apprenticeships")]
        public void GivenThereAreNoApprenticeships()
        {
        }

        [When("we try to retrieve the apprenticeships")]
        public async Task WhenWeTryToRetrieveTheApprenticeships()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}/apprenticeships");
        }

        [Then("the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then("the response should match the apprenticeship in the database")]
        public async Task ThenTheResponseShouldMatchTheApprenticeshipInTheDatabase()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            var response = JsonConvert.DeserializeObject<ApprenticeshipsResponse>(content);
            response.Apprenticeships.Should().BeEquivalentTo(_apprentice.Apprenticeships.Select(a => new
            {
                a.Id,
                a.Revisions.OrderByDescending(b => b.Id).First().CommitmentsApprenticeshipId
            }));
        }

        [Then("the response be an empty list")]
        public async Task ThenTheResponseBeAnEmptyList()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            var response = JsonConvert.DeserializeObject<ApprenticeshipsResponse>(content);
            response.Apprenticeships.Should().BeEmpty();
        }
    }
}