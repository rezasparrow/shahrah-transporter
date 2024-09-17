@echo off
set /p migrationName=Please enter migration name: 
dotnet ef migrations add %migrationName% --output-dir Persistence\Migrations