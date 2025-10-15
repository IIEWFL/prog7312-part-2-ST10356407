namespace Municicpality.Models
{
    // Model for storing trending category information with multiple time windows
    // mgoertz-msft (2023). Understanding Models, Classes and Relationships - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/understanding-models-classes-and-relationships?view=vs-2022.
    // mgoertz-msft (2024). Create models for your app - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/create-models-for-your-app?view=vs-2022.
    public class TrendingCategory
    {
        public string Category { get; set; } = string.Empty;
        public int SearchCount { get; set; }
        public double TrendingScore { get; set; }
        public string TimeWindow { get; set; } = string.Empty;
    }
}
