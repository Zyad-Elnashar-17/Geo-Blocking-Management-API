Geo-Blocking Management System API
Project Overview
This is a .NET 8 Web API designed to manage and enforce geographic blocking based on IP addresses. The system allows administrators to block specific countries permanently or temporarily and provides a mechanism to check if a visitor's IP is from a blocked region using external geolocation services.

Technical Architecture
The project is built following SOLID principles and the Repository Pattern to ensure maintainability and scalability.

Core Technologies
Framework: .NET 8.0

Storage: In-Memory using ConcurrentDictionary and ConcurrentBag to ensure Thread-Safety during high-concurrency requests.

External Integration: Integration with ipapi.co for real-time Geolocation lookup.

Background Tasks: Hosted Services for automatic cleanup of expired data.

Key Features
1. Country Blocking Management
Permanent Block: Add or remove ISO 2-letter country codes from the permanent block list.

Temporal Block: Block a country for a specific duration (1 to 1440 minutes).

Pagination & Search: Optimized retrieval of blocked countries with support for searching and paginated results.

2. IP Lookup and Validation
IP Intelligence: Retrieve detailed information about any IP address, including Country Name, Code, and ISP.

Access Control: Automatically check if the requester's IP is currently blocked based on both permanent and temporal rules.

3. Automated Background Cleanup
A background service (TemporalBlockCleanupService) runs every 5 minutes to identify and remove expired temporal blocks, ensuring the system remains efficient and the memory is optimized.

4. Logging and Auditing
All blocked access attempts are logged, including the IP address, timestamp, and the reason for blocking.

Logs are accessible via a paginated endpoint for administrative review.

Design Patterns Applied
Dependency Injection (DI): Used for decoupling interfaces from their implementations.

Repository Pattern: Each data domain (Permanent Blocks, Temporal Blocks, Logs) has its own repository and interface, following the Interface Segregation Principle.

Singleton Lifetime: Repositories are registered as Singletons to maintain data state in memory across the application lifecycle.

Typed HttpClient: Used for efficient and resilient communication with the external Geolocation API.

Setup and Running
Prerequisites:

.NET 8.0 SDK or later.

Visual Studio 2022 or VS Code.

Configuration:

Open appsettings.json to configure the IpApiSettings if a private API key is required.

Execution:

Clone the repository.

Run the command: dotnet run

Access the Swagger UI at: https://localhost:{port}/swagger

API Endpoints Summary
Countries
POST /api/countries/block - Add a permanent block.

DELETE /api/countries/block/{code} - Remove a permanent block.

GET /api/countries/blocked - List all blocked countries (Paginated).

POST /api/countries/temporal-block - Add a timed block.

IP & Geolocation
GET /api/ip/lookup - Get info for a specific IP.

GET /api/ip/check-block - Check if the caller is blocked.

Logs
GET /api/logs - View access attempt logs (Paginated).