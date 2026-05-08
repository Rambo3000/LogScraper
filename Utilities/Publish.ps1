param(
    [switch]$help,
    [switch]$test,
    [switch]$noBump,

    [ValidateSet("major", "minor", "patch")]
    [string]$bump = "",

    [ValidateSet("", "alpha", "beta")]
    [string]$prerelease = "",

    [int]$prereleaseNumber = 0
)

if ($help -or (-not $test -and -not $noBump -and -not $bump -and -not $prerelease -and $prereleaseNumber -eq 0)) {
    Write-Host ""
    Write-Host "USAGE"
    Write-Host "  .\Utilities\Publish.ps1 [options]"
    Write-Host ""
    Write-Host "OPTIONS"
    Write-Host "  -bump <major|minor|patch>       Version segment to increment (default when not specified: patch)"
    Write-Host "  -noBump                         Re-publish current version without changing it"
    Write-Host "  -prerelease <alpha|beta>        Append a pre-release label (e.g. -alpha.1)"
    Write-Host "  -prereleaseNumber <n>           Force a specific pre-release number (default: auto)"
    Write-Host "  -test                           Build only - no version bump, no installer, no git tag"
    Write-Host "  -help                           Show this help message"
    Write-Host ""
    Write-Host "EXAMPLES"
    Write-Host "  Stable releases:"
    Write-Host "    .\Utilities\Publish.ps1 -bump patch                       # bump patch  e.g. 3.2.11"
    Write-Host "    .\Utilities\Publish.ps1 -bump minor                       # bump minor  e.g. 3.3.0"
    Write-Host "    .\Utilities\Publish.ps1 -bump major                       # bump major  e.g. 4.0.0"
    Write-Host "    .\Utilities\Publish.ps1 -noBump                           # re-publish current version"
    Write-Host ""
    Write-Host "  Pre-release:"
    Write-Host "    .\Utilities\Publish.ps1 -bump major -prerelease alpha     # 4.0.0-alpha.1"
    Write-Host "    .\Utilities\Publish.ps1 -noBump    -prerelease alpha      # 4.0.0-alpha.2 (auto)"
    Write-Host "    .\Utilities\Publish.ps1 -noBump    -prerelease beta       # 4.0.0-beta.1"
    Write-Host "    .\Utilities\Publish.ps1 -noBump    -prerelease alpha -prereleaseNumber 5"
    Write-Host ""
    Write-Host "  Test build:"
    Write-Host "    .\Utilities\Publish.ps1 -test"
    Write-Host "    .\Utilities\Publish.ps1 -test -prerelease alpha"
    Write-Host ""
    exit 0
}

$ErrorActionPreference = 'Stop'

$projectFilePath = "LogScraper.csproj"
[xml]$projectFile = Get-Content $projectFilePath
$propertyGroup = $projectFile.Project.PropertyGroup | Where-Object { $_.Condition -eq $null }

# Warn on uncommitted changes (excluding the .csproj, which this script manages itself)
if (!$test.IsPresent) {
    $gitStatus = git status --porcelain 2>$null | Where-Object { $_ -notmatch [regex]::Escape($projectFilePath) }
    if ($gitStatus) {
        Write-Warning "You have uncommitted changes. Consider committing before publishing."
        $confirm = Read-Host "Continue anyway? (y/N)"
        if ($confirm -ne 'y') { exit 1 }
    }
}

if ($test.IsPresent) {
    $version = $propertyGroup.Version
    if ($prerelease) {
        $resolvedPrereleaseNumber = 1
        if ($prereleaseNumber -gt 0) {
            $resolvedPrereleaseNumber = $prereleaseNumber
        } else {
            $existingInfo = $propertyGroup.InformationalVersion
            if ($existingInfo -match "^$([regex]::Escape($version))-$([regex]::Escape($prerelease))\.(\d+)$") {
                $resolvedPrereleaseNumber = [int]$Matches[1] + 1
            }
        }
        $displayVersion = "$version-$prerelease.$resolvedPrereleaseNumber"
    } else {
        $displayVersion = $version
    }
    $date = Get-Date -Format "yyyyMMdd_HHmmss"
    $destination = ".\bin\LogScraper $displayVersion $date.zip"
    Write-Host "----- Created new test version $displayVersion $date -----"
}
else {
    $currentVersion = $propertyGroup.Version
    [int[]]$versionComponents = $currentVersion -split '\.'

    if ($versionComponents.Count -ne 3) {
        throw "Version '$currentVersion' must be in format Major.Minor.Patch"
    }

    if ($prerelease -and -not $noBump.IsPresent -and -not $bump) {
        $noBump = $true
    }

    if (!$noBump.IsPresent) {
        if ($bump -eq "major") {
            $versionComponents[0]++
            $versionComponents[1] = 0
            $versionComponents[2] = 0
        }
        elseif ($bump -eq "minor") {
            $versionComponents[1]++
            $versionComponents[2] = 0
        }
        else {
            $versionComponents[2]++
        }
    }

    $newVersion = $versionComponents -join '.'

    # Auto-increment prerelease number: if the current InformationalVersion matches
    # the same base version and label, increment its number; otherwise start at 1.
    if ($prerelease) {
        $resolvedPrereleaseNumber = 1
        if ($prereleaseNumber -gt 0) {
            $resolvedPrereleaseNumber = $prereleaseNumber
        } else {
            $existingInfo = $propertyGroup.InformationalVersion
            if ($existingInfo -match "^$([regex]::Escape($newVersion))-$([regex]::Escape($prerelease))\.(\d+)$") {
                $resolvedPrereleaseNumber = [int]$Matches[1] + 1
            }
        }
        $displayVersion = "$newVersion-$prerelease.$resolvedPrereleaseNumber"
    } else {
        $resolvedPrereleaseNumber = 0
        $displayVersion = $newVersion
    }

    $propertyGroup.Version = $newVersion
    $propertyGroup.FileVersion = $newVersion

    # Write InformationalVersion for prerelease builds so the app can show the label;
    # clear it for stable builds so it falls back to the numeric version.
    $informationalVersionNode = $propertyGroup.SelectSingleNode("InformationalVersion")
    if ($prerelease) {
        if ($informationalVersionNode -eq $null) {
            $newNode = $projectFile.CreateElement("InformationalVersion")
            $newNode.InnerText = $displayVersion
            $propertyGroup.AppendChild($newNode) | Out-Null
        } else {
            $informationalVersionNode.InnerText = $displayVersion
        }
    } else {
        if ($informationalVersionNode -ne $null) {
            $propertyGroup.RemoveChild($informationalVersionNode) | Out-Null
        }
    }

    $destination = ".\bin\LogScraperStandalone-$displayVersion.zip"
    $projectFile.Save($projectFilePath)

    $bumpText = if ($noBump.IsPresent) { "no bump" } else { $bump }
    Write-Host "----- Updated version to $displayVersion (bump: $bumpText)$(if ($prerelease) { ", prerelease: $prerelease.$resolvedPrereleaseNumber" }) -----"
}

Write-Host "----- Publishing -----"
dotnet publish -r win-x64 -c Release --nologo --self-contained
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE" }

Write-Host "----- Moving configuration file -----"
Move-Item ".\bin\Release\net10.0-windows\win-x64\publish\Configuration\JsonFiles\LogScraperConfig.json" ".\bin\Release\net10.0-windows\win-x64\publish\LogScraperConfig.json" -Force
Move-Item ".\bin\Release\net10.0-windows\win-x64\publish\Configuration\JsonFiles\LogScraperLogLayouts.json" ".\bin\Release\net10.0-windows\win-x64\publish\LogScraperLogLayouts.json" -Force
Move-Item ".\bin\Release\net10.0-windows\win-x64\publish\Configuration\JsonFiles\LogScraperLogProviders.json" ".\bin\Release\net10.0-windows\win-x64\publish\LogScraperLogProviders.json" -Force
Remove-Item ".\bin\Release\net10.0-windows\win-x64\publish\Configuration" -Force -Recurse

Write-Host "----- Cleaning publish folder -----"
Remove-Item ".\bin\Release\net10.0-windows\win-x64\publish\Stubs" -Recurse -Force
Remove-Item ".\bin\Release\net10.0-windows\win-x64\publish\LogScraper.pdb" -Force

Write-Host "----- Packing -----"
$source = ".\bin\Release\net10.0-windows\win-x64\publish\"
If (Test-Path $destination) { Remove-Item $destination }
Add-Type -Assembly "System.IO.Compression.FileSystem"
[IO.Compression.ZipFile]::CreateFromDirectory($source, $destination)

Write-Host "----- Building installer -----"
if (!$test.IsPresent) {
    $innoSetup = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (!(Test-Path $innoSetup)) { throw "Inno Setup not found at: $innoSetup" }
    & $innoSetup "/DMyAppVersion=`"$displayVersion`"" "/DMyAppBaseVersion=$newVersion" ".\Utilities\Installer\Settings.iss"
    if ($LASTEXITCODE -ne 0) { throw "Inno Setup failed with exit code $LASTEXITCODE" }
}

Write-Host "----- Cleaning up -----"
Remove-Item ".\bin\Release\" -Recurse -Force

Write-Host "----- Committing -----"
# Create a git tag for the published version so GitHub releases stay in sync
if (!$test.IsPresent) {
    $tag = "v$displayVersion"
    Write-Host "----- Committing version bump and tagging as $tag -----"
    git add $projectFilePath
    git commit -m "Bump version to $displayVersion"
    git tag $tag
    $pushTag = Read-Host "Push commit and tag '$tag' to origin? (y/N)"
    if ($pushTag -eq 'y') {
        git push origin
        git push origin $tag
    }
}

Write-Host "----- Open explorer -----"
Start ".\bin\"