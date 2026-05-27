namespace CashFlow.Domain.User;

public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}