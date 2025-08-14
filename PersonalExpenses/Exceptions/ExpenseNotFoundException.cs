namespace PersonalExpenses.Exceptions;

public class ExpenseNotFoundException: Exception
{
    public override string Message { get; } = "Expense not found";
}