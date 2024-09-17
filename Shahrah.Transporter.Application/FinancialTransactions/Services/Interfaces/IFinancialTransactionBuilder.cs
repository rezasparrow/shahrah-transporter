using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;

public interface IFinancialTransactionBuilder
{
    FinancialTransaction Build();
}