using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using PersonalExpenses.Model;
using PersonalExpenses.Services.Interfaces;

namespace PersonalExpenses.Services;

public class CalendarService(IAuthenticationProvider authProvider) : ICalenderService
{
    private readonly GraphServiceClient _graphClient = new(authProvider, "https://graph.microsoft.com/v1.0");

    public async Task<string?> AddFixedExpenseEventAsync(Expense expense)
    {
        if (!expense.Continuous)
            return null;

        string startDate = expense.Date.ToString("yyyy-MM-dd");
        string endDate = expense.Date.AddDays(1).ToString("yyyy-MM-dd");
        string subject = expense.Fixed ? $"{expense.Name} - {expense.Amount:C}" : $"{expense.Name}";
        Event evt = new Event
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = $"Fixed expense: {expense.Name}<br/>Amount: {expense.Amount:C}"
            },
            Start = new DateTimeTimeZone { DateTime = startDate, TimeZone = "UTC" },
            End = new DateTimeTimeZone { DateTime = endDate, TimeZone = "UTC" },
            IsAllDay = true,
            Recurrence = new PatternedRecurrence
            {
                Pattern = new RecurrencePattern
                {
                    Type = RecurrencePatternType.Daily,
                    Interval = expense.Period
                },
                Range = new RecurrenceRange
                {
                    Type = RecurrenceRangeType.NoEnd,
                    StartDate = new Date(expense.Date.Year, expense.Date.Month, expense.Date.Day)
                }
            }
        };

        Event? created = await _graphClient.Me.Events.PostAsync(evt);
        return created?.Id;
    }
}