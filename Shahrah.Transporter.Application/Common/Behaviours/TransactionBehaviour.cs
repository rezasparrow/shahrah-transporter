using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalCommand, IRequest<TResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public TransactionBehaviour(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        await using var tran = await _dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await next();

            await tran.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await tran.RollbackAsync(cancellationToken);
            throw;
        }
    }
}