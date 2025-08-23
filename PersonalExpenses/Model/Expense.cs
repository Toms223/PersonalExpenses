using PersonalExpenses.Services;

namespace PersonalExpenses.Model;


public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Amount { get; set; }
    public DateOnly Date { get; set; }
    
    public int UserId { get; set; }
    
    public int CategoryId { get; set; }
    
    public bool Continuous { get; set; }
    
    public bool Fixed {get; set;}
    
    public int Period { get; set; }

    public DateOnly NextPayment()
    {
        return Date.AddDays(Period);
    }
}