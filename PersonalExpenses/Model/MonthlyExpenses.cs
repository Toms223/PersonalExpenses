namespace PersonalExpenses.Model;

public class MonthlyExpenses
{
    public MonthOfYear MonthOfYear { get; set; }
    public int Year { get; set; }
    public List<Expense> Expenses { get; set; }

    public float TotalSpent
    {
        get
        {
            return Expenses.Sum(expense =>
            {
                if (expense.Fixed)
                {
                    DateOnly date = expense.Date;
                    float amount = 0;
                    while (date < DateOnly.FromDateTime(DateTime.Now))
                    {
                        date = date.AddDays(expense.Period);
                        amount += expense.Amount;
                    }
                    return amount;
                }
                return expense.Amount;
                
            });
        }
    }

    public float ExpectedMonthTotal
    {
        get
        {
            return Expenses.Sum(expense =>
            {
                if (expense.Fixed)
                {
                    DateOnly date = expense.Date;
                    float amount = 0;
                    while ((MonthOfYear)date.Month - 1 == MonthOfYear)
                    {
                        date = date.AddDays(expense.Period);
                        amount += expense.Amount;
                    }
                    return amount;
                }
                return expense.Amount;
            });
        }
    }
}