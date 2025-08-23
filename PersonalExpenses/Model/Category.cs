namespace PersonalExpenses.Model;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Color { get; set; }
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public float Total
    {
        get
        {
            return Expenses.Sum(e => e.Amount);
        }
    }
}