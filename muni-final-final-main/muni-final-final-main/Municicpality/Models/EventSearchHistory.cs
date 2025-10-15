namespace Municicpality.Models
{
    // Model to track user search patterns for recommendation feature
    // mgoertz-msft (2023). Understanding Models, Classes and Relationships - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/understanding-models-classes-and-relationships?view=vs-2022.
    //mgoertz-msft (2024). Create models for your app - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/create-models-for-your-app?view=vs-2022.
    public class EventSearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime SearchDate { get; set; }
        public string UserSession { get; set; } = string.Empty; // Simple session tracking
    }
}
