# TaskPlanner

TaskPlanner is a Razor Pages web application built on .NET 8 (C# 12) for creating and managing projects and tasks. This README provides a complete, ready-to-use guide to build, run and contribute.

## Key information

- Framework: .NET 8
- UI: Razor Pages (Razor views)
- IDE: Visual Studio 2022 compatible
- Repository: `TaskPlanner.GUI`

## Prerequisites

- .NET 8 SDK installed
- Visual Studio 2022 (or VS Code with C# extensions)
- (Optional) SQL Server / PostgreSQL / SQLite depending on your `ConnectionStrings`

## Quickstart — CLI

1. Restore and build:
    
    4-space indented code block (use in your terminal)
    
    dotnet restore
    dotnet build

2. Run the app:

    dotnet run --project <path-to-project-csproj>

3. Open a browser at the URL printed by the host (usually `https://localhost:5001`).

## Quickstart — Visual Studio

1. Open the solution in Visual Studio 2022.
2. Build the solution via __Build > Build Solution__.
3. Start debugging via __Debug > Start Debugging__.

## Configuration

- Main configuration file: `appsettings.json`.
- Add or update the `ConnectionStrings` section to point to your database.

Example `appsettings.json` snippet (insert into your actual file):

    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=.;Database=TaskPlannerDb;Trusted_Connection=True;"
        },
        "Logging": {
            "LogLevel": {
                "Default": "Information",
                "Microsoft": "Warning"
            }
        }
    }

## Database / EF Core

If the project uses EF Core, apply migrations:

- Using CLI (from repository root):

    dotnet ef migrations add InitialCreate --project <path-to-project-csproj>
    dotnet ef database update --project <path-to-project-csproj>

- Using Visual Studio: open __Tools > NuGet Package Manager > Package Manager Console__ and run the same `Add-Migration` / `Update-Database` commands targeting the correct project.

## Static assets

Static assets are under `wwwroot/`. Libraries (jQuery, validation, bootstrap) are available in:

- `wwwroot/lib/jquery`
- `wwwroot/lib/jquery-validation`
- `wwwroot/lib/bootstrap`

Check licenses in `wwwroot/lib/*/LICENSE*`.

## Common tasks

- Run tests:

    dotnet test

- Publish:

    dotnet publish -c Release -o ./publish

- Create a Docker image (optional): add a `Dockerfile` and run `docker build`.

## Troubleshooting

- If Razor views do not refresh, clean and rebuild the solution.
- If migrations fail, ensure the connection string is correct and the target database server is accessible.
- For static file versioning, the project uses `asp-append-version="true"` in `_Layout.cshtml`.

## Contributing

- Fork the repository, create a feature branch, run tests and open a pull request.
- Keep code style consistent with the existing project files.
- Provide clear commit messages and update `README.md` for any new configuration steps.

## Security & secrets

- Never commit secret values. Use user secrets (for development) or environment variables in production.
- For local development with Visual Studio, use __Debug > Open debug launch profiles UI__ or environment variables.

## License

- Verify repository license in the project root. Third-party packages include their licenses in `wwwroot/lib/*/LICENSE*`.

---

If you want, I can:
- add a sample `appsettings.Development.json`,
- add Docker instructions,
- or generate skeleton CI pipeline YAML for GitHub Actions.