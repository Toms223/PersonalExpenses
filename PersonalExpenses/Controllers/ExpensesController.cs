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
     public async Task<ActionResult> AddExpense(string name, float amount, DateOnly date, int? categoryId)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          await expenseService.CreateExpense(name, amount, date, categoryId, userId);
          string referer = Request.Headers["Referer"].ToString();
          if (!string.IsNullOrEmpty(referer))
          {
               Uri uri = new Uri(referer);
               string path = uri.AbsolutePath;
               string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
               
               string actionName = segments.Length > 1 ? segments[1] : segments[0];

               return RedirectToAction(actionName);
          }
          return RedirectToAction(nameof(Index));
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, int? categoryId)
     {
          string? userIdClaim = User.FindFirst("UserId")?.Value;
          if (userIdClaim == null)
          {
               await HttpContext.SignOutAsync();
               return RedirectToAction("Index", "Home");
          }
          int userId = int.Parse(userIdClaim);
          await expenseService.CreateContinuousExpense(name, amount, date, period, fixedExpense, categoryId, userId);
          string referer = Request.Headers["Referer"].ToString();
          if (!string.IsNullOrEmpty(referer))
          {
               Uri uri = new Uri(referer);
               string path = uri.AbsolutePath;
               string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
               
               string actionName = segments.Length > 1 ? segments[1] : segments[0];

               return RedirectToAction(actionName);
          }
          return RedirectToAction(nameof(Index));
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
          string referer = Request.Headers["Referer"].ToString();
          if (!string.IsNullOrEmpty(referer))
          {
               Uri uri = new Uri(referer);
               string path = uri.AbsolutePath;
               string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
               
               string actionName = segments.Length > 1 ? segments[1] : segments[0];

               return RedirectToAction(actionName);
          }
          return RedirectToAction(nameof(Index));
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
          string referer = Request.Headers["Referer"].ToString();
          if (!string.IsNullOrEmpty(referer))
          {
               Uri uri = new Uri(referer);
               string path = uri.AbsolutePath;
               string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
               
               string actionName = segments.Length > 1 ? segments[1] : segments[0];

               return RedirectToAction(actionName);
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
               model.CurrentMonthCategories = await categoryService.GetUserCategories(userId);
          }
          catch (Exception ex)
          {
               model.Error = ex.ToError();
          }
          return model;
     }
}