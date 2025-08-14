using PersonalExpenses.Model;

namespace PersonalExpenses.ViewModel;

public class ExpensesView
{
    public List<Expense>? Expenses { get; set; }
    public Error? Error { get; set; }
    public OrderBy OrderBy { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    
    public bool ContinuousView { get; set; }
    public int EditId { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    
    public bool MonthView { get; set; }
    
    public int SelectedYear { get; set; }
    public int SelectedMonth { get; set; }

    public List<MonthlyExpenses>? MonthlyExpenses
    {
        get
        {
            return Expenses?.GroupBy(e => new {e.Date.Year, e.Date.Month}).Select(g => new MonthlyExpenses
            {
                MonthOfYear = (MonthOfYear)g.Key.Month - 1,
                Year = g.Key.Year,
                Expenses = g.ToList()
            }).OrderBy(e => e.Year).ThenBy(e => e.MonthOfYear).ToList();
        }
    }
}