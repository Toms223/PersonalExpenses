using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;
using PersonalExpenses.ViewModel;
using PersonalExpenses.ViewModel.Errors;

namespace PersonalExpenses.Controllers;

public class ExpensesController(IExpensesService expenseService, ICalenderService calenderService, ICategoryService categoryService, IUserService userService) : Controller
{
     [Authorize]
     public async Task<ActionResult> Index(ExpensesView model)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          return View(await HydrateExpensesView(model, userId));
     }

     [Authorize]
     public async Task<ActionResult> Detailed(ExpensesView model)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          return View(await HydrateExpensesView(model, userId));
     }

     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddExpense(string name, float amount, DateOnly date, int? categoryId, ExpensesView model)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          await expenseService.CreateExpense(name, amount, date, categoryId, userId);
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, int? categoryId, ExpensesView model)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          await expenseService.CreateContinuousExpense(name, amount, date, period, fixedExpense, categoryId, userId);
          return RedirectToAction(nameof(Index), model);
     }

     [HttpPost]
     [Authorize]
     public async Task<ActionResult> EditExpense(int id, string? name, float? amount, DateOnly? date, bool? continuous,
          bool? fixedExpense, int? period, int? categoryId)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }

          int userId = int.Parse(userIdClaim);
          await expenseService.UpdateExpense(id, name, amount, date, continuous, fixedExpense, period, categoryId, userId);
          return RedirectToAction(nameof(Index));
     }

     [HttpPost]
     [Authorize]
     public async Task<ActionResult> DeleteExpense(int id, ExpensesView model)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          await expenseService.DeleteExpense(id,  userId);
          model.EditId = -1;
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddToCalendar(int id)
     {
          Expense? expense = await expenseService.GetExpense(id);
          if (expense != null)
          {
               await calenderService.AddFixedExpenseEventAsync(expense);
               TempData["SuccessMessage"] = $"Expense '{expense.Name}' was added to Outlook Calendar successfully.";
          }
          else
          {
               TempData["SuccessMessage"] = "Failed to add to Outlook Calendar.";
          }
          
          return RedirectToAction(nameof(Index));
     }

     private async Task<ExpensesView> HydrateExpensesView(ExpensesView model, int userId)
     {
          try
          {
               if (model.User.Id == 0)
               {
                    model.User = await userService.GetUserById(userId);
               }

               if (model.Year == 0 || model.Month == 0)
               {
                    model.Year = DateTime.Now.Year;
                    model.Month = DateTime.Now.Month;
               }
               DateOnly currentDate = DateOnly.Parse($"{model.Year}-{model.Month}-1");
               List<Expense> currentMonthExpenses =
                    await expenseService.GetExpensesByMonthAndYear(userId, currentDate.Month, currentDate.Year);
               model.CurrentMonthExpenses = currentMonthExpenses;
               DateOnly previousDate = currentDate.AddMonths(-1);
               List<Expense> previousMonthExpenses =
                    await expenseService.GetExpensesByMonthAndYear(userId, previousDate.Month, previousDate.Year);
               model.PreviousMonthExpenses = previousMonthExpenses;

               List<int> currentMonthCategoryIds = currentMonthExpenses
                    .Where(e => e.CategoryId != null)
                    .Select(e => (int)e.CategoryId)
                    .Distinct()
                    .ToList();
               
               model.CurrentMonthCategories = await categoryService.GetUserCategories(userId);
          }
          catch (Exception ex)
          {
               model.Error = ex.ToError();
          }
          return model;
     }
}