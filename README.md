# 🏏 IPLStore – Clean Architecture E-Commerce System

Built with .NET 9 • ASP.NET MVC • Web API • EF Core • SQL Server • Identity

📌 Overview

IPLStore is a complete e-commerce platform built using modern .NET 9 and Clean Architecture principles.
It showcases enterprise-grade practices such as:

Multi-layered architecture (Core → Application → Infrastructure → API → MVC Client)

Repository & Unit of Work patterns

DTO-driven communication between layers

Proper separation of concerns & service abstraction

Secure user authentication using ASP.NET Identity

REST API consumption using HttpClientFactory in MVC

The app includes a full product catalog, shopping cart system, checkout workflow, and order history for authenticated users.

📐 Clean Architecture Diagram

                          +-------------------------+
                          |      MVC CLIENT         |
                          |  (Razor Views + UI)     |
                          |  Calls REST API via     |
                          |  HttpClientFactory      |
                          +------------+------------+
                                       |
                                       v
                          +-------------------------+
                          |          API            |
                          |  Controllers (Thin)     |
                          |  Uses Application Layer |
                          +------------+------------+
                                       |
                                       v
                  +-----------------------------------------+
                  |             APPLICATION LAYER            |
                  |  DTOs • Services • Interfaces            |
                  |  Business Logic • Validation             |
                  +------------+-----------------------------+
                               |
                               v
                +-----------------------------------+
                |         INFRASTRUCTURE LAYER      |
                | EF Core • Repositories • UoW      |
                | AppDbContext • SQL Server Access  |
                +------------+----------------------+
                             |
                             v
                   +-----------------------+
                   |        CORE LAYER     |
                   | Entities • Domain     |
                   +-----------------------+
Mermaid Diagram
flowchart TD

A[MVC Client (Razor, Bootstrap)] --> B[API Layer<br>Controllers]
B --> C[Application Layer<br>Services, DTOs, Interfaces]
C --> D[Infrastructure Layer<br>EF Core, Repositories, Unit of Work]
D --> E[Core Layer<br>Entities]

style A fill:#0077cc,stroke:#003d66,color:#fff
style B fill:#228be6,stroke:#0c4a6e,color:#fff
style C fill:#495057,stroke:#212529,color:#fff
style D fill:#343a40,stroke:#212529,color:#fff
style E fill:#212529,stroke:#000,color:#fff

🛠 Tech Stack

Backend

C#, .NET 9

ASP.NET Core MVC

ASP.NET Core Web API

Entity Framework Core 9

SQL Server

LINQ / async-await

ASP.NET Identity

Architecture & Patterns

Clean Architecture

Repository Pattern

Unit of Work Pattern

DTO Mapping

Dependency Injection

Service Layer Abstraction

Separation of Concerns / SOLID Principles

Frontend

Razor Views (CSHTML)

Bootstrap 5

Server-Side Rendering

Responsive UI

📦 Features

🛍 Product Features

Product listing

Search by name, type, franchise

Product details page

🛒 Cart Features

Add item to cart

Update quantity

Clear cart

User-specific cart persistence

Cart stored in database

📦 Order Features

Place orders

Order summary page

Order history for logged-in users

Order items with full product details

🔐 Authentication

ASP.NET Identity login

Role-based UI behavior

Protected cart & order routes

📁 Project Structure

<img width="572" height="232" alt="image" src="https://github.com/user-attachments/assets/aa2d52c7-d51b-491d-8ca3-7d7e14af8254" />


🧠 Workflow Example — Placing an Order

1️⃣ User adds product to cart

MVC calls API via HttpClientFactory

API calls Application Service

Service uses Repositories through UnitOfWork

Cart entry saved in SQL Server

2️⃣ User checks out

MVC → API → OrderService

Order + OrderItems created

EF Core handles transactions

3️⃣ User views order history

API returns list of user orders

MVC renders them in a responsive Bootstrap layout

Clean, modular, enterprise-style workflow.

▶️ Getting Started
1. Clone the repo
   git clone https://github.com/vamseebkrishna/IPLStore
   cd IPLStore

2. Update connection string
   Edit in:
   IPLStore.API/appsettings.json
   Example:
   "ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=IPLStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   
3. Apply EF Core migrations
   cd IPLStore.API
   dotnet ef database update

4. Run the application
   cd IPLStore.Client
   dotnet run
   App will run at:
   https://localhost:7200  (or similar)

🤝 Contributing

Feel free to:

Open issues

Suggest enhancements

Create pull requests

Fork and extend the architecture

Contributions are welcome!

⭐ If you like this project, give it a star on GitHub!

👉 https://github.com/vamseebkrishna/IPLStore





