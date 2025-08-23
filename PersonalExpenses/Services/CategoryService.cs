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
    public Task<Category> CreateCategory(string name, string color)
    {
        Category category = new Category { Name = name, Color = color };
        _context.Categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<bool> DeleteCategory(int categoryId)
    {
        Category? category = _context.Categories.Find(categoryId);
        if (category == null)
        {
            return Task.FromResult(false);
        }
        _context.Categories.Remove(category);
        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public async Task<Category> GetCategory(int categoryId)
    {
        return await _context.Categories.FindAsync(categoryId) ?? throw new Exception("Category not found");
    }

    public Task<bool> AddExpenseToCategory(int categoryId, int expenseId)
    {
        Expense? expense = _context.Expenses.Find(expenseId);
        if (expense == null)
        {
            return Task.FromResult(false);
        }
        Category? category = _context.Categories.Find(categoryId);
        if (category == null || expense.CategoryId != 0)
        {
            return Task.FromResult(false);
        }
        expense.CategoryId = categoryId;
        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<bool> RemoveExpenseFromCategory(int categoryId, int expenseId)
    {
        Expense? expense = _context.Expenses.Find(expenseId);
        if (expense == null) return Task.FromResult(false);
        
        Category? category = _context.Categories.Find(categoryId);
        if (category == null) return Task.FromResult(false);
        
        if(!_context.Expenses.Any(e => e.Id == expenseId)) return Task.FromResult(false);
        expense.CategoryId = 0;
        _context.SaveChanges();
        return Task.FromResult(true);
    }
}