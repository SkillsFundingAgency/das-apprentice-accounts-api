using MediatR;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ApprenticeAccountsDbContext _context;

        public UnitOfWorkPipelineBehavior(ApprenticeAccountsDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!(request is IUnitOfWorkCommand))
            {
                return await next();
            }

            var transaction = await _context.Database.BeginTransactionAsync(CancellationToken.None);

            try
            {
                var response = await next();
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(CancellationToken.None);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
}