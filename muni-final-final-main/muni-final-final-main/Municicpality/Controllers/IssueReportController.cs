using Microsoft.AspNetCore.Mvc;
using Municicpality.Models;
using Municicpality.Services;

namespace Municicpality.Controllers

////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
{
    // Controller to manage issue report creation and persistence using linked list
    public class IssueReportController : Controller
    {
        private readonly IssueReportService _reportService;
        private readonly IWebHostEnvironment _env;

        // Constructor injection for service and hosting environment
        public IssueReportController(IssueReportService reportService, IWebHostEnvironment env)
        {
            _reportService = reportService; // Linked list service for in-memory storage
            _env = env; // Hosting environment for file uploads
        }

        // GET: IssueReport/Create
        public IActionResult Create() => View(); // Simple Create form view

        // POST: IssueReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form, IFormFile? attachment)
        {
            // Extract form data
            string location = form["Location"].ToString() ?? string.Empty;
            string category = form["Category"].ToString() ?? string.Empty;
            string description = form["Description"].ToString() ?? string.Empty;
            string? attachmentPath = null;

            // Handle file upload if present
            if (attachment != null && attachment.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads); // Ensure folder exists

                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream); // Save file to server
                }

                attachmentPath = "/uploads/" + fileName; // Relative path for frontend
            }

            // Add issue to in-memory linked list
            var reportNode = _reportService.AddReport(location, category, description, attachmentPath ?? string.Empty);

            TempData["Message"] = "Issue reported successfully!"; // User feedback
            return RedirectToAction(nameof(Create)); // Reload create form
        }
    }
}
