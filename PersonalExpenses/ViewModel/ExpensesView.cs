using PersonalExpenses.Model;

namespace PersonalExpenses.ViewModel;

public class ExpensesView
{
    public List<Expense> CurrentMonthExpenses { get; set; } = [];

    public int CurrentMonthTotal
    {
        get { return (int)Math.Round(CurrentMonthExpenses.Sum(e => e.Amount)); }
    }
    
    public int PreviousMonthTotal
    {
        get { return (int)Math.Round(PreviousMonthExpenses.Sum(e => e.Amount)); }
    }

    public int RemainingBudget => User.Limit - CurrentMonthTotal;

    public int PreviousMonthDifference
    {
        get
        {
            if (CurrentMonthTotal == 0) return 0;
            if (PreviousMonthTotal == 0) return 0;
            if (PreviousMonthTotal > CurrentMonthTotal)
            {
                return -1 * (int)Math.Round((float)CurrentMonthTotal / PreviousMonthTotal * 100);
            }

            if (PreviousMonthTotal == CurrentMonthTotal)
            {
                return 0;
            }
            return (int)Math.Round((float)PreviousMonthTotal / CurrentMonthTotal * 100);
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