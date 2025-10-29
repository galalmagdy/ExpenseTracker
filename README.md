# Expense Tracker - .NET MAUI

## 📱 Overview
A cross-platform mobile application built using **.NET MAUI** that helps users track their daily expenses with categories, filters, and summary statistics.  
The app follows the **MVVM architecture**, uses **Dependency Injection**, and stores data locally with **SQLite**.

---

## ✨ Features Implemented
- ✅ Login screen (demo account)
- ✅ Expense list view (amount, category, date, description)
- ✅ Add, edit, and delete expenses
- ✅ Search and filter by category and date
- ✅ Summary dashboard with total and category breakdown chart
- ✅ Local persistence using SQLite
- ✅ MVVM + Dependency Injection architecture
- ✅ Flyout navigation with multiple pages

---

## 🏗️ Architecture
- **MVVM Pattern:** Clear separation between UI and business logic (View ↔ ViewModel ↔ Model).  
- **Service Layer:** Abstracted through `IExpenseService` and `IAuthService`.  
- **Dependency Injection:** Configured in `MauiProgram.cs`.  
- **Local Persistence:** SQLite storage handled by `SQLiteExpenseService`.  
- **Mock Backend:** `MockExpenseService` used during development.

---

## ⚙️ Technologies Used
- .NET MAUI  
- C# 12  
- SQLite.NET (Local Database)  
- CommunityToolkit.MVVM  
- Microcharts (for summary visualization)  

---

## 🚀 Setup Instructions

### 1. Prerequisites
- .NET SDK 8.0 or later  
- Visual Studio 2022 (v17.8+) with MAUI workload  
- Android/iOS emulator or Windows target  

### 2. Clone the repository
```bash
git clone https://github.com/galalmagdy/ExpenseTracker.git
cd expense-tracker-maui

### 3. Build and Run

    Open ExpenseTracker.sln in Visual Studio.

    Select the target platform (Android, iOS, or Windows).

    Press Run (▶) to start the app.

### 4. Demo Login Credentials

The application includes a pre-configured demo account to quickly test the features:
Field	Value
Username	demo@user.com
Password	1234

🧩 Mock Service Implementation

The app originally used MockExpenseService for temporary in-memory data during development.
It has since been replaced by SQLiteExpenseService for persistent local storage using SQLite.


👤 Author

Galal Magdy
🌐 https://galalmagdy.github.io/

📧 galalmagdywork@gmail.com
