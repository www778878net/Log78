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
    echo 当前分支是 main。运行发布流程...

    REM 直接在 pre-push.bat 中执行发布流程，而不是调用 release.bat
    
    REM 获取项目名称
    for %%I in (.) do set "project_name=%%~nxI"
    echo 处理项目：%project_name%

    REM 设置项目文件路径
    set "project_file=src\%project_name%\%project_name%.csproj"

    REM 检查项目文件是否存在
    if not exist "%project_file%" (
        echo 未找到项目文件：%project_file%
        echo 退出...
        exit /b 1
    )

    echo 找到项目文件：%project_file%

    REM 获取当前版本
    for /f "tokens=3 delims=<>" %%a in ('findstr "<PackageVersion>" "!project_file!"') do set "current_version=%%a"
    if not defined current_version (
        echo 在 !project_file! 中未找到 PackageVersion。退出...
        exit /b 1
    )
    echo 当前版本：!current_version!

    REM 构建发布版本
    echo 构建发布版本...
    dotnet build -c Release
    if !errorlevel! neq 0 (
        echo 构建失败。退出...
        exit /b 1
    )

    REM 运行测试
    echo 运行测试...
    dotnet test
    if !errorlevel! neq 0 (
        echo 测试失败。退出...
        exit /b 1
    )

    REM 增加补丁版本号
    for /f "tokens=1-3 delims=." %%a in ("!current_version!") do (
        set /a patch=%%c+1
        set "new_version=%%a.%%b.!patch!"
    )
    echo 新版本：!new_version!

    REM 更新项目文件中的版本号
    powershell -Command "(Get-Content '!project_file!') -replace '<PackageVersion>!current_version!</PackageVersion>', '<PackageVersion>!new_version!</PackageVersion>' | Set-Content '!project_file!'"

    echo !project_name! (!project_file!) 的版本已更新

    REM 暂存更改的项目文件
    git add "!project_file!"

    REM 提交版本变更
    git commit -m "将版本号提升到 !new_version!"

    REM 创建带注释的标签
    git tag -a v!new_version! -m "发布版本 !new_version!"

    echo 发布流程完成。新版本 %new_version% 已标记。请手动推送更改和标签。

    REM 注意：不要在这里自动推送，以避免循环触发 pre-push 钩子

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