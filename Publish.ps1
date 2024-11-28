# Execute this PS1 file via Terminal in View
param(
    [switch]$test
)

$projectFilePath = "LogScraper.csproj"
[xml]$projectFile = Get-Content $projectFilePath
$propertyGroup = $projectFile.Project.PropertyGroup | Where-Object { $_.Condition -eq $null }

if ($test.IsPresent) {
    $version = $propertyGroup.Version
    $date = Get-Date -Format "yyyyMMdd_HHmmss"
    $destination = ".\bin\LogScraper " + $version + " beta $date.zip"
    Write-Host "----- Created new test version $version $date -----"
} else {
    $currentVersion = $propertyGroup.Version
    $versionComponents = $currentVersion -split '\.'
    $versionComponents[2] = [int]$versionComponents[2] + 1
    $newVersion = $versionComponents -join '.'
    $propertyGroup.Version = $newVersion
    $propertyGroup.FileVersion = $newVersion

    $destination = ".\bin\LogScraper " + $newVersion + ".zip"
    $projectFile.Save($projectFilePath)
    Write-Host "----- Updated version numbers to $newVersion -----"
}

Write-Host "----- Publishing -----" 
dotnet publish -r win-x64 -c Release --nologo --self-contained

Write-Host "----- Moving configuration file -----" 
Move-Item -Path ".\bin\Release\net9.0-windows7.0\win-x64\publish\Configuration\LogScraperConfig.json" -Destination ".\bin\Release\net9.0-windows7.0\win-x64\publish\LogScraperConfig.json" -Force
Move-Item -Path ".\bin\Release\net9.0-windows7.0\win-x64\publish\Configuration\LogScraperLogLayouts.json" -Destination ".\bin\Release\net9.0-windows7.0\win-x64\publish\LogScraperLogLayouts.json" -Force
Move-Item -Path ".\bin\Release\net9.0-windows7.0\win-x64\publish\Configuration\LogScraperLogProviders.json" -Destination ".\bin\Release\net9.0-windows7.0\win-x64\publish\LogScraperLogProviders.json" -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\Configuration" -Force

Write-Host "----- Cleaning publish folder -----" 
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\Stubs" -Recurse -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\LogScraper.pdb" -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\D3DCompiler_47_cor3.dll" -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\PenImc_cor3.dll" -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\vcruntime140_cor3.dll" -Force
Remove-item ".\bin\Release\net9.0-windows7.0\win-x64\publish\wpfgfx_cor3.dll" -Force

Write-Host "----- Packing -----" 
$source = ".\bin\Release\net9.0-windows7.0\win-x64\publish\"
If(Test-path $destination) {Remove-item $destination}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($Source, $destination)

Write-Host "----- Cleaning up -----"
Remove-item ".\bin\Release\" -Recurse -Force

Write-Host "----- Open explorer -----"
Start ".\bin\"
