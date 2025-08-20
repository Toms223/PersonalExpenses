using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PersonalExpenses.ViewModel;

namespace PersonalExpenses.Helpers.Expenses;

public static class ExpenseHelper
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
    public static Task<IHtmlContent> WithExcludedHiddenFields(this IHtmlHelper helper, ExpensesView model, params string[] excludedFields)
    {

        ViewDataDictionary excludedFieldsDictionary = new ViewDataDictionary(helper.ViewData)
        {
            { "ExcludeFields", new HashSet<string>(excludedFields)}
        };
        return helper.PartialAsync("_StateHiddenFields", model, excludedFieldsDictionary);
    }
}