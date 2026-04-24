# Cinema Programme

## A web application for viewing movie programme and ticket reservations.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
* [Setup](#setup)

## General info
This project is simple web application that allows the user to see all movies in their cinema, see the programme for the current month and buy the ticket for the showing of their choice.
It focuses on easy search (by title, genre and price) and a reliable seat reservation system that prevents double-booking for the same screening.

## Technologies
* **Architecture:** MVC (Model-View-Controller)
* **Backend:** .NET 8, C#, Entity Framework Core
* **Frontend:** Razor views
* **Database:** SQL Server

## Features
* **Advanced Search:** Filtering by title, genre, and price range.
* **Smart Reservations:** Real-time seat selection with collision prevention.
* **Monthly Schedule:** Dynamic view of the cinema programme.

## Setup
To run this project locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Navytree/MVCCinemaProgramme.git

2. **Update Database Connection:**
Open appsettings.json and update the DefaultConnection string to point to your local SQL Server instance.

3. **Apply Migrations:**
Run the following command in the Package Manager Console (Visual Studio) or Terminal to create the database:
   ```bash
    dotnet ef database update
4. **Run the application:**
  Press F5 in Visual Studio 2022 or use the command:
  ```bash
  dotnet run
