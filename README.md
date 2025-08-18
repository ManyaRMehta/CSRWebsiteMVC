# CSR Platform

A Corporate Social Responsibility (CSR) platform built with **ASP.NET Core MVC (.NET 8)** and **PostgreSQL**.  
The platform enables employees to participate in CSR missions, while administrators can manage missions, skills, themes, and applications through a secure role-based system.  

---

## Features

### User Features
- Register, log in, and manage personal profiles (name, photo, phone, department, title).
- Upload and update profile pictures with avatar shown in navbar and profile page.
- Browse and apply to missions.
- Filter missions by:
  - Name
  - Theme
  - Skills (multi-select filter).
- View personal mission applications.

### Admin Features
- Role-based access for admins.
- Create, edit, and delete missions, themes, and skills.
- Assign multiple skills to missions (many-to-many).
- Manage mission applications (accept/delete).
- Create new admin accounts via an admin-only page.
- Upload and display mission images.
- Admin dashboard with analytics (planned).

### UI/UX Improvements
- Missions page is the homepage, displayed in a card layout.
- Navbar updates:
  - Profile link under “Hello, Username” dropdown.
  - Profile picture avatar displayed in navbar.
  - Admin dropdown with Missions, Skills, Themes, Applications.
- Toast notifications after profile updates.
- Restriction of mission access to logged-in users only.

---

## Tech Stack
- **Backend:** ASP.NET Core MVC (.NET 8)
- **Database:** PostgreSQL (Code-First with EF Core)
- **Authentication & Authorization:** ASP.NET Identity with role-based authorization
- **UI:** Razor Views, Bootstrap, custom improvements
- **Tools:** VS Code, pgAdmin 4

---



