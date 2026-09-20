@echo off
cd /d "%~dp0"
dotnet build "SearchForItem.slnx"

start "" "SearchForItem\bin\Debug\net10.0\SearchForItem.exe"