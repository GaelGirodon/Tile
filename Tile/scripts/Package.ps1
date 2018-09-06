#
# Package.ps1
# Package executable for release
#

# Exit script on first error
$ErrorActionPreference = "Stop"

# Path to the script
$WorkingDir = $PSScriptRoot

# Check that Tile.exe file is present in Release directory
if (!(Test-Path "$WorkingDir\..\Tile.GUI\bin\Release\Tile.exe")) {
    Write-Error "Tile.GUI must first be built in Release configuration"
    exit 1
}

# Absolute path to bin directory
$BinDir = (Resolve-Path $WorkingDir\..\Tile.GUI\bin\)
# Release directory
$ReleaseDir = "$BinDir\Release"

# Temporary directory used in archive creation
$TmpDir = "$BinDir\Tmp"
# Package directory
$PackageDir = "$BinDir\Package"

# Generated .zip file
$ZipFile = "$PackageDir\Tile.zip"
# Generated file containing checksum
$ChecksumFile = "$PackageDir\Tile.zip.sha256"

# Clean previous generated files
Write-Host -ForegroundColor Yellow "Cleaning previous generated files"
if (Test-Path $PackageDir) {
    Remove-Item -Recurse $PackageDir
}

# Create Package directory
New-Item -Path $PackageDir -ItemType Directory | Out-Null

# Prepare files to compress
Write-Host -ForegroundColor Yellow "Preparing files to compress in $TmpDir"
New-Item -Path $TmpDir -ItemType Directory | Out-Null
Copy-Item -Recurse -Container -Path $ReleaseDir\* -Include *.exe, *.dll, fr-FR -Destination $TmpDir

# Compress files into one archive => Tile.zip
Write-Host -ForegroundColor Yellow "Compressing files to $ZipFile"
Compress-Archive -Path $TmpDir\* -CompressionLevel Optimal -DestinationPath $ZipFile

# Remove temporary files
Write-Host -ForegroundColor Yellow "Cleaning temporary files"
Remove-Item -Recurse $TmpDir

# Compute checksum
Write-Host -ForegroundColor Yellow "Computing and writing checksum to $ChecksumFile"
$Hash = (Get-FileHash -Algorithm SHA256 $ZipFile).Hash.ToLower()
Set-Content -Path $ChecksumFile -Value "$Hash *Tile.zip`n" -Encoding ASCII -NoNewline

# Success message
Write-Host -ForegroundColor Green "Package successfully generated!"
Get-ChildItem $PackageDir
