using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface IUserService
{
    public Task<User> GetUserById(int id);
    
}