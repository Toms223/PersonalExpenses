using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.Services.Interfaces;
using PersonalExpenses.ViewModel;

namespace PersonalExpenses.Controllers;

public class CategoryController(ICategoryService categoryService) : Controller
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(string name, string color, ExpensesView model)
    {
        if (!ValidColorValue(color))
        {
            model.Error = new Error{ Code = 400, Message = "Invalid Color"};
        }
        else
        {
            await categoryService.CreateCategory(name, color);
        }
        return RedirectToAction("Index", "Expenses", model);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int categoryId,  ExpensesView model)
    {
        await categoryService.DeleteCategory(categoryId);
        return RedirectToAction("Index", "Expenses", model);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddExpenseToCategory(int expenseId, int categoryId,  ExpensesView model)
    {
        await categoryService.AddExpenseToCategory(expenseId, categoryId);
        return RedirectToAction("Index", "Expenses", model);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveExpenseFromCategory(int expenseId, int categoryId,  ExpensesView model)
    {
        await categoryService.RemoveExpenseFromCategory(expenseId, categoryId);
        return RedirectToAction("Index", "Expenses", model);
    }

    private bool ValidColorValue(string color)
    {
        string hexadecimalValues = "0123456789ABCDEF";
        return color.FirstOrDefault(' ') == '#' && color.ToList().Skip(1).All(c => hexadecimalValues.Contains(c)) && color.Length == 7;
    }
}