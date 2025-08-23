using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface ICategoryService
{
    public Task<Category> CreateCategory(string name, string color);
    public Task<bool> DeleteCategory(int categoryId);
    
    public Task<Category> GetCategory(int categoryId);
    public Task<bool> AddExpenseToCategory(int categoryId, int expenseId);
    public Task<bool> RemoveExpenseFromCategory(int categoryId, int expenseId);
}