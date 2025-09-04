using PersonalExpenses.Model;

namespace PersonalExpenses.ViewModel;

public class UserView
{
    public string Username {get;set;} = "";
    public string Email {get;set;} = "";
    public int Limit { get; set; } = 0;
    public List<Category> UserCategories { get; set; } = [];

}