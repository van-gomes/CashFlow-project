namespace CashFlow.Application.UseCases.Expenses.Register.Reports.Excel;

public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}