using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface ICategoryService
{
    public Task<Category> CreateCategory(string name, string color, int userId);
    public Task<bool> DeleteCategory(int categoryId, int userId);

    public Task<List<Category>> GetUserCategories(int userId);
}