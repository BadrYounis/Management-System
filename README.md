# Employee Management System

A comprehensive ASP.NET Core MVC application for managing employees and departments with user authentication and authorization features.

## 🚀 Features

- **Employee Management**: Complete CRUD operations for employee records
- **Department Management**: Full department lifecycle management
- **User Authentication**: Secure login, registration, and password reset functionality
- **Role-Based Authorization**: Identity-based access control
- **File Attachments**: Support for file uploads and management
- **Email Services**: Email configuration and sending capabilities
- **Responsive Design**: Modern, mobile-friendly user interface

## 🏗️ Architecture

This project follows the **Clean Architecture** pattern with clear separation of concerns:

```
├── Demo.PL/          # Presentation Layer (MVC Controllers, Views, ViewModels)
├── Demo.BLL/         # Business Logic Layer (Services, DTOs)
└── Demo.DAL/         # Data Access Layer (Entities, Repositories, DbContext)
```

### Technology Stack

- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core 8.0** - ORM for data access
- **SQL Server** - Database
- **ASP.NET Core Identity** - Authentication and authorization
- **AutoMapper** - Object-to-object mapping
- **Bootstrap** - Frontend CSS framework
- **jQuery** - JavaScript library

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Full SQL Server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MVCApp
```

### 2. Database Setup

1. Update the connection string in `Demo.PL/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.; Database=MVCApplication; Trusted_Connection=True; TrustServerCertificate=True;"
     }
   }
   ```

2. Run Entity Framework migrations:
   ```bash
   cd Demo.PL
   dotnet ef database update
   ```

### 3. Build and Run

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the application
cd Demo.PL
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## 📁 Project Structure

### Presentation Layer (Demo.PL)
- **Controllers**: Handle HTTP requests and responses
- **Views**: Razor pages for UI rendering
- **ViewModels**: Data transfer objects for views
- **Mapping**: AutoMapper profiles for object mapping

### Business Logic Layer (Demo.BLL)
- **Services**: Business logic implementation
- **DTOs**: Data transfer objects for API communication
- **Common Services**: Shared services like email and attachments

### Data Access Layer (Demo.DAL)
- **Entities**: Domain models and database entities
- **Repositories**: Data access abstraction
- **DbContext**: Entity Framework database context
- **Migrations**: Database schema versioning

## 🔧 Configuration

### Database Configuration
The application uses Entity Framework Core with SQL Server. Connection strings are configured in `appsettings.json`.

### Identity Configuration
User authentication is configured with the following password requirements:
- Minimum length: 5 characters
- Requires lowercase letters
- Requires uppercase letters
- Requires digits
- Requires non-alphanumeric characters

### Email Configuration
Email settings can be configured through the `IEmailSetting` service for sending notifications and password reset emails.

## 🎯 Key Features Implementation

### Employee Management
- Create, read, update, and delete employee records
- Employee details with department association
- File attachment support for employee documents

### Department Management
- Complete department lifecycle management
- Department-employee relationships
- Hierarchical department structure support

### Authentication & Authorization
- User registration with email verification
- Secure login with cookie authentication
- Password reset functionality
- Role-based access control

## 🧪 Testing

To run tests (if available):

```bash
dotnet test
```

## 📦 Dependencies

### NuGet Packages
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** (8.0.14)
- **Microsoft.EntityFrameworkCore.SqlServer** (8.0.14)
- **Microsoft.EntityFrameworkCore.Tools** (8.0.14)
- **Microsoft.EntityFrameworkCore.Proxies** (8.0.14)
- **AutoMapper** (14.0.0)
- **Microsoft.VisualStudio.Web.CodeGeneration.Design** (8.0.7)

## 🚀 Deployment

### Development
The application is configured for development with detailed error pages and hot reload support.

### Production
For production deployment:

1. Update `appsettings.Production.json` with production connection strings
2. Configure HTTPS certificates
3. Set up proper logging
4. Configure email services
5. Set up database backups

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Entity Framework team for the robust ORM
- Bootstrap team for the responsive CSS framework
- All contributors and open source libraries used in this project

## 📞 Support

If you have any questions or need help with this project, please:

1. Check the [Issues](https://github.com/yourusername/your-repo/issues) page
2. Create a new issue if your problem isn't already reported
3. Contact the maintainers

---

**Happy Coding! 🎉**
