using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.Services.Interfaces;
using PersonalExpenses.ViewModel;

namespace PersonalExpenses.Controllers;

public class CategoryController(ICategoryService categoryService) : Controller
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(string name, string color)
    {
        string? userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        int userId = int.Parse(userIdClaim);
        if (ValidColorValue(color.ToUpper()))
        {
            await categoryService.CreateCategory(name, color.ToUpper(), userId);
        }
        return RedirectToAction("Index", "User");
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        string? userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        int userId = int.Parse(userIdClaim);
        await categoryService.DeleteCategory(id, userId);
        return RedirectToAction("Index", "User");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditCategory(int id, string? name, string? color)
    {
        string? userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        int userId = int.Parse(userIdClaim);
        await categoryService.EditCategory(id, name, color, userId);
        return RedirectToAction("Index", "User");
    }

    private bool ValidColorValue(string color)
    {
        string hexadecimalValues = "0123456789ABCDEF";
        return color.ToList().All(c => hexadecimalValues.Contains(c)) && color.Length == 6;
    }
}