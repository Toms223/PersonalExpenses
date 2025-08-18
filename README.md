# Personal Expenses Tracker

A comprehensive ASP.NET Core MVC web application for tracking personal expenses with support for both one-time and recurring expenses, featuring Microsoft Graph integration for Outlook calendar synchronization.

## Features

### Expense Management
- **One-time Expenses**: Create and track individual expenses by date
- **Continuous/Recurring Expenses**: Set up expenses that repeat every X days with customizable periods
- **Fixed Expenses**: Special category for recurring expenses with unchanging amounts (e.g., gym memberships, subscriptions)
- **Flexible Editing**: In-place editing of all expense types with validation
- **Smart Filtering**: Filter expenses by date range, view type (continuous vs fixed), and monthly breakdowns

### Advanced Visualization & Analytics
- **Comprehensive Dashboard**: View all expenses in a single, sortable interface
- **Monthly View**: Dedicated monthly expense breakdown with totals and projections
- **Date-based Filtering**: Filter by start date, end date, or specific date ranges
- **Expense Analytics**: Track spending patterns with expected vs actual monthly totals
- **Dual View Modes**: Toggle between continuous/recurring expenses and fixed expenses
- **Sortable Lists**: Sort expenses by date (ascending/descending) with dynamic controls

### Microsoft Graph Integration
- **Outlook Calendar Sync**: Automatically create recurring calendar events for continuous expenses
- **Smart Event Creation**: Calendar events include expense details, amounts, and proper recurrence patterns
- **Seamless Authentication**: Single sign-on with Microsoft accounts for both app access and calendar integration
- **Recurring Reminders**: Never miss a payment with automated calendar notifications

### Security & User Management
- **Microsoft OIDC Authentication**: Secure login using Microsoft personal accounts (`/consumers` endpoint)
- **Privacy-focused**: Only stores username and email address from Microsoft Graph
- **User Isolation**: Each user sees only their own expenses with proper user ID validation
- **Automatic User Creation**: New users are automatically created on first login
- **Session Management**: Middleware ensures user existence and handles authentication edge cases

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 9.0)
- **Frontend**: Razor Pages with modern CSS (glass morphism design)
- **Database**: PostgreSQL 17 with Entity Framework Core
- **Authentication**: OpenID Connect (OIDC) with Microsoft Identity Platform
- **ORM**: Microsoft Entity Framework Core 9.0
- **Microsoft Graph**: Microsoft Graph SDK 5.90.0 for calendar integration
- **Containerization**: Docker Compose for PostgreSQL database

## Architecture

### Project Structure
```
PersonalExpenses/
├── Controllers/           # MVC Controllers
├── Services/             # Business logic services
├── Data/                 # EF Core DbContext
├── Model/                # Entity models
├── ViewModel/            # View models and DTOs
├── Auth/                 # Microsoft Graph authentication
├── Extensions/           # Service registration extensions
├── Middleware/           # Custom middleware
├── Views/                # Razor views
└── PostgreSQL/           # Database initialization scripts
```

### Key Services
- **ExpensesService**: Core business logic for expense CRUD operations
- **CalendarService**: Microsoft Graph integration for Outlook calendar
- **GraphAccessTokenProvider**: Custom authentication provider for Graph API calls

## Prerequisites

- .NET 9.0 SDK
- Docker and Docker Compose (for PostgreSQL)
- Microsoft Azure AD application registration
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd PersonalExpenses
```

### 2. Set up PostgreSQL with Docker
```bash
cd Compose
docker-compose up -d
```

This will start PostgreSQL 17 with the database `expenses` and initialize the required tables.

### 3. Configure Microsoft OIDC Authentication

Register your application in Azure AD with these settings:
- **Redirect URI**: `https://localhost:5001/signin-oidc` (adjust for your domain)
- **Logout URL**: `https://localhost:5001/signout-callback-oidc`
- **Supported account types**: Personal Microsoft accounts only
- **Required permissions**:
  - `openid`
  - `profile`
  - `email`
  - `offline_access`
  - `User.Read`
  - `Calendars.ReadWrite`

### 4. Environment Configuration

Set the following environment variables or update `appsettings.json`:

```bash
# Database Configuration
export POSTGRES_DB=expenses
export POSTGRES_USER=toms223
export POSTGRES_PASSWORD=123123
export POSTGRES_HOST=localhost
export POSTGRES_PORT=5432

# Microsoft Authentication
export MICROSOFT_CLIENT_ID=your-client-id
export MICROSOFT_CLIENT_SECRET=your-client-secret
```

### 5. Run Database Migrations
```bash
dotnet ef database update
```

### 6. Build and Run
```bash
dotnet restore
dotnet run
```

The application will be available at `https://localhost:5001`.

## Usage Guide

### Getting Started
1. Navigate to the application URL
2. Click "Login" to authenticate with your Microsoft account
3. Grant permissions for calendar access when prompted
4. Start adding expenses immediately

### Managing Expenses

#### One-time Expenses
- Use the main form to add individual expenses
- Specify name, amount, and date
- Edit in-place by clicking the "Edit" button

#### Continuous/Recurring Expenses
- Toggle to "Continuous View" using the switch
- Add expenses that repeat every X days
- Mark as "Fixed" for unchanging amounts (subscriptions)
- Sync to Outlook calendar for reminders

### View Options
- **All Expenses**: Default chronological view with sorting
- **Monthly View**: Grouped by month with spending totals
- **Date Filtering**: Custom date range filtering
- **Continuous vs Fixed Toggle**: Switch between expense types

### Calendar Integration
- Click "Add to Outlook Calendar" on any continuous expense
- Events are created with proper recurrence patterns
- Includes expense details and amount formatting

## Configuration Details

### Database Configuration
The application uses PostgreSQL with Entity Framework Core. Connection details are configured via environment variables for security.

### Authentication Flow
1. User clicks login → redirects to Microsoft OIDC
2. Microsoft authenticates user and returns claims
3. Application extracts email/name and creates/retrieves user record
4. User ID is added to claims for session management
5. Microsoft Graph access token is stored for calendar operations

### Microsoft Graph Integration
- Uses the `/consumers` tenant for personal Microsoft accounts
- Requires `Calendars.ReadWrite` permission
- Creates recurring events with proper recurrence patterns
- Handles authentication via access tokens stored in session

## Development

### Running in Development
```bash
dotnet watch run
```

### Database Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Docker Development
```bash
# Start only the database
cd Compose
docker-compose up -d db

# View logs
docker-compose logs -f
```

## Deployment

### Environment Variables for Production
```bash
POSTGRES_HOST=your-production-db-host
POSTGRES_PORT=5432
POSTGRES_DB=expenses
POSTGRES_USER=your-db-user
POSTGRES_PASSWORD=your-secure-password
MICROSOFT_CLIENT_ID=your-production-client-id
MICROSOFT_CLIENT_SECRET=your-production-client-secret
ASPNETCORE_ENVIRONMENT=Production
```

### Docker Deployment
The application can be containerized and deployed with Docker. Ensure PostgreSQL is accessible and environment variables are properly configured.

## API Permissions

The application requires these Microsoft Graph permissions:
- `User.Read`: Basic user profile information
- `Calendars.ReadWrite`: Create and manage calendar events
- `offline_access`: Refresh tokens for long-lived sessions

## Security Considerations

- All database queries are parameterized to prevent SQL injection
- User isolation is enforced at the service level
- Microsoft OIDC provides enterprise-grade authentication
- No sensitive data is logged or exposed in client-side code
- HTTPS is enforced in production environments

## Roadmap

- [ ] Expense categories and tagging system
- [ ] Budget planning and spending alerts
- [ ] Data export (PDF, Excel, CSV)
- [ ] Mobile-responsive design improvements
- [ ] Multi-currency support
- [ ] Google Calendar integration
- [ ] Expense receipt photo upload
- [ ] Advanced analytics and reporting
- [ ] API endpoints for third-party integrations
- [ ] Dark/light theme toggle
