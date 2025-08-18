using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;
using PersonalExpenses.ViewModel;
using PersonalExpenses.ViewModel.Errors;

namespace PersonalExpenses.Controllers;

public class ExpensesController(IExpensesService expenseService, ICalenderService calenderService) : Controller
{
     [Authorize]
     public async Task<ActionResult> Index(ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          if (model.Limit == 0)
          {
               model.Limit = 10;
          }
          try
          {
               List<Expense> expenses;
               if (model is { StartDate: not null, EndDate: not null })
               {
                    expenses = await expenseService.GetAllExpensesByDateAsync((DateOnly)model.StartDate, (DateOnly)model.EndDate, model.ContinuousView, userId);
               }
               else if (model.EndDate != null)
               {
                    expenses = await expenseService.GetAllExpensesUntilDateAsync((DateOnly)model.EndDate, model.ContinuousView, userId);
               }
               else if (model.StartDate != null)
               {
                    expenses = await expenseService.GetAllExpensesFromDateAsync((DateOnly)model.StartDate, model.ContinuousView, userId);
               }
               else
               {
                    expenses = await expenseService.GetAllExpensesAsync(model.Offset, model.Limit, model.ContinuousView, userId);
               }
               Func<List<Expense>, List<Expense>> order = GetOrderLambda(model.OrderBy);
               model.Expenses = order(expenses);
          }
          catch (Exception ex)
          {
               model.Error = ex.ToError();
          }
          return View(model);
     }

     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddExpense(string name, float amount, DateOnly date, ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          await expenseService.CreateExpense(name, amount, date, userId);
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddContinuousExpense(string name, float amount, DateOnly date, int period, bool fixedExpense, ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          await expenseService.CreateContinuousExpense(name, amount, date, period, fixedExpense, userId);
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> EditExpense(int id, string? name, float? amount, DateOnly? date, ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          await expenseService.UpdateExpense(id, name, amount, date, userId);
          model.EditId = -1;
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> EditContinuousExpense(int id, string? name, float? amount, DateOnly? date, int? period, bool? fixedExpense, ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          await expenseService.UpdateContinuousExpense(id, name, amount, date, period, fixedExpense, userId);
          model.EditId = -1;
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> DeleteExpense(int id, ExpensesView model)
     {
          var userIdClaim = User.FindFirst("UserId")?.Value;
          var userId = int.Parse(userIdClaim);
          await expenseService.DeleteExpenseAsync(id,  userId);
          model.EditId = -1;
          return RedirectToAction(nameof(Index), model);
     }
     
     [HttpPost]
     [Authorize]
     public async Task<ActionResult> AddToCalender(int id, ExpensesView model)
     {
          Expense? expense = await expenseService.GetExpense(id);
          if (expense != null)
          {
               await calenderService.AddFixedExpenseEventAsync(expense);
               TempData["SuccessMessage"] = $"Expense '{expense.Name}' was added to Outlook Calendar successfully.";
          }
          else
          {
               TempData["SuccessMessage"] = $"Failed to add to Outlook Calendar.";
          }
          
          return RedirectToAction(nameof(Index), model);
     }

     private Func<List<Expense>, List<Expense>> GetOrderLambda(OrderBy orderBy)
     {
          return orderBy switch
          {
               OrderBy.DateAscending => (expenses) => expenses.OrderBy(expense => expense.Date).ToList(),
               OrderBy.DateDescending => (expenses) => expenses.OrderByDescending(expense => expense.Date).ToList()
          };
     }

     
}