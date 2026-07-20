namespace CashFlow.Domain.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}