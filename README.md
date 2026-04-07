# Task Manager

A full-stack Task Manager application built with **Angular** (frontend) and **ASP.NET Core** (backend).

## Features

- View all tasks in a list
- Add new tasks
- Edit existing tasks
- Delete tasks
- Mark tasks as completed / pending

## Project Structure

```
task-manager/
├── task-manager-ui/     # Angular frontend
└── TaskMgmt/            # ASP.NET Core backend
```

## Tech Stack

| Layer    | Technology           |
|----------|----------------------|
| Frontend | Angular, Bootstrap   |
| Backend  | ASP.NET Core Web API |
| Language | TypeScript, C#       |

## Prerequisites

- [Node.js](https://nodejs.org/) 
- [Angular CLI](https://angular.io/cli) — `npm install -g @angular/cli`
- [.NET SDK](https://dotnet.microsoft.com/) 

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Raguram98/task-manager.git
cd task-manager
```

### 2. Run the Backend

```bash
cd TaskMgmt
dotnet restore
dotnet run
```

The API will start on `https://localhost:7xxx` (see `Properties/launchSettings.json` for the exact port).

### 3. Run the Frontend

```bash
cd task-manager-ui
npm install
ng serve
```

Open your browser and navigate to `http://localhost:4200`

> Make sure the backend is running before starting the frontend.

## API Endpoints

| Method | Endpoint          | Description    |
|--------|-------------------|----------------|
| GET    | /api/tasks        | Get all tasks  |
| GET    | /api/tasks/{id}   | Get task by ID |
| POST   | /api/tasks        | Create a task  |
| PUT    | /api/tasks/{id}   | Update a task  |
| DELETE | /api/tasks/{id}   | Delete a task  |
