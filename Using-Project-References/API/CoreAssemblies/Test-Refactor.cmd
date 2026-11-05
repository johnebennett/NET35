@echo off
setlocal
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-Refactor.ps1" %*
exit /b %errorlevel%
