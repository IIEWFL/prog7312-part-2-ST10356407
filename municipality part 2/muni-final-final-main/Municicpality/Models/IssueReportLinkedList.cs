namespace Municicpality.Models
{
    // Each issue report is stored as a node in our custom linked list
    //Microsoft Learn. (n.d.). LinkedList<T> Class (System.Collections.Generic). Retrieved 10 September 2025, from Microsoft Learn website: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-9.0
    public class IssueReportNode
    {
        public int Id { get; set; } // Unique ID for the report
        public string Location { get; set; } = string.Empty; // Where the issue is located
        public string Category { get; set; } = string.Empty; // Type of issue (Roads, Sanitation, etc.)
        public string Description { get; set; } = string.Empty; // Detailed description of the problem
        public string AttachmentPath { get; set; } = string.Empty; // Path to uploaded file if any
        public DateTime SubmittedAt { get; set; } // When the report was submitted
        public IssueReportNode? Next { get; set; } // Points to the next report in the list
    }

    // Our custom linked list that stores all issue reports in memory
    public class IssueReportLinkedList
    {
        private IssueReportNode? head; // First report in the list
        private int nextId = 1; // Keeps track of the next ID to assign

        // Lets other parts of the code see the first report
        public IssueReportNode? Head => head;

        // Adds a new report to the end of the list
        public IssueReportNode AddReport(string location, string category, string description, string attachmentPath)
        {
            var newNode = new IssueReportNode
            {
                Id = nextId++, // Give it the next available ID
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath,
                SubmittedAt = DateTime.Now // Record when it was submitted
            };

            // If this is the first report, make it the head
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                // Otherwise, find the last report and add this one after it
                var current = head;
                while (current.Next != null) current = current.Next;
                current.Next = newNode;
            }

            return newNode;
        }

        // Goes through all reports in the list one by one
        public IEnumerable<IssueReportNode> GetAll()
        {
            var current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        // Removes all reports from the list
        public void Clear()
        {
            head = null; // This removes all reports
            nextId = 1; // Start ID counter over
        }
    }
}
