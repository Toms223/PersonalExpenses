using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;
using PersonalExpenses.ViewModel;

namespace PersonalExpenses.Controllers;

public class UserController(IUserService userService, ICategoryService categoryService): Controller
{
    [Authorize]
    public async Task<ActionResult> Index(UserView model)
    {
        string? userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        int userId = int.Parse(userIdClaim);
        User user = await userService.GetUserById(userId);
        List<Category> categories = await categoryService.GetUserCategories(userId);
        model.Username = user.Name;
        model.Email = user.Email;
        model.Limit = user.Limit;
        model.UserCategories = categories;
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> EditBudgetLimit(int limit)
    {
        string? userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        int userId = int.Parse(userIdClaim);
        await userService.EditLimit(userId, limit);
        return RedirectToAction("Index");
    }
}