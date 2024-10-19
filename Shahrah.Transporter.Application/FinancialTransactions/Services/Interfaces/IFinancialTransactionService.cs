namespace Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;

public interface IFinancialTransactionService
{
    Task CreateTransaction(FinancialTransactionBuilderFinal builder);

    Task CreateTransaction(IEnumerable<FinancialTransactionBuilderFinal> builder);
}