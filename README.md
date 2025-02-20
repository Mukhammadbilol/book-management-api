# Book Management API

## Overview
The Book Management API is a RESTful API built using ASP.NET Core Web API. It allows users to manage books by performing CRUD operations. The API also includes features like JWT-based authentication, soft delete, and popularity score calculation

## Technologies Used
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger (for API documentation)
- JWT (for authentication)

## API Endpoints

### 
- **GET** `/api/books` - Retrieve a list of all books
- **GET** `/api/books/popular?page=1&pageSize=5` - Retrieve a list of popular books sorted by views count
- **GET** `/api/books/{id}` - Retrieve details of a specific book by ID
- **POST** `/api/auth/login` - Authenticate a user and generate a JWT token
- **POST** `/api/books` -  Add a single book
- **POST** `/api/books/bulk` - Add multiple books in bulk
- **PUT** `/api/books/{id}` - Update a specific book by ID
- **DELETE** `/api/books/{id}` - Soft delete a specific book by ID
- **DELETE** `/api/books/bulk` - Soft delete multiple books by their IDs

## Authentication
**To access secured endpoints:**
1. Use the `/api/auth/login` endpoint to get a JWT token
2. Include the token in the `Authorization` header

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request

## Contact
For questions or support, email: muhammadbilolumaroff@gmail.com