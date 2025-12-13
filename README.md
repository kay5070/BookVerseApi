# 📚 BookVerse API

A comprehensive RESTful API for an online bookstore built with **ASP.NET Core 8**, featuring user authentication, role-based authorization, and integrated Stripe payment processing.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🌟 Overview

BookVerse API is a production-ready backend system for managing an online bookstore. It implements clean architecture principles with a focus on maintainability, scalability, and security. The project demonstrates professional software development practices including dependency injection, repository pattern, unit of work, and comprehensive error handling.

## ✨ Key Features

### 🔐 Authentication & Authorization
- JWT-based authentication with refresh tokens
- Role-based access control (Admin/User)
- Secure password hashing with ASP.NET Core Identity
- Password reset functionality with email integration
- Token expiration and refresh mechanisms

### 📖 Book Management
- Complete CRUD operations for books, authors, and categories
- Advanced filtering and search capabilities
- Pagination support for large datasets
- Many-to-many relationships between books, authors, and categories
- Inventory tracking with stock management

### 💳 Payment Processing
- Stripe payment integration
- Secure order creation and management
- Real-time payment status tracking
- Automatic inventory updates on order completion
- User order history

### 🏗️ Architecture
- **Clean Architecture** with separation of concerns
- **Repository Pattern** for data access abstraction
- **Unit of Work** for transaction management
- **AutoMapper** for object-to-object mapping
- **Entity Framework Core** with Code First approach

## 🛠️ Technologies & Tools

| Category | Technologies |
|----------|-------------|
| **Framework** | ASP.NET Core 8.0 |
| **Language** | C# 12.0 |
| **Database** | SQL Server, Entity Framework Core |
| **Authentication** | JWT Bearer, ASP.NET Core Identity |
| **Payment** | Stripe API |
| **Documentation** | Swagger/OpenAPI |
| **Mapping** | AutoMapper |
| **Architecture** | Clean Architecture, Repository Pattern, Unit of Work |

## 📁 Project Structure

```
BookVerse/
├── BookVerse.Core/              # Domain entities, interfaces, constants
│   ├── Entities/                # Domain models
│   ├── Interfaces/              # Core interfaces
│   ├── Enums/                   # Enumerations
│   ├── Constants/               # Application constants
│   └── Models/                  # Domain models
├── BookVerse.Application/       # Business logic, DTOs, service interfaces
│   ├── Dtos/                    # Data Transfer Objects
│   └── Interfaces/              # Service interfaces
├── BookVerse.Infrastructure/    # Data access, external services
│   ├── Data/                    # DbContext, migrations
│   ├── Repositories/            # Repository implementations
│   ├── Services/                # Service implementations
│   └── Profiles/                # AutoMapper profiles
└── BookVerseApi/                # API layer, controllers, middleware
    ├── Controllers/             # API endpoints
    └── Middlewares/             # Custom middleware
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Stripe Account](https://stripe.com) (for payment processing)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/bookverse-api.git
   cd bookverse-api
   ```

2. **Configure User Secrets**
   ```bash
   cd BookVerseApi
   dotnet user-secrets init
   
   # Database
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=BookVerseDb;Trusted_Connection=True;TrustServerCertificate=True"
   
   # JWT Configuration
   dotnet user-secrets set "JwtOptions:Secret" "your-super-secret-key-min-32-characters-long"
   dotnet user-secrets set "JwtOptions:Issuer" "BookVerseApi"
   dotnet user-secrets set "JwtOptions:Audience" "BookVerseClient"
   dotnet user-secrets set "JwtOptions:ExpirationTimeInMinutes" "60"
   
   # Admin User
   dotnet user-secrets set "AdminUser:Email" "admin@bookverse.com"
   dotnet user-secrets set "AdminUser:Password" "Admin@123456"
   dotnet user-secrets set "AdminUser:FirstName" "Admin"
   dotnet user-secrets set "AdminUser:LastName" "User"
   
   # Stripe Keys
   dotnet user-secrets set "Stripe:PublishableKey" "pk_test_your_key"
   dotnet user-secrets set "Stripe:SecretKey" "sk_test_your_key"
   
   # Email Configuration (Optional)
   dotnet user-secrets set "EmailOptions:SmtpHost" "smtp.gmail.com"
   dotnet user-secrets set "EmailOptions:SmtpPort" "587"
   dotnet user-secrets set "EmailOptions:SmtpUsername" "your-email@gmail.com"
   dotnet user-secrets set "EmailOptions:SmtpPassword" "your-app-password"
   dotnet user-secrets set "EmailOptions:FromEmail" "noreply@bookverse.com"
   dotnet user-secrets set "EmailOptions:FromName" "BookVerse"
   ```

3. **Apply Migrations**
   ```bash
   dotnet ef database update --project ../BookVerse.Infrastructure --startup-project .
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access Swagger Documentation**
   ```
   https://localhost:7xxx/
   ```

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | User login | No |
| POST | `/api/auth/refresh-token` | Refresh access token | No |
| POST | `/api/auth/logout` | User logout | Yes |
| GET | `/api/auth/me` | Get current user profile | Yes |
| POST | `/api/auth/forgot-password` | Request password reset | No |
| POST | `/api/auth/reset-password` | Reset password | No |
| DELETE | `/api/auth/delete-account` | Delete user account | Yes |

### Book Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/book` | Get paginated books | No |
| GET | `/api/book/{id}` | Get book by ID | No |
| POST | `/api/book` | Create new book | Admin |
| PUT | `/api/book/{id}` | Update book | Admin |
| DELETE | `/api/book/{id}` | Delete book | Admin |

### Author Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/author` | Get paginated authors | No |
| GET | `/api/author/{id}` | Get author by ID | No |
| POST | `/api/author` | Create new author | Admin |
| PUT | `/api/author/{id}` | Update author | Admin |
| DELETE | `/api/author/{id}` | Delete author | Admin |

### Category Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/category` | Get paginated categories | No |
| GET | `/api/category/{id}` | Get category by ID | No |
| POST | `/api/category` | Create new category | Admin |
| PUT | `/api/category/{id}` | Update category | Admin |
| DELETE | `/api/category/{id}` | Delete category | Admin |

### Order Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/order` | Create new order | Yes |
| GET | `/api/order/{id}` | Get order by ID | Yes |
| GET | `/api/order/my-orders` | Get user's orders | Yes |
| POST | `/api/order/{id}/payment` | Create payment intent | Yes |

### Admin Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/admin/users` | Get all users | Admin |
| GET | `/api/admin/users/{id}` | Get user by ID | Admin |
| POST | `/api/admin/users/{id}/make-admin` | Grant admin role | Admin |
| POST | `/api/admin/users/{id}/remove-admin` | Remove admin role | Admin |
| DELETE | `/api/admin/users/{id}` | Delete user | Admin |

## 🔍 Query Parameters

### Pagination
```
?pageNumber=1&pageSize=10
```

### Sorting
```
?sortBy=Title&sortDescending=false
```

### Search
```
?searchTerm=harry+potter
```

### Book Filtering
```
?minPrice=10&maxPrice=50&authorId=1&categoryId=2&publishedAfter=2020-01-01
```

## 📋 Example Requests

### Register a New User
```json
POST /api/auth/register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "Password@123",
  "role": 1
}
```

### Create an Order
```json
POST /api/order
Authorization: Bearer {your_token}
{
  "items": [
    {
      "bookId": 1,
      "quantity": 2
    },
    {
      "bookId": 3,
      "quantity": 1
    }
  ]
}
```

### Create a Book
```json
POST /api/book
Authorization: Bearer {admin_token}
{
  "title": "Clean Code",
  "description": "A Handbook of Agile Software Craftsmanship",
  "isbn": "9780132350884",
  "price": 42.99,
  "publishDate": "2008-08-01",
  "quantityInStock": 50,
  "authorIds": [1],
  "categoryIds": [2, 5]
}
```
## 🔒 Security Features

- ✅ Password hashing with ASP.NET Core Identity
- ✅ JWT token-based authentication
- ✅ Refresh token mechanism
- ✅ Role-based authorization
- ✅ CORS configuration
- ✅ HTTPS enforcement
- ✅ Security headers middleware
- ✅ Input validation with Data Annotations
- ✅ SQL injection prevention with EF Core
- ✅ XSS protection

## 📝 Database Schema

```
Users (ASP.NET Identity)
├── Books
│   ├── BookAuthors (junction table)
│   │   └── Authors
│   └── BookCategories (junction table)
│       └── Categories
└── Orders
    └── OrderItems
        └── Books
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

    Karem Sabry
- GitHub: [@karem-sabry](https://github.com/karem-sabry)
- LinkedIn: [karem-sabry](https://www.linkedin.com/in/karem-sabry/)
- Email: karemsabry2013@gmail.com

## 🙏 Acknowledgments

- ASP.NET Core documentation
- Stripe API documentation
- Clean Architecture by Robert C. Martin
- Entity Framework Core best practices

---

⭐ If you found this project helpful, please give it a star!

**Built with ❤️ using ASP.NET Core**