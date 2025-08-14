using Microsoft.AspNetCore.Mvc;
using PersonalExpenses.ViewModel;

namespace PersonalExpenses.Helpers.Expenses;

public static class ExpenseUrlHelper
{
    public static string GenerateExpenseLink(this IUrlHelper url, string controller, string action, ExpensesView model, object newValues)
    {
        RouteValueDictionary routeValues = new RouteValueDictionary();
        typeof(ExpensesView).GetProperties().ToList().ForEach(property =>
        {
            routeValues.Add(property.Name, property.GetValue(model));
        });
        newValues.GetType().GetProperties().ToList().ForEach(property =>
            {
                routeValues[property.Name] = property.GetValue(newValues);
            }
        );
        return url.Action(action, controller, routeValues) ?? "";
    }
}