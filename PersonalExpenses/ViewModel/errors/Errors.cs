using PersonalExpenses.Exceptions;

namespace PersonalExpenses.ViewModel.Errors;

public static class Errors
{
    public static Error ToError(this Exception ex)
    {
        return ex switch
        {
            ExpenseNotFoundException expenseNotFoundException => new Error{Code = 404, Message = expenseNotFoundException.Message},
            _ => new Error{Code = 500, Message = ex.Message}
        };
    }
}