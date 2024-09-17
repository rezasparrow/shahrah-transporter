using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.FinancialTransactions.Services;

public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly IApplicationDbContext _dbContext;

    public FinancialTransactionService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTransaction(FinancialTransactionBuilderFinal builder)
    {
        await CreateTransaction(new[] { builder });
    }

    public async Task CreateTransaction(IEnumerable<FinancialTransactionBuilderFinal> builders)
    {
        var financialTransactionBuilderFinals = builders.ToList();
        if (!financialTransactionBuilderFinals.Any()) throw new Exception("No transaction found to change transporter balance.");

        var transactions = financialTransactionBuilderFinals.Select(t => t.Build())
            .GroupBy(t => t.PersonId)
            .Select(t => new { personId = t.Key, transactions = t.ToList() });

        foreach (var group in transactions)
            await _dbContext.FinancialTransactions.AddRangeAsync(group.transactions);

        await _dbContext.SaveChangesAsync();
    }
}