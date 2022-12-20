using NServiceBus;
using SFA.DAS.NServiceBus.Testing.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests;
public class TestEventPublisher : IMessageSession
{
    private readonly TestableEventPublisher _testableEventPublisher;

    public TestEventPublisher(TestableEventPublisher eventsPublished)
    {
        _testableEventPublisher = eventsPublished;
    }

    public Task Send(object message, SendOptions options)
    {
        throw new NotImplementedException();
    }

    public Task Send<T>(Action<T> messageConstructor, SendOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task Publish(object message, PublishOptions options)
    {
        await _testableEventPublisher.Publish(message);
    }

    public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions)
    {
        throw new NotImplementedException();
    }

    public Task Subscribe(Type eventType, SubscribeOptions options)
    {
        throw new NotImplementedException();
    }

    public Task Unsubscribe(Type eventType, UnsubscribeOptions options)
    {
        throw new NotImplementedException();
    }
}