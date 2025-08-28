using PersonalExpenses.Model;

namespace PersonalExpenses.ViewModel;

public class ExpensesView
{
    public List<Expense> CurrentMonthExpenses { get; set; } = [];

    public int CurrentMonthTotal
    {
        get
        {
            float currentTotal = CurrentMonthExpenses.Sum(e => e.Amount);
            float upToSum = 0;
            foreach (var expense in CurrentMonthExpenses)
            {
                if(!expense.Continuous || !expense.Fixed) continue;
                DateOnly date = expense.Date;
                while (date < DateOnly.FromDateTime(DateTime.Now))
                {
                    upToSum += expense.Amount;
                    date = date.AddDays(expense.Period);
                }
            }
            return (int)Math.Round(currentTotal + upToSum);
        }
    }

    public int ExpectedMonthTotal
    {
        get
        {
            float currentSum = CurrentMonthTotal;
            float expectedSum = 0;
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            foreach (var expense in CurrentMonthExpenses)
            {
                if(!expense.Continuous || !expense.Fixed) continue;
                DateOnly date = expense.Date.AddDays(expense.Period);
                while (date.Month == currentDate.Month && date.Year == currentDate.Year)
                {
                    expectedSum += expense.Amount;
                    date = date.AddDays(expense.Period);
                }
            }
            return (int)Math.Round(currentSum + expectedSum);
        }
    }

    public int PreviousMonthTotal
    {
        get { float currentTotal = PreviousMonthExpenses.Sum(e => e.Amount);
            float upToSum = 0;
            foreach (var expense in PreviousMonthExpenses)
            {
                if(!expense.Continuous || !expense.Fixed) continue;
                DateOnly date = expense.Date;
                while (date < DateOnly.FromDateTime(DateTime.Now))
                {
                    upToSum += expense.Amount;
                    date = date.AddDays(expense.Period);
                }
            }
            return (int)Math.Round(currentTotal + upToSum); }
    }

    public int RemainingBudget => User.Limit - CurrentMonthTotal;

    public int PreviousMonthDifference
    {
        get
        {
            if (CurrentMonthTotal == 0) return 0;
            if (PreviousMonthTotal == 0) return 0;
            if (PreviousMonthTotal == CurrentMonthTotal) return 0;
            int difference = (int)Math.Round((float)CurrentMonthTotal / PreviousMonthTotal * 100);
            return PreviousMonthTotal > CurrentMonthTotal ? difference - 100 : difference;
        }
    }
    public List<Category> CurrentMonthCategories { get; set; } = [];
    public List<Expense> PreviousMonthExpenses { get; set; } = [];
    
    public User User { get; set; } =  new User();
    public Error? Error { get; set; }
    public int Month { get; set; }
    
    public int Year { get; set; }
    public int EditId { get; set; }
}