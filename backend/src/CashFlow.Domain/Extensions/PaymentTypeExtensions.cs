using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessages.cach,
            PaymentType.CreditCard => ResourceReportGenerationMessages.creditCard,
            PaymentType.DebitCard => ResourceReportGenerationMessages.debitCard,
            PaymentType.EletronicTransfer => ResourceReportGenerationMessages.eletronicTransfer,
            _ => string.Empty
        };
    }
}