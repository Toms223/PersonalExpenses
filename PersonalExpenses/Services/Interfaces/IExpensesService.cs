using PersonalExpenses.Model;

namespace PersonalExpenses.Services.Interfaces;

public interface IExpensesService
{
    Task<Expense> CreateExpense(string name, float amount, DateOnly date, int? categoryId, int userId);
    
    Task<Expense?> GetExpense(int id);
    
    Task<Expense> CreateContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, int? categoryId, int userId);
    Task<Expense> UpdateExpense(int id, string? name, float? amount, DateOnly? date, int userId);
    
    Task<Expense> UpdateContinuousExpense(int id, string? name, float? amount, DateOnly? date, int? period, bool? fixedExpense, int userId);
    Task<bool> DeleteExpense(int id, int userId);
    
    Task<List<Expense>> GetExpensesByMonthAndYear(int userId, int month, int year);
    
}