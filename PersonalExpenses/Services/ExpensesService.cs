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
    
    public Task<Expense> CreateExpense(string name, float amount, DateOnly date, int? categoryId, int userId)
    {
        Expense expense = new Expense();
        expense.Name = name;
        expense.Amount = amount;
        expense.Date = date;
        expense.UserId =  userId;
        expense.CategoryId = categoryId;
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return Task.FromResult(expense);
        
    }

    public async Task<Expense?> GetExpense(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public async Task<List<Expense>> GetExpensesByMonthAndYear(int userId, int month, int year)
    {
        return await _context.Expenses.Where(x => x.UserId == userId && x.Date.Month == month && x.Date.Year == year).ToListAsync();
    }

    public async Task<Expense> CreateContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, int? categoryId, int userId)
    {
        Expense expense = new Expense();
        expense.Name = name;
        expense.Amount = amount;
        expense.Date = date;
        expense.Period = period;
        expense.Continuous = true;
        expense.UserId =  userId;
        expense.Fixed = fixedExpense;
        expense.CategoryId = categoryId;
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense> UpdateExpense(int id, string? name, float? amount, DateOnly? date, int userId)
    {
        Expense? expense = _context.Expenses.Find(id);
        if (userId != expense?.UserId) throw new ExpenseNotFoundException();
        if (expense == null) throw new ExpenseNotFoundException();
        if (name != null) expense.Name = name;
        if (amount != null) expense.Amount = (float)amount;
        if (date != null) expense.Date = (DateOnly)date;
        await _context.SaveChangesAsync();
        return expense;
    }
    
    public async Task<Expense> UpdateContinuousExpense(int id, string? name, float? amount, DateOnly? date, int? period, bool? fixedExpense,
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
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteExpense(int id, int userId)
    {
        Expense? expense = _context.Expenses.Find(id);
        if (userId != expense?.UserId) throw new ExpenseNotFoundException();
        if (expense == null) throw new ExpenseNotFoundException();
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }
    
}