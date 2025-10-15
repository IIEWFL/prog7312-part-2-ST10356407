namespace Municicpality.Models
{
    // Model to track event viewing history for data structure demonstration
    // mgoertz-msft (2023). Understanding Models, Classes and Relationships - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/understanding-models-classes-and-relationships?view=vs-2022.
    //mgoertz-msft (2024). Create models for your app - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/create-models-for-your-app?view=vs-2022.
    public class EventViewHistory
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserSession { get; set; } = string.Empty;
        public DateTime ViewedAt { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string EventCategory { get; set; } = string.Empty;
    }
}

