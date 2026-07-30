using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;
    private string _token;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

    }

    public string GetName() => _user.Name;

    public string GetEmail() => _user.Email;

    public string GetPassword() => _password;

    public string GetToken() => _token;

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        AddUsers(dbContext, passwordEncripter);
        AddExpenses(dbContext, _user);

        dbContext.SaveChanges();
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;

        _user.Password = passwordEncripter.Encrypt(_user.Password);

        dbContext.Users.Add(_user);
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(expense);
    }
}