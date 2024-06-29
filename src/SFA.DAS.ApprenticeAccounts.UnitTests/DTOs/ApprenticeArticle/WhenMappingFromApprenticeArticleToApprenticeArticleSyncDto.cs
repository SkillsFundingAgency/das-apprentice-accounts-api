using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticeSync;
public class WhenMappingFromApprenticeArticleToApprenticeArticleSyncDto
{
    [Test]
    [RecursiveMoqAutoData]
    public void ThenTheFieldsAreMappedCorrectly(ApprenticeArticle apprenticeArticle)
    {
        var mappedObject = ApprenticeArticleSyncDto.MapToSyncResponse(apprenticeArticle);

        mappedObject.Id.Should().Be(apprenticeArticle.Id);
        mappedObject.EntryId.Should().Be(apprenticeArticle.EntryId);
        mappedObject.IsSaved.Should().Be(apprenticeArticle.IsSaved);
        mappedObject.LikeStatus.Should().Be(apprenticeArticle.LikeStatus);
    }
}