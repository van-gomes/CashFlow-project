namespace CashFlow.Domain.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(Entities.User user);
}