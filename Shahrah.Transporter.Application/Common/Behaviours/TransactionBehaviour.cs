using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse>(IApplicationDbContext dbContext) : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalCommand, IRequest<TResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
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