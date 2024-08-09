using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Infrastructure.MediatorTests
{
    public class WhenLoggingTheMediatorPipeline
    {
        private Mock<ILogger<SimpleRequest>> _loggerMock;
        private LoggingPipelineBehavior<SimpleRequest, SimpleResponse> _sut;

        [SetUp]
        public void Arrange()
        {
            _loggerMock = new Mock<ILogger<SimpleRequest>>();
            _sut = new LoggingPipelineBehavior<SimpleRequest, SimpleResponse>(_loggerMock.Object);
        }

        [Test, AutoData]
        public async Task Then_we_call_delegator_and_return_response(SimpleRequest request, SimpleResponse expectedResponse)
        {
            var response = await _sut.Handle(request,  () => Task.FromResult(expectedResponse), CancellationToken.None);

            response.Should().Be(expectedResponse);
        }

        [Test, AutoData]
        public async Task Then_we_log_the_handler_is_starting(SimpleRequest request, SimpleResponse expectedResponse)
        {
            await _sut.Handle(request, () => Task.FromResult(expectedResponse), CancellationToken.None);

            _loggerMock.VerifyLog(LogLevel.Information, Times.Once(), $"Start handling '{typeof(SimpleRequest)}'");
        }

        [Test, AutoData]
        public async Task Then_we_log_the_handler_has_finished(SimpleRequest request, SimpleResponse expectedResponse)
        {
            await _sut.Handle(request,  () => Task.FromResult(expectedResponse),CancellationToken.None);

            _loggerMock.VerifyLog(LogLevel.Information, Times.Once(), $"End handling '{typeof(SimpleRequest)}'");
        }

        [Test, AutoData]
        public void Then_we_log_the_handler_has_errored(SimpleRequest request, SimpleResponse expectedResponse)
        {
            Func<Task> action = () => _sut.Handle(request, () => throw new Exception("failed"), CancellationToken.None);

            action.Should().ThrowAsync<Exception>().WithMessage("failed");

            _loggerMock.VerifyLog(LogLevel.Error, Times.Once(), $"Error handling '{typeof(SimpleRequest)}'");
        }

        public class SimpleRequest : IRequest<SimpleResponse>
        {
        }

        public class SimpleResponse
        {
        }
    }
}
