namespace CashFlow.Application.UseCases.Expenses.Register.Reports.PDF;

public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}