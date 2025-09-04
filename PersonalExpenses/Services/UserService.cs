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

    public async Task<User> EditLimit(int id, int limit)
    {
        User user = await _context.Users.FindAsync(id) ?? throw new Exception("User not found");
        user.Limit = limit;
        await _context.SaveChangesAsync();
        return user;
    }
}