# LogScraper

**Investigate application flows through the noise.**

LogScraper is a fast desktop log analysis tool for investigating application flows in large logs. It retrieves logs directly from their source, processes them locally, filters noise almost instantly, and helps you trace what happened across thousands or millions of log lines.

Centralized observability platforms such as Splunk, ELK, and Grafana are great for collecting, aggregating, and searching logs. LogScraper focuses on what comes next: **understanding what actually happened**.

No centralized logging platform is required. LogScraper can work directly with logs from HTTP endpoints, Kubernetes and local files.

<img width="1515" height="819" alt="image" src="https://github.com/user-attachments/assets/d7a5eb8c-f447-4705-88ff-b0f507198d9e" />

## Built for log investigation
- Filter large logs almost instantly using metadata and log content
- Trace application flows through hierarchical log relationships
- Jump to relevant events using the timeline, markers, navigation, and search
- Spot problems quickly with error indicators and content-based highlighting
- Reduce noise by styling or de-emphasizing irrelevant log lines
- Monitor logs live while an application is running
- Inspect embedded JSON and XML directly within log lines
- Extract log content from JSON using JSONPath
- Adapt LogScraper to your logs with configurable layouts, metadata, and parsing rules

## Designed for everyday use
- Process large amounts of log data quickly while keeping filtering and navigation highly responsive, within the performance limits of the system and log source.
- Read logs directly from HTTP endpoints, Kubernetes, and local files
- Use Mini Controls for compact, always-on-top log access
- Run as a standalone application or install it without a separate .NET runtime
- Stay up to date with automatic update checks

## Security & privacy
- Retrieved logs are kept in memory during analysis and are only written to disk when explicitly requested by the user.
- Log data is processed locally and does not need to be uploaded to a third-party service or the cloud.
- Credentials are stored using the Windows Credential Manager.

## Download

Get the latest release from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

---

## Before You Start

Download and run the installer, or download the standalone ZIP and extract it to a location of your choice.

Built in C# (.NET), the application does **not** require a separate .NET runtime.

## Updates

### Recommended: Use the installer
The preferred way to update LogScraper is to **download and run the latest installer** from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

If you switch from the standalone version to the installer version, you can import your existing settings through the Settings window. The JSON files next to the standalone executable can be imported one at a time.

### Alternative: Standalone (ZIP) update
If the installer does not work for your setup, you can update manually using the **standalone ZIP**:

1. Download the latest ZIP from the [Releases section](https://github.com/Rambo3000/logscraper/releases)
2. **Replace the executable file** with the new version

You can **keep your existing configuration files**  
(`LogScraperConfig.json`, `LogScraperLogLayouts.json`, `LogScraperLogProviders.json`) — there is no need to change or reconfigure them unless specified in the release notes.

## Configuration

Open the **Settings** window via the ⚙️ icon.

LogScraper uses a **layout system** to define how logs are interpreted. Each log line is split into:

- A **timestamp**
- **Metadata fields**
- The actual **log content**

You can configure:
- Which **metadata fields** are present and available for filtering
- **Content filters** for faster navigation within the log
- **Content-based styling** to color log lines per content filter
- **Transformers** that extract values from raw JSON logs
- The **timestamp format** used for parsing (Optional)

## Configuration for Blueriq

This tool has initially been developed for the low code Blueriq platform. If you are using Blueriq you can import the follwing log layout settings:
[LogScraperSettings_LogLayouts_Blueriq_17.json](https://github.com/user-attachments/files/31946357/LogScraperSettings_LogLayouts_Blueriq_17.json)

Note: possibly metadata fields are named differently for your environment so please correct these after importing

## Using the Application

1. Select a log source (HTTP, Kubernetes, or file system), then choose the specific source details such as a filename or Kubernetes pod
2. (Optional) Select a log layout, or use the default
3. Click **Record** or **Record for several minutes** to start loading logs
4. (Optional) Filter by metadata
5. (Optional) Restrict the range of log lines using begin/end markers
6. (Optional) Use the timeline, navigation or search to jump to a relevant section of the log

## Tips & Recommendations

- Use **Mini Controls** for a compact always-on-top window while working in other apps  
- Enable hierarchical navigation by configuring Begin/End Content Filters in Settings. This groups related log lines into a tree view and makes large logs easier to scan.
- Use content properties to color log lines and make patterns easier to spot
- Metadata is hidden by default. Enable it in the **Metadata** section or change the default in Settings
- Keep logs readable by selectively showing metadata values inline  
- Ensure the correct **log layout** is selected to avoid parsing issues  

## Feedback

Found a bug or want to request a feature? Please open an issue on the [Issues page](https://github.com/Rambo3000/logscraper/issues).

---

