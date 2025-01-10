@echo off

set NUGET_GLOBAL_PACKAGES=%USERPROFILE%\.nuget\packages
set NUGET_LOCAL_SOURCE=%ProgramFiles(x86)%\Microsoft SDKs\NuGetPackages
set NUPKG_DIR=.\PlaywrightGitHubActionAndLocal\bin\Debug

:: Select the last nupkg file (in alphabetical order)
set "NUPKG_PATH="
for /f "delims=" %%f in ('dir "%NUPKG_DIR%\*.nupkg" /b /o:n') do (
    set "NUPKG_PATH=%NUPKG_DIR%\%%f"
)

echo Global NuGet packages: %NUGET_GLOBAL_PACKAGES%
echo Local NuGet source: %NUGET_LOCAL_SOURCE%
echo NuGet package path: %NUPKG_PATH%

echo.
echo clean global %NUGET_GLOBAL_PACKAGES%
rmdir /s /q "%NUGET_GLOBAL_PACKAGES%\playwright.github.action.and.local"

echo.
echo clean local %NUGET_LOCAL_SOURCE%
rmdir /s /q "%NUGET_LOCAL_SOURCE%\playwright.github.action.and.local"

echo.
echo add nuget to local %NUGET_LOCAL_SOURCE%
nuget add %NUPKG_PATH% -Source "%NUGET_LOCAL_SOURCE%"
