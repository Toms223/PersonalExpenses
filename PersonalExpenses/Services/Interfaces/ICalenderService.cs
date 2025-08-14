using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface ICalenderService
{
    public Task<string?> AddFixedExpenseEventAsync(Expense expense);
}