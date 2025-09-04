using Microsoft.EntityFrameworkCore;
using PersonalExpenses.Data;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;

namespace PersonalExpenses.Services;

public class CategoryService : ICategoryService
{
    private ExpensesDbContext _context;
    
    public CategoryService(ExpensesDbContext context)
    {
        _context = context;
    }
    public async Task<Category> CreateCategory(string name, string color, int userId)
    {
        Category category = new Category { Name = name, Color = color, UserId = userId };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategory(int categoryId, int userId)
    {
        Category? category = await _context.Categories.FindAsync(categoryId);
        if (category == null || category.UserId != userId)
        {
            return false;
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Category>> GetUserCategories(int userId)
    {
        return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
    }
}