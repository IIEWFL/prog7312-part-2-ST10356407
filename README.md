# Municipality Portal

A modern ASP.NET Core MVC web application that demonstrates advanced data structures implementation for municipal services management. The application features issue reporting using a custom linked list and a sophisticated events management system using multiple data structures.

## YouTube Video Link: https://youtu.be/nOBOMl4PJQQ

## Features

### Issue Reporting System
- **Custom Linked List Implementation**: In-memory storage using a custom linked list data structure
- **File Upload Support**: Attach images or documents to issue reports
- **Real-time Display**: View all reported issues instantly on the home page
- **Category Organization**: Organize issues by type (Sanitation, Roads, Utilities, Other)
- **No Database Required**: All data stored in memory for fast access

### Events & Announcements System
- **Advanced Data Structures**: Uses 9 different data structures for optimal performance
- **Smart Search**: Fuzzy matching with relevance scoring
- **Personalized Recommendations**: AI-powered suggestions based on user behavior
- **Trending Analysis**: Multi-timeframe trending categories
- **User Session Tracking**: Personalized experience with view history
- **Priority Management**: High-priority announcements and events
- **Date Filtering**: Filter events by time periods (today, this week, this month)

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Language**: C#
- **Frontend**: HTML5, CSS3, JavaScript
- **Data Storage**: In-memory data structures (no database required)
- **File Storage**: Local file system for uploads

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, Visual Studio Code, or any compatible IDE
- Windows, or Linux

## How to Run and Compile

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd muni-final-final-main/Municicpality
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Application
```bash
dotnet build
```

### 4. Run the Application
```bash
dotnet run
```

The application will start on `https://localhost:5001` or `http://localhost:5000`

## How to Use the Application

### Home Page
- **Main Portal**: Access all municipal services
- **Issue Reports Display**: View all reported issues in real-time
- **Navigation**: Quick access to reporting and events

### Issue Reporting
1. Click "Report an Issue" on the home page
2. Fill in the required information:
   - Location (required)
   - Category (required): Sanitation, Roads, Utilities, or Other
   - Description (required)
   - Optional file attachment
3. Click "Submit Report"
4. View your report immediately on the home page

### Events & Announcements
1. Click "Local Events & Announcements" on the home page
2. **Browse Events**: View all upcoming events and announcements
3. **Search**: Use the search functionality with smart filtering
4. **Filter by Category**: Select specific categories
5. **Date Filtering**: Filter by time periods (today, this week, this month)
6. **View Details**: Click on any event for detailed information
7. **Personalized Recommendations**: Get suggestions based on your activity

## Data Structures Implementation

### Issue Reporting (Linked List)
- **IssueReportNode**: Individual issue nodes with pointer to next node
- **IssueReportLinkedList**: Custom linked list with add, get all, and clear operations
- **Benefits**: Simple, memory-efficient, maintains order of submission

### Events System (Advanced Data Structures)
- **Stack**: Recent events (LIFO behavior)
- **Queue**: Event processing order (FIFO behavior)
- **Priority Queue**: High-priority announcements
- **Hash Table**: O(1) event lookup by ID
- **Sorted Dictionary**: Chronological event organization
- **Set**: Unique category tracking
- **Dictionary**: Category-based event grouping
- **List**: Search history for recommendations

### 3. Build the Application
### 4. Run the Application
The application will start on https://localhost:5001 or http://localhost:5000
How to Use the Application

### Home Page
Main Portal: Access all municipal services
Issue Reports Display: View all reported issues in real-time
Navigation: Quick access to reporting and events

### Issue Reporting
Click "Report an Issue" on the home page
Fill in the required information:
Location (required)
Category (required): Sanitation, Roads, Utilities, or Other
Description (required)
Optional file attachment
Click "Submit Report"
View your report immediately on the home page

### Events & Announcements
Click "Local Events & Announcements" on the home page
Browse Events: View all upcoming events and announcements
Search: Use the search functionality with smart filtering
Filter by Category: Select specific categories
Date Filtering: Filter by time periods (today, this week, this month)
View Details: Click on any event for detailed information
