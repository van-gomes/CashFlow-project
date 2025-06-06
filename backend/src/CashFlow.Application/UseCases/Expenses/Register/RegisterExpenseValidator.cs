using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessages.titleRequired_);
        RuleFor(expense => expense.Amount).GreaterThan(0)
            .WithMessage(ResourceErrorMessages.amountMustBeGreaterThanZero);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.expensesCannotForTheFuture);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.paymentTypeInvalid);
    }
}