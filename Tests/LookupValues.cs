namespace Tests;

public static class LookupValues
{
    public static readonly Priority LowPriority = new() { Id = 1, Name = "Low" };
    public static readonly Priority MediumPriority = new() { Id = 2, Name = "Medium" };
    public static readonly Priority HighPriority = new() { Id = 3, Name = "High" };

    public static readonly State PendingState = new() { Id = 1, Name = "Pending" };
    public static readonly State InProgressState = new() { Id = 2, Name = "InProgress" };
    public static readonly State ComplemetedState = new() { Id = 3, Name = "Completed" };
}
