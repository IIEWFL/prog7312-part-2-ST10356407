using Microsoft.AspNetCore.Mvc;
using Municicpality.Models;
using Municicpality.Services;
using System.Diagnostics;

namespace Municicpality.Controllers
{
    //////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    // Controller to manage local events and announcements functionality
    public class EventsController : Controller
    {
        private readonly EventManagementService _eventService;
        private readonly ILogger<EventsController> _logger;

        // Constructor injection for event service and logger
        public EventsController(EventManagementService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService; // Event management service with advanced data structures
            _logger = logger; // Logger for debugging and monitoring
        }

        // Shows the main events page with all events and smart features
        public IActionResult Index(string? userSession)
        {
            // Get data from all our data structures
            var allEvents = _eventService.GetAllEvents();
            var categories = _eventService.GetUniqueCategories();
            var recentEvents = _eventService.GetRecentEvents(5); // From stack
            var highPriorityEvents = _eventService.GetHighPriorityEvents(); // From priority queue
            var trendingCategories = _eventService.GetTrendingCategories(5);
            var lastViewedEvent = _eventService.GetLastViewedEvent(userSession ?? "anonymous");

            // Put everything together for the view
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = allEvents,
                Categories = categories,
                RecentEvents = recentEvents,
                HighPriorityEvents = highPriorityEvents,
                TrendingCategories = trendingCategories,
                LastViewedEvent = lastViewedEvent,
                SearchTerm = "",
                SelectedCategory = "",
                SelectedDateFilter = ""
            };

            return View(viewModel);
        }

        // Handles search requests and shows filtered results
        [HttpPost]
        public IActionResult Search(string? searchTerm, string? category, string? dateFilter, string? userSession)
        {
            // Remember what the user searched for to improve recommendations
            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(category))
            {
                _eventService.AddSearchHistory(searchTerm ?? string.Empty, category ?? string.Empty, userSession ?? "anonymous");
            }

            // Start with all events or filter by category first
            List<LocalEvent> searchResults;
            
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                searchResults = _eventService.GetEventsByCategory(category); // Uses dictionary for fast lookup
            }
            else
            {
                searchResults = _eventService.GetAllEvents();
            }
            
            // Filter by search text if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchResults = searchResults.Where(e => 
                    e.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    e.Description.ToLower().Contains(searchTerm.ToLower()) ||
                    e.Location.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            // Filter by date if specified
            if (!string.IsNullOrEmpty(dateFilter))
            {
                searchResults = _eventService.FilterEventsByDate(searchResults, dateFilter);
            }

            // Get personalized recommendations based on user's search history
            var recommendations = _eventService.GetRecommendations(userSession ?? "anonymous", 3);

            var trendingCategories = _eventService.GetTrendingCategories(5);

            var viewModel = new EventsIndexViewModel
            {
                AllEvents = searchResults,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                TrendingCategories = trendingCategories,
                SearchTerm = searchTerm ?? "",
                SelectedCategory = category ?? "",
                SelectedDateFilter = dateFilter ?? "",
                Recommendations = recommendations
            };

            return View("Index", viewModel);
        }

        // Shows details for a specific event
        public IActionResult Details(int id, string? userSession)
        {
            var eventItem = _eventService.GetEventById(id); // Uses hash table for instant lookup
            if (eventItem == null)
            {
                return NotFound();
            }

            // Remember that this user viewed this event for better recommendations
            _eventService.TrackEventView(id, userSession ?? "anonymous");

            return View(eventItem);
        }

        // GET: Events/Create - Form to create new event/announcement
        public IActionResult Create()
        {
            return View();
        }

        // Handles when user creates a new event or announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalEvent eventModel)
        {
            if (ModelState.IsValid)
            {
                // Add the event to all our data structures
                var newEvent = _eventService.AddEvent(
                    eventModel.Title,
                    eventModel.Description,
                    eventModel.Category,
                    eventModel.EventDate,
                    eventModel.Location,
                    eventModel.Organizer,
                    eventModel.ContactInfo,
                    eventModel.IsAnnouncement,
                    eventModel.Priority
                );

                TempData["Message"] = eventModel.IsAnnouncement ? 
                    "Announcement created successfully!" : 
                    "Event created successfully!";
                
                return RedirectToAction(nameof(Index));
            }

            return View(eventModel);
        }

        // GET: Events/ByCategory - Get events by specific category
        public IActionResult ByCategory(string? category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction(nameof(Index));
            }

            var events = _eventService.GetEventsByCategory(category);
            var trendingCategories = _eventService.GetTrendingCategories(5);
            var recommendations = _eventService.GetRecommendations("anonymous", 3);

            var viewModel = new EventsIndexViewModel
            {
                AllEvents = events,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                TrendingCategories = trendingCategories,
                Recommendations = recommendations,
                SearchTerm = "",
                SelectedCategory = category,
                SelectedDateFilter = ""
            };

            return View("Index", viewModel);
        }

        // GET: Events/ByDate - Get events by specific date
        public IActionResult ByDate(DateTime date)
        {
            var events = _eventService.GetEventsByDate(date);
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = events,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                SearchTerm = "",
                SelectedCategory = ""
            };

            return View("Index", viewModel);
        }

        // GET: Events/Recommendations - Get personalized recommendations
        public IActionResult Recommendations(string? userSession)
        {
            var recommendations = _eventService.GetRecommendations(userSession ?? "anonymous", 10);
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = recommendations,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                SearchTerm = "",
                SelectedCategory = "",
                Recommendations = recommendations
            };

            return View("Index", viewModel);
        }
    }

    // View model for the events index page
    public class EventsIndexViewModel
    {
        public List<LocalEvent> AllEvents { get; set; } = new List<LocalEvent>();
        public HashSet<string> Categories { get; set; } = new HashSet<string>();
        public List<LocalEvent> RecentEvents { get; set; } = new List<LocalEvent>();
        public List<LocalEvent> HighPriorityEvents { get; set; } = new List<LocalEvent>();
        public List<LocalEvent> Recommendations { get; set; } = new List<LocalEvent>();
        public List<string> TrendingCategories { get; set; } = new List<string>();
        public EventViewHistory? LastViewedEvent { get; set; }
        public string SearchTerm { get; set; } = "";
        public string SelectedCategory { get; set; } = "";
        public string SelectedDateFilter { get; set; } = "";
    }
}
