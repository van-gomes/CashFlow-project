namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
}