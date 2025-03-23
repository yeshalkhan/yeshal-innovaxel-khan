# URL Shortener API 

A simple RESTful API to shorten URLs. It includes features like retrieving original URLs, update existing short URLs, delete short URLs, and track number of accesses to each short URL.


## Installation & Setup

### 1. Clone the Repository
```sh
git clone https://github.com/yeshalkhan/yeshal-innovaxel-khan.git
cd yeshal-innovaxel-khan
```

### 2. Install Dependencies
Make sure you have **.NET SDK** installed. Then run:
```sh
dotnet restore
```

### 3. Configure Database (MS SQL Server)
i. Open `appsettings.json` and update the **connection string**:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=UrlShortenerDB;User Id=sa;TrustServerCertificate=True;"
}
```
ii. Apply migrations:
```sh
dotnet ef database update
```

### 4. Run the Application
```sh
dotnet run
```

## API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| `POST` | `/api/shorten` | Create a short URL |
| `GET` | `/api/shorten/{shortCode}` | Retrieve original URL |
| `PUT` | `/api/shorten/{shortCode}` | Update short URL |
| `DELETE` | `/api/shorten/{shortCode}` | Delete a short URL |
| `GET` | `/api/shorten/{shortCode}/stats` | Get short URL stats |


## Tools & Technologies
- **C# / .NET 7/8**
- **Entity Framework Core**
- **MS SQL Server**
- **Swagger (API Documentation)**
- **Postman (API Testing)**
