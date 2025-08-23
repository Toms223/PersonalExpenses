using PersonalExpenses.Data;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;

namespace PersonalExpenses.Services;

public class UserService : IUserService
{
    private ExpensesDbContext _context;
    
    public UserService(ExpensesDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id) ?? throw new Exception("User not found");
    }
}