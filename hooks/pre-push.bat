@echo off
setlocal enabledelayedexpansion

echo Pre-push hook is running...

REM Get current branch name
for /f "delims=" %%i in ('git rev-parse --abbrev-ref HEAD') do set "current_branch=%%i"

if "!current_branch!" == "develop" (
    echo Current branch is develop. Running pre-push checks...

    echo Merging develop branch to main...

    REM Switch to main branch and update
    git checkout main
    git pull origin main

    REM Get the last commit message from develop branch
    for /f "delims=" %%i in ('git log develop -1 --pretty^=%%s') do set "last_commit_msg=%%i"

    REM Merge develop branch, using --squash option to compress all commits into one
    git merge --squash develop

    REM Commit with the last commit  message
    git commit -m "!last_commit_msg!"

    REM Uncomment the following line if you want to automatically push to main
    REM git push origin main

    echo Merge completed. Please review the changes and push manually if everything looks good.


    echo Pre-push checks for develop branch completed.
) else if "!current_branch!" == "main" (
    echo Current branch is main. Running release process...

    REM Get project name
    for %%I in (.) do set "project_name=%%~nxI"
    echo Processing project: %project_name%

    REM Set project file path
    set "project_file=src\%project_name%\%project_name%.csproj"

    REM Check if project  file exists
    if not exist "%project_file%" (
        echo Project file not found: %project_file%
        echo Exiting...
        exit /b 1
    )

    echo Found project file: %project_file%

    REM Get current version
    for /f "tokens=3 delims=<>" %%a in ('findstr "<PackageVersion>" "!project_file!"') do set "current_version=%%a"
    if not defined current_version (
        echo PackageVersion not found in !project_file!. Exiting...
        exit /b 1
    )
    echo Current version: !current_version!

    REM Build release version
    echo Building release version...
    dotnet build -c Release
    if !errorlevel! neq 0 (
        echo Build failed. Exiting...
        exit /b 1
    )

    REM Run tests
    echo Running tests...
    dotnet test
    if !errorlevel! neq 0 (
        echo Tests failed. Exiting...
        exit /b 1
    )

    REM Increment patch version
    for /f "tokens=1-3 delims=." %%a in ("!current_version!") do (
        set /a patch=%%c+1
        set "new_version=%%a.%%b.!patch!"
    )
    echo New version: !new_version!

    REM Update version in project file
    powershell -Command "(Get-Content '!project_file!') -replace '<PackageVersion>!current_version!</PackageVersion>', '<PackageVersion>!new_version!</PackageVersion>' | Set-Content '!project_file!'"

    echo Version updated for !project_name! (!project_file!)

    REM Stage the changed project file
    git add "!project_file!"

    REM Commit the version change
    git commit -m "Bump version to !new_version!"

    REM Create an annotated tag
    git tag -a v!new_version! -m "Release version !new_version!"

    echo Release process completed. New version %new_version% has been tagged. Please push changes and tags manually.

    REM Note: Do not push automatically here to avoid triggering the pre-push hook in a loop

    echo Release process completed. Merging changes to develop...

    REM Store the current commit hash
    for /f "delims=" %%i in ('git rev-parse HEAD') do set "current_commit=%%i"

    REM Switch to develop branch
    git checkout develop

    REM Pull latest changes from remote develop
    git pull origin develop

    REM Merge changes from main
    git merge !current_commit!

    echo Merged changes from main to develop. Please review and push manually if everything looks good.
) else (
    echo Current branch is !current_branch!. Skipping pre-push checks and merge.
)

endlocal