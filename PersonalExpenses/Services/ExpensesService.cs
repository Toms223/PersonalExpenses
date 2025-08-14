using Microsoft.EntityFrameworkCore;
using PersonalExpenses.Data;
using PersonalExpenses.Exceptions;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;

namespace PersonalExpenses.Services;

public class ExpensesService: IExpensesService
{
    private readonly ExpensesDbContext _context;

    public ExpensesService(ExpensesDbContext context)
    {
        _context = context;
    }
    
    public Task<Expense> CreateExpense(string name, float amount, DateOnly date, int userId)
    {
        Expense expense = new Expense();
        expense.Name = name;
        expense.Amount = amount;
        expense.Date = date;
        expense.UserId =  userId;
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return Task.FromResult(expense);
        
    }

    public async Task<Expense?> GetExpense(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public Task<Expense> CreateContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, int userId)
    {
        Expense expense = new Expense();
        expense.Name = name;
        expense.Amount = amount;
        expense.Date = date;
        expense.Period = period;
        expense.Continuous = true;
        expense.UserId =  userId;
        expense.Fixed = fixedExpense;
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return Task.FromResult(expense);
    }

    public Task<Expense> UpdateExpense(int id, string? name, float? amount, DateOnly? date, int userId)
    {
        Expense? expense = _context.Expenses.Find(id);
        if (userId != expense?.UserId) throw new ExpenseNotFoundException();
        if (expense == null) throw new ExpenseNotFoundException();
        if (name != null) expense.Name = name;
        if (amount != null) expense.Amount = (float)amount;
        if (date != null) expense.Date = (DateOnly)date;
        _context.SaveChanges();
        return Task.FromResult(expense);
    }

    public Task<bool> DeleteExpenseAsync(int id, int userId)
    {
        Expense? expense = _context.Expenses.Find(id);
        if (userId != expense?.UserId) throw new ExpenseNotFoundException();
        if (expense == null) throw new ExpenseNotFoundException();
        _context.Expenses.Remove(expense);
        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<List<Expense>> GetAllExpensesAsync(int offset, int limit, bool continuous, int userId)
    {
        return _context.Expenses.AsNoTracking().Where(e => e.UserId == userId).Skip(offset).Take(limit).ToListAsync();
    }

    public Task<List<Expense>> GetAllExpensesByDateAsync(DateOnly startDate, DateOnly endDate, bool continuous, int userId)
    {
        return _context.Expenses.AsNoTracking().Where(e => e.Date >= startDate && e.Date <= endDate && e.UserId == userId && e.Continuous == continuous).ToListAsync();
    }
    
    public Task<List<Expense>> GetAllExpensesFromDateAsync(DateOnly startDate, bool continuous, int userId)
    {
        return _context.Expenses.AsNoTracking().Where(e => e.Date >= startDate && e.UserId == userId&& e.Continuous == continuous).ToListAsync();
    }
    
    public Task<List<Expense>> GetAllExpensesUntilDateAsync(DateOnly endDate, bool continuous, int userId)
    {
        return _context.Expenses.AsNoTracking().Where(e => e.Date <= endDate && e.UserId == userId&& e.Continuous == continuous).ToListAsync();
    }

    public Task<Expense> UpdateContinuousExpense(int id, string? name, float? amount, DateOnly? date, int? period, bool? fixedExpense,
        int userId)
    {
        Expense? expense = _context.Expenses.Find(id);
        if (userId != expense?.UserId) throw new ExpenseNotFoundException();
        if (expense == null) throw new ExpenseNotFoundException();
        if (name != null) expense.Name = name;
        if (amount != null) expense.Amount = (float)amount;
        if (date != null) expense.Date = (DateOnly)date;
        if (period != null) expense.Period = (int)period;
        if (fixedExpense != null) expense.Fixed = (bool)fixedExpense;
        _context.SaveChanges();
        return Task.FromResult(expense);
    }
}