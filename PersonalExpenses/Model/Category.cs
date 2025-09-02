namespace PersonalExpenses.Model;

public class Category
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public string Name { get; set; }
    
    public string Color { get; set; }
}