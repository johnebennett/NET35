@echo off
setlocal

rem Prefer MSBuild from the current developer environment.
set "BUILD_EXE="
for /f "delims=" %%I in ('where MSBuild.exe 2^>nul') do if not defined BUILD_EXE set "BUILD_EXE=%%I"
if defined BUILD_EXE goto build

rem Locate Visual Studio or Build Tools when MSBuild is not on PATH.
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto missing
for /f "usebackq delims=" %%I in (`"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do if not defined BUILD_EXE set "BUILD_EXE=%%I"
if not defined BUILD_EXE goto missing

:build
echo Building MailKitEmailProcessor - Release...
"%BUILD_EXE%" "%~dp0MailKitEmailProcessor.sln" /t:Build /p:Configuration=Release /p:Platform="Any CPU" /nologo /v:minimal
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if not "%BUILD_EXIT_CODE%"=="0" goto failed
echo Build succeeded: "%~dp0bin\Release\MailKitEmailProcessor.exe"
exit /b 0

:failed
echo Build failed with exit code %BUILD_EXIT_CODE%.
exit /b %BUILD_EXIT_CODE%

:missing
echo ERROR: MSBuild was not found. Install Visual Studio or Build Tools with the .NET Framework 4.5.2 targeting pack.
exit /b 1
