using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Municicpality.Models;
using Municicpality.Services;
using System.Diagnostics;

namespace Municicpality.Controllers
{
    ////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    // HomeController manages the main portal views and issue reporting
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IssueReportService _reportService;

        // Constructor injection for logger, environment, and service
        public HomeController(
            ILogger<HomeController> logger,
            IWebHostEnvironment env,
            IssueReportService reportService)
        {
            _logger = logger; // Logger reference
            _env = env; // Web host environment for uploads
            _reportService = reportService; // In-memory linked list service
        }

        // Shows the main page with all reported issues
        public IActionResult Index()
        {
            // Pass the linked list to the view so it can display all reports
            return View(_reportService.Reports);
        }

        // GET: Home/Create
        public IActionResult Create() => View(); // Simple Create page

        // GET: Home/Announcements - Redirect to Events controller
        public IActionResult Announcements() => RedirectToAction("Index", "Events");

        // GET: Home/Status - Placeholder for Service Request Status
        public IActionResult Status() => View(); // Placeholder page

        // Handles when user submits an issue report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form, IFormFile? attachment)
        {
            // Get the data the user entered in the form
            string location = form["Location"].ToString() ?? string.Empty;
            string category = form["Category"].ToString() ?? string.Empty;
            string description = form["Description"].ToString() ?? string.Empty;
            string? attachmentPath = null;

            // If user uploaded a file, save it to the server
            if (attachment != null && attachment.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await attachment.CopyToAsync(stream);

                attachmentPath = "/uploads/" + fileName; // Save the path to display the file
            }

            // Add the report to our linked list in memory
            var reportNode = _reportService.AddReport(location, category, description, attachmentPath ?? string.Empty);

            TempData["Message"] = "Issue reported successfully!"; // Show success message
            return RedirectToAction(nameof(Create));
        }

        // GET: Home/Privacy
        public IActionResult Privacy() => View(); // Static privacy page

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Error page
    }
}
