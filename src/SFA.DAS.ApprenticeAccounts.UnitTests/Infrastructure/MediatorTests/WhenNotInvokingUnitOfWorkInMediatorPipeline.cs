using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using SFA.DAS.UnitOfWork.Managers;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Infrastructure.MediatorTests
{
    public class WhenNotInvokingUnitOfWorkInMediatorPipeline
    {
        private Mock<IUnitOfWorkManager> _unitOfWorkManager;
        private UnitOfWorkPipelineBehavior<SimpleRequest, SimpleResponse> _sut;

        [SetUp]
        public void Arrange()
        {
            _unitOfWorkManager = new Mock<IUnitOfWorkManager>();
            _sut = new UnitOfWorkPipelineBehavior<SimpleRequest, SimpleResponse>(_unitOfWorkManager.Object);
        }

        [Test, AutoData]
        public async Task Then_we_call_delegator(SimpleRequest request, SimpleResponse expectedResponse)
        {
            var response = await _sut.Handle(request, CancellationToken.None, () => Task.FromResult(expectedResponse));

            response.Should().Be(expectedResponse);
        }

        [Test, AutoData]
        public async Task Then_we_do_not_invoke_unit_of_work(SimpleRequest request, SimpleResponse expectedResponse)
        {
            await _sut.Handle(request, CancellationToken.None, () => Task.FromResult(expectedResponse));

            _unitOfWorkManager.Verify(x=>x.BeginAsync(), Times.Never);
            _unitOfWorkManager.Verify(x=>x.EndAsync(It.IsAny<Exception>()), Times.Never);
        }

        public class SimpleRequest
        {
        }

        public class SimpleResponse
        {
        }
    }
}
