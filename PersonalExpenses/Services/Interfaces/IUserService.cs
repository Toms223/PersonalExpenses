using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface IUserService
{
    public Task<User> GetUserById(int id);

    public Task<User> EditLimit(int id, int limit);

}