#
# Release.ps1
# Release a new version
#

# Parameters
Param (
    [string] $Version
)

$VersionRegex = "[0-9]+\.[0-9]+\.[0-9]+(-[\w]+)?"
$WorkingDir = $PSScriptRoot

# Parameters check
if (!$Version -or !($Version -match $VersionRegex)) {
    Write-Error "The next version number is invalid"
    exit 2
}

# Start release
Write-Host -ForegroundColor Yellow "Starting release (git flow release start $Version)"
Pause
git flow release start "$Version"
if (!$?) {
    Write-Error "Failed starting release ($LASTEXITCODE)"
    exit 11
}

# TODO Bump version number

# Finish release
Write-Host -ForegroundColor Yellow "Finishing release (git flow release finish $Version)"
Pause
git flow release finish "$Version"
if (!$?) {
    Write-Error "Failed finishing release ($LASTEXITCODE)"
    exit 12
}

# Push changes
Write-Host -ForegroundColor Yellow "Pushing changes"
Pause
git push --all origin
git push --tags
if (!$?) {
    Write-Error "Failed pushing changes ($LASTEXITCODE)"
    exit 21
}
