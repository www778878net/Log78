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

    REM Run release.bat
    if exist "%~dp0release.bat" (
        call "%~dp0release.bat"
        echo release.bat is found
        exit /b 1
      ) else (
          echo release.bat not found
          exit /b 1
      )

    echo Release process completed. Merging changes to develop...

    @REM REM Store the current commit hash
    @REM for /f "delims=" %%i in ('git rev-parse HEAD') do set "current_commit=%%i"

    @REM REM Switch to develop branch
    @REM git checkout develop

    @REM REM Pull latest changes from remote develop
    @REM git pull origin develop

    @REM REM Merge changes from main
    @REM git merge !current_commit!

    @REM echo Merged changes from main to develop. Please review and push manually if everything looks good.
) else (
    echo Current branch is !current_branch!. Skipping pre-push checks and merge.
)

endlocal