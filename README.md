SGEducationMobile — Cross‑Platform Academic Services App (Built with .NET MAUI + .NET Core)
SGEducationMobile is a modern, cross‑platform mobile application built with .NET MAUI and powered by a .NET Core Web API backend.
It was developed for SGEducation Nigeria Ltd. to streamline university admission assistance, academic document submission, and AI‑powered academic guidance for students.

🚀 Core Purpose
SGEducationMobile helps students and applicants:

Apply for university admission assistance

Upload and submit academic documents securely

Access AI‑generated academic articles and study guidance

Manage their academic journey through a clean, intuitive interface



🏗️ Architecture Overview
This project follows a modular, scalable architecture suitable for enterprise‑grade mobile applications.

Mobile App — .NET MAUI
Cross‑platform UI (Android, iOS, Windows)

MVVM architecture

Dependency Injection (built‑in MAUI DI)

Refit/HttpClient for API communication

Secure local storage for tokens and user data

Reusable UI components and services

Backend — .NET Core Web API
Clean, layered architecture

RESTful endpoints

JWT authentication

SQL database integration (SQL Server)

Document upload endpoints with validation and secure storage


Shared Models
DTOs for request/response

Shared validation rules

Strong typing across client and server

🔐 Security (Seperate RESTful API Project)
JWT‑based authentication

HTTPS‑only API communication

Sanitized document uploads

Role‑based access control

Secure token storage on device

📦 Project Structure
Code
SGEducationMobile/
│
├── SGEducationMobile.App/        # .NET MAUI mobile project
├── SGEducationMobile.Shared/     # Shared models, DTOs, utilities
└── README.md                     # Project documentation
🌟 Key Features
Document Upload & Submission — WAEC, NECO, transcripts, certificates

Admission Application Workflow — guided steps for applicants

AI‑Generated Academic Articles — personalized study guidance

Cross‑Platform Support — Android, iOS, Windows

Modern UI/UX — MAUI Shell navigation, responsive layouts

Cloud‑Ready Backend — optimized for Azure deployment


🛠️ Tech Stack
Frontend: .NET MAUI

Backend: .NET Core Web API

Database: SQL Server

Auth: JWT

Tools: Rider / Visual Studio, Postman, Azure Studio

📥 Getting Started
Clone the repository:

bash
git clone https://github.com/ChisomGrantAnuforom/SGEducationNigeriaMobile.git
Open the solution in Visual Studio or JetBrains Rider, restore packages, and run the MAUI project.

🤝 Contributing
Contributions, issues, and feature requests are welcome.
Please open an issue or submit a pull request.

📄 License
This project is proprietary software owned by SGEducation Nigeria Ltd.
