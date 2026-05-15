# LogScraper

**LogScraper** is a standalone tool for retrieving, filtering, and analyzing logs from HTTP endpoints, Kubernetes clusters, and local files. It is built to help developers and operators inspect logs quickly and efficiently.

![image](https://github.com/user-attachments/assets/60c5bb6d-0830-4b20-867a-c89575853e0a)


---

## 🔍 Key Features

- Read logs from HTTP endpoints, Kubernetes, and local files  
- Filter large logs quickly using metadata fields  
- Navigate quickly with content filters and begin/end markers  
- Find problem areas fast with the timeline and error indicators  
- Follow related log lines in a hierarchical flow view  
- Color log lines  
- Monitor logs live with continuous reading  
- Configure log layouts with custom timestamps, metadata, and content rules  
- Pretty print JSON and XML inside log lines  
- Read raw JSON logs more easily with JSON path extraction  
- Combine multiple log downloads into one seamless view  
- Use Mini Controls for compact, always-on-top log access  
- Run as an installer or standalone app without a separate .NET runtime  
- Stay up to date with automatic update checks  

---

## 📦 Download

Get the latest release from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

---

## ⚙️ Before You Start

Download and run the installer, or download the standalone ZIP and extract it to a location of your choice.

Built in C# (.NET), the application does **not** require a separate .NET runtime.

---

## 🔄 Updates

### Recommended: Use the installer
The preferred way to update LogScraper is to **download and run the latest installer** from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

If you switch from the standalone version to the installer version, you can import your existing settings through the Settings window. The JSON files next to the standalone executable can be imported one at a time.

---

### Alternative: Standalone (ZIP) update
If the installer does not work for your setup, you can update manually using the **standalone ZIP**:

1. Download the latest ZIP from the [Releases section](https://github.com/Rambo3000/logscraper/releases)
2. **Replace the executable file** with the new version

You can **keep your existing configuration files**  
(`LogScraperConfig.json`, `LogScraperLogLayouts.json`, `LogScraperLogProviders.json`) — there is no need to change or reconfigure them unless specified in the release notes.


---

## 🛠️ Configuration

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

---

## 🚀 Using the Application

1. Select a log source (HTTP, Kubernetes, or file system), then choose the specific source details such as a filename or Kubernetes pod
2. (Optional) Select a log layout, or use the default
3. Click **Record** or **Record for several minutes** to start loading logs
4. (Optional) Filter by metadata
5. (Optional) Restrict the range of log lines using begin/end markers
6. (Optional) Use the timeline, navigation or search to jump to a relevant section of the log

---

## 💡 Tips & Recommendations

- 🪟 Use **Mini Controls** for a compact always-on-top window while working in other apps  
- 🌲 Enable hierarchical navigation by configuring Begin/End Content Filters in Settings. This groups related log lines into a tree view and makes large logs easier to scan.
- 🎨 Use content properties to color log lines and make patterns easier to spot
- 👁️ Metadata is hidden by default. Enable it in the **Metadata** section or change the default in Settings
- ✂️ Keep logs readable by selectively showing metadata values inline  
- ⚠️ Ensure the correct **log layout** is selected to avoid parsing issues  
- 🧠 Memory usage depends on log size. As a guideline, keep logs under **100,000 lines** to avoid performance issues  

---

## 📧 Feedback

Found a bug or want to request a feature? Please open an issue on the [Issues page](https://github.com/Rambo3000/logscraper/issues).

---

