namespace Municicpality.Models
{
    // Model for storing user category preferences with time decay and trending analysis
    // mgoertz-msft (2023). Understanding Models, Classes and Relationships - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/understanding-models-classes-and-relationships?view=vs-2022.
    //mgoertz-msft (2024). Create models for your app - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/create-models-for-your-app?view=vs-2022.
    public class UserCategoryPreference
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime LastSearch { get; set; }
        public double TimeDecayScore { get; set; }
        public double TrendingScore { get; set; }
    }
}
