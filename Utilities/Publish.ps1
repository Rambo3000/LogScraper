# Execute this PS1 file via Terminal in View
param(
    [switch]$test,

    [ValidateSet("major", "minor", "patch")]
    [string]$bump = "patch"
)

$projectFilePath = "LogScraper.csproj"
[xml]$projectFile = Get-Content $projectFilePath
$propertyGroup = $projectFile.Project.PropertyGroup | Where-Object { $_.Condition -eq $null }

if ($test.IsPresent) {
    $version = $propertyGroup.Version
    $date = Get-Date -Format "yyyyMMdd_HHmmss"
    $destination = ".\bin\LogScraper $version beta $date.zip"
    Write-Host "----- Created new test version $version $date -----"
}
else {
    $currentVersion = $propertyGroup.Version
    [int[]]$versionComponents = $currentVersion -split '\.'

    if ($versionComponents.Count -ne 3) {
        throw "Version '$currentVersion' must be in format Major.Minor.Patch"
    }

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

    $newVersion = $versionComponents -join '.'
    $propertyGroup.Version = $newVersion
    $propertyGroup.FileVersion = $newVersion

    $destination = ".\bin\LogScraperStandalone-$newVersion.zip"
    $projectFile.Save($projectFilePath)

    Write-Host "----- Updated version numbers to $newVersion (bump: $bump) -----"
}

Write-Host "----- Publishing -----"
dotnet publish -r win-x64 -c Release --nologo --self-contained

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
    & "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "/DMyAppVersion=`"$newVersion`"" ".\Utilities\Installer\Settings.iss"
}

Write-Host "----- Cleaning up -----"
Remove-Item ".\bin\Release\" -Recurse -Force

Write-Host "----- Open explorer -----"
Start ".\bin\"