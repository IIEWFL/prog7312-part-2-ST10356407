using System.ComponentModel.DataAnnotations;

namespace Municicpality.Models
{
    // Model representing a local event or announcement
    //mgoertz-msft (2024). Create models for your app - Visual Studio (Windows). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/visualstudio/modeling/create-models-for-your-app?view=vs-2022.
    public class LocalEvent
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        public DateTime EventDate { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        public string Organizer { get; set; } = string.Empty;
        
        public string ContactInfo { get; set; } = string.Empty;
        
        public bool IsAnnouncement { get; set; } // True for announcements, false for events
        
        public DateTime CreatedAt { get; set; }
        
        public int Priority { get; set; } // For priority queue implementation (1=highest, 5=lowest)
    }
}
