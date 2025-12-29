; LogScraperInstaller.iss
; Per-user installer (no elevation). JSON files: installed only if they don't exist and kept on uninstall.

[Setup]
AppName=LogScraper
; AppVersion is provided by the Inno preprocessor (see your PS build script / ISCC /DMyAppVersion=...)
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
VersionInfoProductVersion={#MyAppVersion}
; stable AppId -- keep the same GUID between releases so upgrade works correctly
AppId={{7f3a9e2b-4c1a-4f5b-9e2a-123456789abc}}
DefaultDirName={localappdata}\LogScraper
PrivilegesRequired=none
OutputBaseFilename=LogScraperInstaller-{#MyAppVersion}
; Place produced installer in repo bin folder next to your ZIP (optional)
OutputDir=..\..\bin
Compression=lzma
SolidCompression=yes
; Do not ask the user to select a program group for the Start Menu
DisableProgramGroupPage=yes
; Do not show a summery page
DisableReadyPage=yes
; Disabled icon because it was not showing correctly at all sizes
SetupIconFile=..\..\Icons\Scraper Installer.ico 

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Main executable from publish folder
Source: "..\..\bin\Release\net10.0-windows\win-x64\publish\LogScraper.exe"; DestDir: "{app}"; Flags: ignoreversion

; Supporting runtime files (only include the ones you need; example kept minimal)
; Copy DLLs or other files if required, adjust as needed
; Source: "..\..\bin\Release\net10.0-windows\win-x64\publish\SomeDependency.dll"; DestDir: "{app}"; Flags: ignoreversion

; JSON config files: only install if they do not already exist, and never remove on uninstall
Source: "..\..\bin\Release\net10.0-windows\win-x64\publish\LogScraperConfig.json"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall ignoreversion
Source: "..\..\bin\Release\net10.0-windows\win-x64\publish\LogScraperLogLayouts.json"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall ignoreversion
Source: "..\..\bin\Release\net10.0-windows\win-x64\publish\LogScraperLogProviders.json"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall ignoreversion

[Icons]
Name: "{group}\LogScraper"; Filename: "{app}\LogScraper.exe"
Name: "{userdesktop}\LogScraper"; Filename: "{app}\LogScraper.exe"; Tasks: desktopicon

[Tasks]
Name: desktopicon; Description: "Create a desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Run]
Filename: "{app}\LogScraper.exe"; Description: "Launch LogScraper"; Flags: nowait postinstall skipifsilent

