namespace PersonalExpenses.ViewModel;

public class Error
{
    public int Code { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return $"Error code: {Code}, Message: {Message}";
    }
}