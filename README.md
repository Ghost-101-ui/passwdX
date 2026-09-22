<h1 align="center">
  🔐 PASSWORDX
</h1>

<p align="center">
  <b>Smart Password Analyzer &amp; Generator</b><br/>
  <i>Built by <a href="https://github.com/Ghost-101-ui">CyberEDT</a> — All rights reserved © CyberEDT 2026</i>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Platform-.NET%208.0-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Framework-ASP.NET%20Core%20MVC-blue?style=for-the-badge&logo=microsoft" />
  <img src="https://img.shields.io/badge/Made%20by-CyberEDT-red?style=for-the-badge" />
  <img src="https://img.shields.io/badge/License-Proprietary-black?style=for-the-badge" />
</p>

---

## 📖 What is PasswordX?

**PasswordX** is a feature-rich, web-based **password security suite** built with ASP.NET Core MVC. It helps users analyze the strength of their existing passwords, generate strong new ones, and understand exactly *why* a password is weak or strong — all in a clean, modern interface.

Whether you are a developer, a cybersecurity enthusiast, or just someone who wants to stay safe online, PasswordX gives you the tools to make smarter password decisions.

---

## ✨ Features

| Module | Description |
|---|---|
| 🔍 **Analyzer** | Real-time password strength analysis with entropy calculation and a detailed scorecard |
| 🔑 **Generator** | Generate passwords in multiple styles: Professional, Easy to Remember, Maximum Security, Gaming, Corporate |
| 🧬 **Password DNA** | Visual breakdown of character composition — uppercase, lowercase, digits, symbols |
| 🧠 **Smart Coach** | AI-like tips and coaching on how to improve your password |
| 🕵️ **Pattern Detection** | Detects common patterns (keyboard walks, dictionary words, leet-speak, repeated chars) |
| ⏱️ **Crack Time Estimator** | Estimates how long a brute-force attack would take to crack your password |
| 📊 **Scorecard** | Full security report with a letter grade (A+ to F) and actionable feedback |
| 🎮 **Challenge Mode** | A fun timed mini-game to build and guess strong passwords |

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 8.0), C#
- **Frontend:** HTML5, CSS3 (Vanilla), JavaScript, jQuery AJAX
- **Rendering:** Razor Views (`.cshtml`)
- **Cryptography:** `System.Security.Cryptography.RandomNumberGenerator` (no `Random` for passwords)
- **No external dependencies** beyond jQuery and canvas-confetti CDN links

---

## ⚙️ Prerequisites

Before running PasswordX, make sure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (**required**)
- [Git](https://git-scm.com/downloads)
- A terminal (PowerShell, CMD, or bash)
- A modern web browser (Chrome, Edge, Firefox)

---

## 🚀 Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/Ghost-101-ui/passwdX.git
cd passwdX
```

### 2. Verify .NET SDK is installed

```bash
dotnet --version
```

You should see something like `8.0.x`. If not, download it from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0).

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Build the Project

```bash
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### 5. Run the Application

```bash
dotnet run
```

### 6. Open in Browser

Once running, open your browser and go to:

```
http://localhost:5000
```

The app will load immediately. No login or setup required!

---

## 📁 Project Structure

```
passwdX/
├── Controllers/
│   └── PasswordController.cs     # Handles all HTTP routes and API endpoints
├── Models/
│   └── PasswordModel.cs          # Request/response models
├── Services/
│   ├── PasswordChecker.cs        # Core strength analysis logic
│   ├── PasswordGenerator.cs      # Password generation (5 styles)
│   ├── PasswordCoach.cs          # Smart coaching tips engine
│   ├── PatternDetector.cs        # Common pattern detection
│   ├── CrackEstimator.cs         # Brute-force time estimation
│   ├── ScorecardService.cs       # Full security scorecard grading
│   ├── ChallengeService.cs       # Challenge mode game logic
│   └── MemeService.cs            # Fun contextual meme messages
├── Views/
│   ├── Shared/_Layout.cshtml     # Main layout with navbar & footer
│   └── Password/Index.cshtml     # Main single-page application view
├── wwwroot/
│   ├── Content/style.css         # All styles
│   ├── Scripts/                  # JS modules (analyzer, generator, challenge)
│   └── images/icons/             # Pixel-art icons and sprites
├── Program.cs                    # App entry point and middleware pipeline
└── PasswordX.csproj              # Project configuration (targets net8.0)
```

---

## 🔒 How It Works

1. **User types a password** into the analyzer input
2. The input is sent via **jQuery AJAX** to the ASP.NET Core backend (`/Password/Analyze`)
3. The server runs it through multiple service classes:
   - Entropy calculation
   - Pattern detection (dictionary, leet-speak, keyboard walks)
   - Crack time estimation
   - Scorecard grading
4. Results are returned as **JSON** and rendered dynamically on the page with animations
5. The **Generator** similarly calls `/Password/Generate` with user preferences and returns a ready-to-use password

---

## 🎮 Challenge Mode

Test your password knowledge! In Challenge Mode:
- A password is shown to you
- You must rate its strength before the timer runs out
- Score points based on how accurate your rating is

---

## 🤝 Contributing

This project is built and maintained by **CyberEDT**. If you'd like to contribute:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---

## 📄 License & Copyright

```
© 2026 CyberEDT — All Rights Reserved.

This software is the intellectual property of CyberEDT.
Unauthorized reproduction, distribution, or modification
of this project or any part thereof is strictly prohibited
without prior written permission from CyberEDT.
```

---

<p align="center">
  Built with ❤️ by <b>CyberEDT</b> | Stay Safe, Stay Secure 🔐
</p>
