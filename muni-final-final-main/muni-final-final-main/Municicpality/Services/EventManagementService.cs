using Municicpality.Models;
using System.Collections.Generic;
using System.Linq;

namespace Municicpality.Services
{
    // Manages all events and announcements using multiple data structures for different purposes
    // GeeksforGeeks (2022). Data Structure Types, Classifications and Applications. [online] GeeksforGeeks. Available at: https://www.geeksforgeeks.org/dsa/what-is-data-structure-types-classifications-and-applications/.
    // IEvangelist (2022). Collections and Data Structures. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/standard/collections/.IEvangelist (2022). Collections and Data Structures. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/standard/collections/.
    
    public class EventManagementService
    {
        // Stack - stores newest events first (Last In, First Out)
        private readonly Stack<LocalEvent> _recentEvents = new Stack<LocalEvent>();
        
        // Queue - processes events in the order they were created (First In, First Out)
        private readonly Queue<LocalEvent> _eventProcessingQueue = new Queue<LocalEvent>();
        
        // Priority Queue - organizes events by importance level (1=highest, 5=lowest)
        private readonly SortedDictionary<int, Queue<LocalEvent>> _priorityEvents = new SortedDictionary<int, Queue<LocalEvent>>();
        
        // Hash Table - finds any event instantly by its ID
        private readonly Dictionary<int, LocalEvent> _eventLookup = new Dictionary<int, LocalEvent>();
        
        // Sorted Dictionary - keeps events organized by date for easy date filtering
        private readonly SortedDictionary<DateTime, List<LocalEvent>> _eventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();
        
        // Set - keeps track of all unique categories without duplicates
        private readonly HashSet<string> _uniqueCategories = new HashSet<string>();
        
        // Dictionary - groups events by category for fast category filtering
        private readonly Dictionary<string, List<LocalEvent>> _eventsByCategory = new Dictionary<string, List<LocalEvent>>();
        
        // List - remembers what users have searched for to make better recommendations
        private readonly List<EventSearchHistory> _searchHistory = new List<EventSearchHistory>();
        
        // Stack - tracks which events users have viewed (newest views first)
        private readonly Stack<EventViewHistory> _viewHistory = new Stack<EventViewHistory>();
        
        // Dictionary - remembers the last event each user looked at
        private readonly Dictionary<string, EventViewHistory> _lastViewedByUser = new Dictionary<string, EventViewHistory>();
        
        private int _nextId = 1;

        // Constructor to initialize with sample data
        public EventManagementService()
        {
            InitializeSampleData();
        }

        // Initialize the service with sample events and announcements
        private void InitializeSampleData()
        {
            // Add sample events
            AddEvent("Community Clean-up Day", 
                "Join us for a community-wide clean-up initiative. We'll provide gloves, bags, and refreshments. Help keep our neighborhood beautiful!", 
                "Community", 
                DateTime.Now.AddDays(7), 
                "Central Park", 
                "Municipality Environmental Department", 
                "environment@municipality.gov.za", 
                false, 2);

            AddEvent("Road Maintenance Notice", 
                "Scheduled road maintenance on Main Street will begin next week. Expect temporary traffic delays and detours.", 
                "Infrastructure", 
                DateTime.Now.AddDays(3), 
                "Main Street", 
                "Municipality Public Works", 
                "publicworks@municipality.gov.za", 
                true, 1);

            AddEvent("Youth Sports Tournament", 
                "Annual youth soccer tournament for ages 12-18. Registration required. Prizes for winners!", 
                "Sports", 
                DateTime.Now.AddDays(14), 
                "Municipal Sports Complex", 
                "Youth Development Committee", 
                "youth@municipality.gov.za", 
                false, 3);

            AddEvent("Health and Wellness Fair", 
                "Free health screenings, nutrition advice, and fitness demonstrations. All ages welcome!", 
                "Health", 
                DateTime.Now.AddDays(21), 
                "Community Center", 
                "Health Department", 
                "health@municipality.gov.za", 
                false, 3);

            AddEvent("Water Conservation Workshop", 
                "Learn about water-saving techniques and sustainable practices for your home and garden.", 
                "Environment", 
                DateTime.Now.AddDays(10), 
                "Library Meeting Room", 
                "Environmental Education Team", 
                "environment@municipality.gov.za", 
                false, 4);

            AddEvent("Safety Awareness Campaign", 
                "Important safety tips for residents. Learn about emergency procedures and neighborhood watch programs.", 
                "Safety", 
                DateTime.Now.AddDays(5), 
                "Police Station Community Room", 
                "Community Safety Unit", 
                "safety@municipality.gov.za", 
                true, 2);

            AddEvent("Educational Technology Expo", 
                "Discover the latest in educational technology. Free workshops for teachers and parents.", 
                "Education", 
                DateTime.Now.AddDays(28), 
                "High School Auditorium", 
                "Education Department", 
                "education@municipality.gov.za", 
                false, 4);

            AddEvent("Holiday Market", 
                "Local vendors, crafts, and festive activities. Perfect for holiday shopping and family fun!", 
                "Community", 
                DateTime.Now.AddDays(35), 
                "Town Square", 
                "Events Committee", 
                "events@municipality.gov.za", 
                false, 3);

            // Additional sample events to reach 15 total
            AddEvent("Public Library Book Sale", 
                "Annual book sale with thousands of books at discounted prices. Proceeds support library programs.", 
                "Education", 
                DateTime.Now.AddDays(12), 
                "Central Library", 
                "Library Friends Association", 
                "library@municipality.gov.za", 
                false, 4);

            AddEvent("Traffic Light Maintenance", 
                "Scheduled maintenance on traffic lights at Main Street intersection. Expect brief delays.", 
                "Infrastructure", 
                DateTime.Now.AddDays(2), 
                "Main Street & Oak Avenue", 
                "Traffic Department", 
                "traffic@municipality.gov.za", 
                true, 1);

            AddEvent("Senior Citizens Health Fair", 
                "Free health screenings, flu shots, and wellness information for residents 65 and older.", 
                "Health", 
                DateTime.Now.AddDays(18), 
                "Senior Center", 
                "Health Department", 
                "health@municipality.gov.za", 
                false, 3);

            AddEvent("Community Garden Workshop", 
                "Learn sustainable gardening techniques and join our community garden initiative.", 
                "Environment", 
                DateTime.Now.AddDays(25), 
                "Community Garden", 
                "Environmental Group", 
                "environment@municipality.gov.za", 
                false, 4);

            AddEvent("Neighborhood Watch Meeting", 
                "Monthly meeting to discuss community safety and crime prevention strategies.", 
                "Safety", 
                DateTime.Now.AddDays(8), 
                "Community Center", 
                "Police Department", 
                "safety@municipality.gov.za", 
                false, 2);

            AddEvent("Youth Art Exhibition", 
                "Showcase of artwork created by local students. Awards ceremony and refreshments provided.", 
                "Education", 
                DateTime.Now.AddDays(30), 
                "Art Gallery", 
                "Education Department", 
                "education@municipality.gov.za", 
                false, 4);

            AddEvent("Waste Collection Schedule Change", 
                "Due to holiday, waste collection will be delayed by one day next week. Please adjust accordingly.", 
                "Infrastructure", 
                DateTime.Now.AddDays(1), 
                "All Areas", 
                "Waste Management", 
                "waste@municipality.gov.za", 
                true, 2);
        }

        // Creates a new event and adds it to all the data structures
        public LocalEvent AddEvent(string title, string description, string category, DateTime eventDate, 
            string location, string organizer = "", string contactInfo = "", bool isAnnouncement = false, int priority = 3)
        {
            var newEvent = new LocalEvent
            {
                Id = _nextId++,
                Title = title,
                Description = description,
                Category = category,
                EventDate = eventDate,
                Location = location,
                Organizer = organizer,
                ContactInfo = contactInfo,
                IsAnnouncement = isAnnouncement,
                CreatedAt = DateTime.Now,
                Priority = priority
            };

            // Add to stack so it appears in recent events
            _recentEvents.Push(newEvent);
            
            // Add to processing queue for FIFO processing
            _eventProcessingQueue.Enqueue(newEvent);
            
            // Add to priority queue based on importance level
            if (!_priorityEvents.ContainsKey(priority))
                _priorityEvents[priority] = new Queue<LocalEvent>();
            _priorityEvents[priority].Enqueue(newEvent);
            
            // Add to hash table for instant lookup by ID
            _eventLookup[newEvent.Id] = newEvent;
            
            // Add to date dictionary for date-based filtering
            if (!_eventsByDate.ContainsKey(eventDate.Date))
                _eventsByDate[eventDate.Date] = new List<LocalEvent>();
            _eventsByDate[eventDate.Date].Add(newEvent);
            
            // Add to category tracking
            _uniqueCategories.Add(category);
            if (!_eventsByCategory.ContainsKey(category))
                _eventsByCategory[category] = new List<LocalEvent>();
            _eventsByCategory[category].Add(newEvent);

            return newEvent;
        }

        // Gets all events sorted by date
        public List<LocalEvent> GetAllEvents()
        {
            return _eventLookup.Values.OrderBy(e => e.EventDate).ToList();
        }

        // Finds all events in a specific category quickly
        public List<LocalEvent> GetEventsByCategory(string category)
        {
            return _eventsByCategory.ContainsKey(category) ? _eventsByCategory[category] : new List<LocalEvent>();
        }

        // Finds all events happening on a specific date
        public List<LocalEvent> GetEventsByDate(DateTime date)
        {
            return _eventsByDate.ContainsKey(date.Date) ? _eventsByDate[date.Date] : new List<LocalEvent>();
        }

        // Gets the most recently added events using the stack
        public List<LocalEvent> GetRecentEvents(int count = 5)
        {
            var recent = new List<LocalEvent>();
            var tempStack = new Stack<LocalEvent>();
            
            // Take events from the stack (newest first)
            for (int i = 0; i < count && _recentEvents.Count > 0; i++)
            {
                var eventItem = _recentEvents.Pop();
                recent.Add(eventItem);
                tempStack.Push(eventItem);
            }
            
            // Put the events back in the stack so we don't lose them
            while (tempStack.Count > 0)
            {
                _recentEvents.Push(tempStack.Pop());
            }
            
            return recent;
        }

        // Gets all high priority events (priority 1 and 2) using the priority queue
        public List<LocalEvent> GetHighPriorityEvents()
        {
            var highPriority = new List<LocalEvent>();
            
            // Look at priority levels 1 and 2 (highest priority)
            foreach (var priority in _priorityEvents.Keys.Where(p => p <= 2))
            {
                var queue = _priorityEvents[priority];
                var tempQueue = new Queue<LocalEvent>();
                
                // Take all events from this priority level
                while (queue.Count > 0)
                {
                    var eventItem = queue.Dequeue();
                    highPriority.Add(eventItem);
                    tempQueue.Enqueue(eventItem);
                }
                
                // Put them back so we don't lose them
                while (tempQueue.Count > 0)
                {
                    queue.Enqueue(tempQueue.Dequeue());
                }
            }
            
            return highPriority.OrderBy(e => e.Priority).ThenBy(e => e.EventDate).ToList();
        }

        // Gets all unique categories without duplicates
        public HashSet<string> GetUniqueCategories()
        {
            return new HashSet<string>(_uniqueCategories);
        }

        // Enhanced search with relevance scoring and fuzzy matching
        public List<LocalEvent> SearchEvents(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllEvents();

            var term = searchTerm.ToLower().Trim();
            var searchWords = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var scoredResults = new Dictionary<LocalEvent, double>();

            foreach (var eventItem in _eventLookup.Values)
            {
                double score = 0;
                var titleLower = eventItem.Title.ToLower();
                var descriptionLower = eventItem.Description.ToLower();
                var categoryLower = eventItem.Category.ToLower();
                var locationLower = eventItem.Location.ToLower();

                // Exact match bonus
                if (titleLower.Contains(term))
                    score += 100;
                if (descriptionLower.Contains(term))
                    score += 50;
                if (categoryLower.Contains(term))
                    score += 80;
                if (locationLower.Contains(term))
                    score += 60;

                // Word-by-word matching with partial matches
                foreach (var word in searchWords)
                {
                    if (titleLower.Contains(word))
                        score += 20;
                    if (descriptionLower.Contains(word))
                        score += 10;
                    if (categoryLower.Contains(word))
                        score += 15;
                    if (locationLower.Contains(word))
                        score += 12;
                }

                // Priority bonus (higher priority events rank higher)
                score += (6 - eventItem.Priority) * 5;

                // Recency bonus (events happening soon rank higher)
                var daysUntilEvent = (eventItem.EventDate - DateTime.Now).TotalDays;
                if (daysUntilEvent >= 0 && daysUntilEvent <= 7)
                    score += 10;
                else if (daysUntilEvent <= 14)
                    score += 5;
                else if (daysUntilEvent <= 30)
                    score += 2;

                // Announcement bonus
                if (eventItem.IsAnnouncement)
                    score += 8;

                if (score > 0)
                {
                    scoredResults[eventItem] = score;
                }
            }

            return scoredResults
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp => kvp.Key.EventDate)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        // Enhanced search history tracking with better categorization
        public void AddSearchHistory(string searchTerm, string category, string userSession)
        {
            // Auto-detect category if not provided
            if (string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(searchTerm))
            {
                category = DetectCategoryFromSearchTerm(searchTerm);
            }

            _searchHistory.Add(new EventSearchHistory
            {
                Id = _searchHistory.Count + 1,
                SearchTerm = searchTerm,
                Category = category ?? "General",
                SearchDate = DateTime.Now,
                UserSession = userSession
            });
        }

        // Auto-detect category from search terms
        private string DetectCategoryFromSearchTerm(string searchTerm)
        {
            var term = searchTerm.ToLower();
            
            // Health-related keywords
            if (term.Contains("health") || term.Contains("medical") || term.Contains("doctor") || 
                term.Contains("hospital") || term.Contains("wellness") || term.Contains("fitness"))
                return "Health";

            // Education-related keywords
            if (term.Contains("school") || term.Contains("education") || term.Contains("library") || 
                term.Contains("book") || term.Contains("learning") || term.Contains("student"))
                return "Education";

            // Environment-related keywords
            if (term.Contains("environment") || term.Contains("green") || term.Contains("recycle") || 
                term.Contains("garden") || term.Contains("conservation") || term.Contains("sustainability"))
                return "Environment";

            // Safety-related keywords
            if (term.Contains("safety") || term.Contains("police") || term.Contains("security") || 
                term.Contains("emergency") || term.Contains("crime") || term.Contains("watch"))
                return "Safety";

            // Infrastructure-related keywords
            if (term.Contains("road") || term.Contains("traffic") || term.Contains("construction") || 
                term.Contains("maintenance") || term.Contains("repair") || term.Contains("utility"))
                return "Infrastructure";

            // Sports-related keywords
            if (term.Contains("sport") || term.Contains("game") || term.Contains("tournament") || 
                term.Contains("fitness") || term.Contains("exercise") || term.Contains("athletic"))
                return "Sports";

            // Community-related keywords (default)
            if (term.Contains("community") || term.Contains("event") || term.Contains("meeting") || 
                term.Contains("gathering") || term.Contains("festival") || term.Contains("market"))
                return "Community";

            return "General";
        }

        // Get trending categories based on recent searches
        public List<string> GetTrendingCategories(int count = 5)
        {
            return _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-7)) // Last 7 days
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToList();
        }

        // Get user's search preferences
        public Dictionary<string, int> GetUserSearchPreferences(string userSession)
        {
            return _searchHistory
                .Where(s => s.UserSession == userSession)
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Filter events by date range with accurate date calculations
        public List<LocalEvent> FilterEventsByDate(List<LocalEvent> events, string dateFilter)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            return dateFilter.ToLower() switch
            {
                "today" => events.Where(e => e.EventDate.Date == today).ToList(),
                "tomorrow" => events.Where(e => e.EventDate.Date == today.AddDays(1)).ToList(),
                "thisweek" => events.Where(e => e.EventDate >= today && e.EventDate < today.AddDays(7)).ToList(),
                "nextweek" => events.Where(e => e.EventDate >= today.AddDays(7) && e.EventDate < today.AddDays(14)).ToList(),
                "thismonth" => events.Where(e => e.EventDate >= today && e.EventDate < GetFirstDayOfNextMonth(today)).ToList(),
                "nextmonth" => events.Where(e => e.EventDate >= GetFirstDayOfNextMonth(today) && e.EventDate < GetFirstDayOfNextMonth(GetFirstDayOfNextMonth(today))).ToList(),
                _ => events
            };
        }

        // Helper method to get the first day of the next month
        private DateTime GetFirstDayOfNextMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1);
        }

        // Enhanced recommendation system with better variety and first-time user support
        public List<LocalEvent> GetRecommendations(string userSession, int count = 5)
        {
            // Get user's recent searches
            var userSearches = _searchHistory
                .Where(s => s.UserSession == userSession)
                .OrderByDescending(s => s.SearchDate)
                .Take(10) // Reduced to focus on recent behavior
                .ToList();

            // Get global search patterns for trending analysis
            var globalSearches = _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-14)) // Reduced to 14 days for more recent trends
                .ToList();

            var recommendations = new List<LocalEvent>();
            var scoredEvents = new Dictionary<LocalEvent, double>();

            // Check if user is new or has limited search history
            var isNewUser = userSearches.Count <= 2;
            var hasRecentSearches = userSearches.Any(s => s.SearchDate >= DateTime.Now.AddDays(-3));

            if (isNewUser || !hasRecentSearches)
            {
                // For new users or users with old searches, prioritize variety and trending
                recommendations = GetDiverseRecommendations(count, globalSearches);
            }
            else
            {
                // For users with recent activity, use personalized recommendations with variety
                recommendations = GetPersonalizedRecommendations(userSearches, globalSearches, count);
            }

            return recommendations;
        }

        // Get diverse recommendations for new users or users with limited history
        private List<LocalEvent> GetDiverseRecommendations(int count, List<EventSearchHistory> globalSearches)
        {
            var recommendations = new List<LocalEvent>();
            var usedCategories = new HashSet<string>();
            var usedEvents = new HashSet<int>();

            // 1. Get trending categories (most popular recently)
            var trendingCategories = GetAdvancedTrendingCategories().Take(3).ToList();
            
            // 2. Get popular categories (overall popular)
            var popularCategories = GetTimeWeightedPopularCategories(globalSearches).Take(3).ToList();
            
            // 3. Get high priority events
            var highPriorityEvents = GetHighPriorityEvents().Where(e => e.EventDate >= DateTime.Now).ToList();
            
            // 4. Get upcoming events (next 7 days)
            var upcomingEvents = GetAllEvents()
                .Where(e => e.EventDate >= DateTime.Now && e.EventDate <= DateTime.Now.AddDays(7))
                .OrderBy(e => e.EventDate)
                .ToList();

            // Strategy: Mix different types of recommendations
            var strategies = new List<Func<List<LocalEvent>>>()
            {
                () => GetEventsFromCategories(trendingCategories.Select(t => t.Category).ToList(), 2, usedCategories, usedEvents),
                () => GetEventsFromCategories(popularCategories.Select(p => p.Category).ToList(), 2, usedCategories, usedEvents),
                () => highPriorityEvents.Where(e => !usedEvents.Contains(e.Id)).Take(1).ToList(),
                () => upcomingEvents.Where(e => !usedEvents.Contains(e.Id)).Take(1).ToList()
            };

            // Apply strategies in order until we have enough recommendations
            foreach (var strategy in strategies)
            {
                if (recommendations.Count >= count) break;
                
                var newEvents = strategy();
                foreach (var evt in newEvents)
                {
                    if (recommendations.Count >= count) break;
                    recommendations.Add(evt);
                    usedEvents.Add(evt.Id);
                    usedCategories.Add(evt.Category);
                }
            }

            // Fill remaining slots with diverse events
            if (recommendations.Count < count)
            {
                var remainingEvents = GetAllEvents()
                    .Where(e => e.EventDate >= DateTime.Now)
                    .Where(e => !usedEvents.Contains(e.Id))
                    .OrderBy(e => Guid.NewGuid()) // Randomize for variety
                    .Take(count - recommendations.Count)
                    .ToList();
                
                recommendations.AddRange(remainingEvents);
            }

            return recommendations.Take(count).ToList();
        }

        // Get personalized recommendations for users with recent activity
        private List<LocalEvent> GetPersonalizedRecommendations(List<EventSearchHistory> userSearches, 
            List<EventSearchHistory> globalSearches, int count)
        {
            var recommendations = new List<LocalEvent>();
            var usedEvents = new HashSet<int>();
            var usedCategories = new HashSet<string>();

            // Analyze user preferences with reduced time decay (more balanced)
            var userCategoryFrequency = AnalyzeUserPreferences(userSearches);
            
            // Get trending and popular categories
            var trendingCategories = GetAdvancedTrendingCategories().Take(2).ToList();
            var popularCategories = GetTimeWeightedPopularCategories(globalSearches).Take(2).ToList();

            // Build recommendations with variety in mind
            var scoredEvents = new Dictionary<LocalEvent, double>();

            // 1. User's preferred categories (but limit to avoid repetition)
            var userCategories = userCategoryFrequency.Take(2).ToList(); // Limit to top 2
            foreach (var userCategory in userCategories)
            {
                var categoryEvents = GetEventsByCategory(userCategory.Category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .ToList();

                foreach (var eventItem in categoryEvents)
                {
                    var score = CalculateBalancedEventScore(eventItem, userCategory.Count, userCategory.TimeDecayScore, true);
                    if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                    {
                        scoredEvents[eventItem] = score;
                    }
                }
            }

            // 2. Trending categories (medium weight)
            foreach (var trendingCategory in trendingCategories)
            {
                var categoryEvents = GetEventsByCategory(trendingCategory.Category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .ToList();

                foreach (var eventItem in categoryEvents)
                {
                    var score = CalculateBalancedEventScore(eventItem, trendingCategory.SearchCount, trendingCategory.TrendingScore, false);
                    if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                    {
                        scoredEvents[eventItem] = score;
                    }
                }
            }

            // 3. Popular categories (lower weight)
            foreach (var popularCategory in popularCategories)
            {
                var categoryEvents = GetEventsByCategory(popularCategory.Category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .ToList();

                foreach (var eventItem in categoryEvents)
                {
                    var score = CalculateBalancedEventScore(eventItem, popularCategory.SearchCount, popularCategory.TimeWeight, false);
                    if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                    {
                        scoredEvents[eventItem] = score;
                    }
                }
            }

            // Sort by score and ensure variety
            var sortedEvents = scoredEvents
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            // Apply variety filter - limit events per category
            var categoryCounts = new Dictionary<string, int>();
            foreach (var evt in sortedEvents)
            {
                if (recommendations.Count >= count) break;
                
                var categoryCount = categoryCounts.GetValueOrDefault(evt.Category, 0);
                if (categoryCount < 2) // Max 2 events per category
                {
                    recommendations.Add(evt);
                    categoryCounts[evt.Category] = categoryCount + 1;
                }
            }

            // Fill remaining slots with diverse events if needed
            if (recommendations.Count < count)
            {
                var remainingEvents = GetAllEvents()
                    .Where(e => e.EventDate >= DateTime.Now)
                    .Where(e => !recommendations.Any(r => r.Id == e.Id))
                    .OrderBy(e => Guid.NewGuid())
                    .Take(count - recommendations.Count)
                    .ToList();
                
                recommendations.AddRange(remainingEvents);
            }

            return recommendations.Take(count).ToList();
        }

        // Helper method to get events from specific categories with variety
        private List<LocalEvent> GetEventsFromCategories(List<string> categories, int maxPerCategory, 
            HashSet<string> usedCategories, HashSet<int> usedEvents)
        {
            var events = new List<LocalEvent>();
            
            foreach (var category in categories)
            {
                if (usedCategories.Contains(category)) continue;
                
                var categoryEvents = GetEventsByCategory(category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .Where(e => !usedEvents.Contains(e.Id))
                    .OrderBy(e => e.EventDate)
                    .Take(maxPerCategory)
                    .ToList();
                
                events.AddRange(categoryEvents);
            }
            
            return events;
        }

        // Balanced event scoring that reduces over-weighting of single categories
        private double CalculateBalancedEventScore(LocalEvent eventItem, double categoryWeight, double timeDecayScore, bool isUserCategory)
        {
            double score = 0;

            // Reduced base score to prevent over-weighting
            score += Math.Min(categoryWeight * 5, 20); // Cap category weight influence

            // Reduced user category bonus
            if (isUserCategory)
                score += 3; // Reduced from 8

            // Time decay with reduced impact
            score += Math.Min(timeDecayScore * 2, 10); // Cap time decay influence

            // Priority bonus (higher priority events get higher scores)
            score += (6 - eventItem.Priority) * 2;

            // Recency bonus (events happening soon get higher scores)
            var daysUntilEvent = (eventItem.EventDate - DateTime.Now).TotalDays;
            if (daysUntilEvent <= 1)
                score += 5;
            else if (daysUntilEvent <= 3)
                score += 4;
            else if (daysUntilEvent <= 7)
                score += 3;
            else if (daysUntilEvent <= 14)
                score += 2;
            else if (daysUntilEvent <= 30)
                score += 1;

            // Announcement bonus
            if (eventItem.IsAnnouncement)
                score += 2;

            // Add some randomness for variety (0.9 to 1.1)
            var random = new Random(eventItem.Id);
            score *= (0.9 + random.NextDouble() * 0.2);

            return score;
        }

        // Enhanced user preference analysis with time decay
        private List<UserCategoryPreference> AnalyzeUserPreferences(List<EventSearchHistory> userSearches)
        {
            var now = DateTime.Now;
            var preferences = new List<UserCategoryPreference>();

            var categoryGroups = userSearches
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .ToList();

            foreach (var group in categoryGroups)
            {
                var category = group.Key;
                var searches = group.OrderByDescending(s => s.SearchDate).ToList();
                
                // Calculate time decay score (recent searches weighted higher)
                double timeDecayScore = 0;
                double trendingScore = 0;
                
                foreach (var search in searches)
                {
                    var daysSinceSearch = (now - search.SearchDate).TotalDays;
                    var decayFactor = Math.Exp(-daysSinceSearch / 7.0); // 7-day half-life
                    timeDecayScore += decayFactor;
                    
                    // Trending score based on search frequency over time
                    if (daysSinceSearch <= 3)
                        trendingScore += 2.0;
                    else if (daysSinceSearch <= 7)
                        trendingScore += 1.5;
                    else if (daysSinceSearch <= 14)
                        trendingScore += 1.0;
                }

                preferences.Add(new UserCategoryPreference
                {
                    Category = category,
                    Count = searches.Count,
                    LastSearch = searches.First().SearchDate,
                    TimeDecayScore = timeDecayScore,
                    TrendingScore = trendingScore
                });
            }

            return preferences.OrderByDescending(p => p.TimeDecayScore * p.TrendingScore).ToList();
        }

        // Get advanced trending categories with multiple time windows
        private List<TrendingCategory> GetAdvancedTrendingCategories()
        {
            var now = DateTime.Now;
            var trendingCategories = new List<TrendingCategory>();

            // Analyze different time windows
            var timeWindows = new[]
            {
                new { Days = 1, Weight = 3.0, Name = "Today" },
                new { Days = 3, Weight = 2.5, Name = "Last 3 Days" },
                new { Days = 7, Weight = 2.0, Name = "Last Week" },
                new { Days = 14, Weight = 1.5, Name = "Last 2 Weeks" },
                new { Days = 30, Weight = 1.0, Name = "Last Month" }
            };

            foreach (var window in timeWindows)
            {
                var windowSearches = _searchHistory
                    .Where(s => s.SearchDate >= now.AddDays(-window.Days))
                    .Where(s => !string.IsNullOrEmpty(s.Category))
                    .GroupBy(s => s.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(3)
                    .ToList();

                foreach (var category in windowSearches)
                {
                    var existing = trendingCategories.FirstOrDefault(t => t.Category == category.Category);
                    if (existing != null)
                    {
                        existing.SearchCount += category.Count;
                        existing.TrendingScore += category.Count * window.Weight;
                    }
                    else
                    {
                        trendingCategories.Add(new TrendingCategory
                        {
                            Category = category.Category,
                            SearchCount = category.Count,
                            TrendingScore = category.Count * window.Weight,
                            TimeWindow = window.Name
                        });
                    }
                }
            }

            return trendingCategories.OrderByDescending(t => t.TrendingScore).Take(8).ToList();
        }

        // Get time-weighted popular categories
        private List<PopularCategory> GetTimeWeightedPopularCategories(List<EventSearchHistory> globalSearches)
        {
            var now = DateTime.Now;
            var popularCategories = new List<PopularCategory>();

            var categoryGroups = globalSearches
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .ToList();

            foreach (var group in categoryGroups)
            {
                var category = group.Key;
                var searches = group.ToList();
                
                // Calculate time weight (recent searches weighted higher)
                double timeWeight = 0;
                foreach (var search in searches)
                {
                    var daysSinceSearch = (now - search.SearchDate).TotalDays;
                    var weight = Math.Exp(-daysSinceSearch / 14.0); // 14-day half-life
                    timeWeight += weight;
                }

                popularCategories.Add(new PopularCategory
                {
                    Category = category,
                    SearchCount = searches.Count,
                    TimeWeight = timeWeight
                });
            }

            return popularCategories.OrderByDescending(p => p.TimeWeight).Take(5).ToList();
        }

        // Enhanced related categories with better relationship mapping
        private List<string> GetEnhancedRelatedCategories(List<string> userCategories)
        {
            var relatedCategories = new HashSet<string>();
            
            // Enhanced category relationships
            var categoryRelationships = new Dictionary<string, List<string>>
            {
                ["Infrastructure"] = new List<string> { "Community", "Safety", "Environment" },
                ["Health"] = new List<string> { "Community", "Education", "Safety" },
                ["Education"] = new List<string> { "Community", "Health", "Sports" },
                ["Community"] = new List<string> { "Health", "Education", "Sports", "Infrastructure" },
                ["Sports"] = new List<string> { "Community", "Health", "Education" },
                ["Environment"] = new List<string> { "Infrastructure", "Community", "Health" },
                ["Safety"] = new List<string> { "Infrastructure", "Community", "Health" },
                ["Waste Management"] = new List<string> { "Environment", "Infrastructure", "Community" }
            };

            foreach (var userCategory in userCategories)
            {
                if (categoryRelationships.ContainsKey(userCategory))
                {
                    foreach (var related in categoryRelationships[userCategory])
                    {
                        relatedCategories.Add(related);
                    }
                }
            }

            // Remove user's own categories from related
            foreach (var userCategory in userCategories)
            {
                relatedCategories.Remove(userCategory);
            }

            return relatedCategories.ToList();
        }

        // Enhanced event scoring with trending and time decay factors
        private double CalculateEnhancedEventScore(LocalEvent eventItem, double categoryWeight, DateTime lastSearch, 
            double timeDecayScore, bool isUserCategory, double trendingScore)
        {
            double score = 0;

            // Base score from category frequency with time decay
            score += categoryWeight * 10 * timeDecayScore;

            // Bonus for user's own categories
            if (isUserCategory)
                score += 8;

            // Trending category bonus
            score += trendingScore * 5;

            // Priority bonus (higher priority events get higher scores)
            score += (6 - eventItem.Priority) * 3;

            // Recency bonus (events happening soon get higher scores)
            var daysUntilEvent = (eventItem.EventDate - DateTime.Now).TotalDays;
            if (daysUntilEvent <= 1)
                score += 8;
            else if (daysUntilEvent <= 3)
                score += 6;
            else if (daysUntilEvent <= 7)
                score += 4;
            else if (daysUntilEvent <= 14)
                score += 3;
            else if (daysUntilEvent <= 30)
                score += 2;

            // Announcement bonus (announcements are often more important)
            if (eventItem.IsAnnouncement)
                score += 3;

            // View history bonus (if user has viewed similar events)
            var viewBonus = GetViewHistoryBonus(eventItem);
            score += viewBonus;

            return score;
        }

        // Get bonus score based on user's view history
        private double GetViewHistoryBonus(LocalEvent eventItem)
        {
            var userViews = _viewHistory
                .Where(v => v.EventCategory == eventItem.Category)
                .Take(5)
                .ToList();

            if (userViews.Any())
            {
                return Math.Min(userViews.Count * 2, 10); // Max 10 bonus points
            }

            return 0;
        }

        // Calculate event score based on various factors (legacy method for compatibility)
        private double CalculateEventScore(LocalEvent eventItem, double categoryWeight, DateTime lastSearch, bool isUserCategory)
        {
            return CalculateEnhancedEventScore(eventItem, categoryWeight, lastSearch, 1.0, isUserCategory, 0.5);
        }

        // Get related categories based on category relationships
        private List<string> GetRelatedCategories(List<string> userCategories)
        {
            var relatedCategories = new List<string>();
            var categoryRelationships = GetCategoryRelationships();

            foreach (var userCategory in userCategories)
            {
                if (categoryRelationships.ContainsKey(userCategory))
                {
                    relatedCategories.AddRange(categoryRelationships[userCategory]);
                }
            }

            return relatedCategories.Distinct().Where(c => !userCategories.Contains(c)).ToList();
        }

        // Define category relationships for better recommendations
        private Dictionary<string, List<string>> GetCategoryRelationships()
        {
            return new Dictionary<string, List<string>>
            {
                ["Community"] = new List<string> { "Health", "Education", "Safety" },
                ["Health"] = new List<string> { "Community", "Education", "Environment" },
                ["Education"] = new List<string> { "Community", "Health", "Environment" },
                ["Environment"] = new List<string> { "Health", "Education", "Community" },
                ["Safety"] = new List<string> { "Community", "Infrastructure" },
                ["Infrastructure"] = new List<string> { "Safety", "Environment" },
                ["Sports"] = new List<string> { "Community", "Health" }
            };
        }

        // Get popular events based on global search patterns
        private List<LocalEvent> GetPopularEvents(int count)
        {
            var popularCategories = _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-30))
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            var popularEvents = new List<LocalEvent>();
            foreach (var category in popularCategories)
            {
                var events = GetEventsByCategory(category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .OrderBy(e => e.Priority)
                    .ThenBy(e => e.EventDate)
                    .Take(count - popularEvents.Count);
                
                popularEvents.AddRange(events);
                if (popularEvents.Count >= count) break;
            }

            return popularEvents.Take(count).ToList();
        }

        // Get event by ID using hash table
        public LocalEvent? GetEventById(int id)
        {
            return _eventLookup.ContainsKey(id) ? _eventLookup[id] : null;
        }

        // Track event view using Stack and Dictionary data structures
        public void TrackEventView(int eventId, string userSession = "anonymous")
        {
            var eventItem = GetEventById(eventId);
            if (eventItem == null) return;

            var viewHistory = new EventViewHistory
            {
                Id = _viewHistory.Count + 1,
                EventId = eventId,
                UserSession = userSession,
                ViewedAt = DateTime.Now,
                EventTitle = eventItem.Title,
                EventCategory = eventItem.Category
            };

            // Add to view history stack (most recent first)
            _viewHistory.Push(viewHistory);

            // Update user's last viewed event in dictionary
            _lastViewedByUser[userSession] = viewHistory;
        }

        // Get last viewed event for a user using Dictionary lookup
        public EventViewHistory? GetLastViewedEvent(string userSession = "anonymous")
        {
            return _lastViewedByUser.ContainsKey(userSession) ? _lastViewedByUser[userSession] : null;
        }

        // Get recent view history using Stack (most recent first)
        public List<EventViewHistory> GetRecentViewHistory(int count = 5)
        {
            return _viewHistory.Take(count).ToList();
        }

        // Get view history for specific user
        public List<EventViewHistory> GetUserViewHistory(string userSession = "anonymous", int count = 10)
        {
            return _viewHistory.Where(v => v.UserSession == userSession).Take(count).ToList();
        }

        // Clear all data
        public void ClearAll()
        {
            _recentEvents.Clear();
            _eventProcessingQueue.Clear();
            _priorityEvents.Clear();
            _eventLookup.Clear();
            _eventsByDate.Clear();
            _uniqueCategories.Clear();
            _eventsByCategory.Clear();
            _searchHistory.Clear();
            _viewHistory.Clear();
            _lastViewedByUser.Clear();
            _nextId = 1;
        }
    }
}
