using Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.FinancialTransactions.Services;

public class FinancialTransactionBuilder
{
    protected FinancialTransaction FinancialTransaction = new();

    public FinancialTransactionBuilder(long personId, decimal amount)
    {
        FinancialTransaction.PersonId = personId;
        FinancialTransaction.Amount = amount;
    }

    protected FinancialTransactionBuilder(FinancialTransaction financialTransaction)
    {
        FinancialTransaction = financialTransaction;
    }

    public FinancialTransactionBuilderFinal WithTransactionType(FinancialTransactionType transactionType)
    {
        FinancialTransaction.TransactionType = transactionType;
        return new FinancialTransactionBuilderFinal(FinancialTransaction);
    }

    public FinancialTransactionBuilderFinal WithRefrenceId(Guid refrenceId)
    {
        FinancialTransaction.RefrenceId = refrenceId;
        return new FinancialTransactionBuilderFinal(FinancialTransaction);
    }

    public FinancialTransactionBuilderFinal WithDescription(string description)
    {
        FinancialTransaction.Description = description;
        return new FinancialTransactionBuilderFinal(FinancialTransaction);
    }
}

public sealed class FinancialTransactionBuilderFinal(FinancialTransaction financialTransaction) : FinancialTransactionBuilder(financialTransaction), IFinancialTransactionBuilder
{
    public FinancialTransaction Build()
    {
        return FinancialTransaction;
    }
}