using MediatR;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure
{
    public interface IEventDispatcher
    {
        public Task Dispatch(INotification message);
    }

    public class NullEventDispatcher : IEventDispatcher
    {
        public Task Dispatch(INotification message) => Task.CompletedTask;
    }

    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
            => _mediator = mediator;

        public async Task Dispatch(INotification message)
            => await _mediator.Publish(message);
    }
}