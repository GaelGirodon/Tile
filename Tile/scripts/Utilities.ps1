#
# Utilities.ps1
# Tile utilities
#

# Create fake shortcuts and exe files to test Tile
# on a sample "data set"
function FakeShortcuts([string] $TileConfig, [string] $WorkingDirectory) {
    # Check parameters
    if (!$WorkingDirectory -or !(Test-Path $WorkingDirectory)) {
        Write-Error "The working directory path can't be found"
        return 1
    }
    $workDir = (Resolve-Path $WorkingDirectory)
    if (!$TileConfig -or !(Test-Path $TileConfig)) {
        Write-Error "The tile configuration file can't be found"
        return 2
    }

    # Parse configuration file
    $tilesConfigRaw = (Get-Content $TileConfig | ConvertFrom-Json)
    $tiles = @{}
    foreach ($property in $tilesConfigRaw.psobject.properties.name) {
        $tiles[$property] = $tilesConfigRaw.$property
    }

    # Prepare fake programs and start menu directories
    $programs = "$workDir\Programs"
    New-Item -ItemType Directory -Force -Path $programs
    $startMenu = "$workDir\StartMenu"
    New-Item -ItemType Directory -Force -Path $startMenu

    # Create fake .exe files and associated shortcuts
    $wshShell = New-Object -comObject WScript.Shell
    foreach ($app in $tiles.Keys) {
        New-Item -ItemType Directory -Force -Path "$programs\$app"
        Write-Output "fake" | Out-File "$programs\$app\$app.exe"
        $shortcut = $wshShell.CreateShortcut("$startMenu\$app.lnk")
        $shortcut.TargetPath = "$programs\$app\$app.exe"
        $shortcut.Save()
    }

    # Generate settings.json file
    $shortcutsLocation = $startMenu.Replace("\", "\\")
    $settings = "{`n`t`"ShortcutsLocations`": [`"$shortcutsLocation`"],`n`t`"Overwrite`": true`n}"
    Write-Output $settings | Out-File "$workDir\settings.json"
}