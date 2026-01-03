# LogScraper

**LogScraper** is a powerful, standalone tool for retrieving, filtering, and analyzing logs from HTTP endpoints, Kubernetes clusters, or local files. It's designed to help developers and operators inspect logs quickly and efficiently.

![image](https://github.com/user-attachments/assets/60c5bb6d-0830-4b20-867a-c89575853e0a)


---

## 🔍 Key Features

- ✅ Retrieve logs from HTTP endpoints, Kubernetes, and local file systems  
- 🔄 Concatenate multiple log downloads for seamless reading  
- 📡 Continuous log reading for live monitoring  
- 🧩 Filter and hide metadata for cleaner views  
- ⚡ Fast navigation via metadata and content filters  
- 🔍 Search within logs  
- 📤 Export to external tools like Notepad++  
- 🛠️ Fully configurable UI  
- 🗃️ Single standalone executable — no installer or .NET Framework required  
- 🔔 Automatic update checks  
- 📌 Mini Controls: compact always-on-top UI for quick log access  
- 🔐 Secure credential storage using Windows Credential Manager 

---

## 📦 Download

Get the latest release from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

---

## ⚙️ Before You Start

Download the installer and run it or download the standalone ZIP and extract it to a desired location. 

Built in C# (.NET), the application does **not** require a separate .NET runtime.

---

## 🔄 Updates

### Recommended: Use the installer
The preferred way to update Logscraper is to **download and run the latest installer** from the [Releases section](https://github.com/Rambo3000/logscraper/releases).

When switching from standalone to installer, you can import settings from the standalone version in the installed version using the settings window. The JSON files adjecent to the standalone can be imported one at a time.

---

### Alternative: Standalone (ZIP) update
If the installer does not work for your setup, you can update manually using the **standalone ZIP**:

1. Download the latest ZIP from the [Releases section](https://github.com/Rambo3000/logscraper/releases)
2. **Replace the executable file** with the new version

You can **keep your existing configuration files**  
(`LogScraperConfig.json`, `LogScraperLogLayouts.json`, `LogScraperLogProviders.json`) — there is no need to change or reconfigure them unless specified in the release notes.


---

## 🛠️ Configuration

Open the **Settings** window via the ⚙️ icon. Most options are self-explanatory.

LogScraper uses a **layout system** to define how logs are interpreted. Each log line is treated as a combination of:

- A **timestamp**
- **Metadata fields**
- The actual **log content**

You can configure:
- The **timestamp format** (used for parsing)
- Which **metadata fields** are present, on which you filter the log
- Log content filters for easy navigating within the log
- Transformers which can inverse the log or extract JSON from a line

---

## 🚀 Using the Application

1. Select a log source (HTTP, Kubernetes, or file system) and select the corresponding detailed source information like a filename or a Kubernetes pod
2. (optional) Select a log layout, or use the default
3. Click **Record** or **Record for several minutes** to start loading logs
4. (optional) Filter on specified metadata
5. (optional) Search for a specific word or use the content filter to the right of the screen to quickly navigate to a specific line in the log or to pick a specific beginn and end of the log

---

## 💡 Tips & Recommendations

- 🪟 Use **Mini Controls**: a compact always-on-top window for easy log reading without switching apps  
- 🌲 Enable hierarchical navigation by setting up Begin/End Content Filters in the layout section of Settings. This groups related log lines into a hierarchical view/tree, making large logs easier to scan and explore.
- 👁️ By default, metadata is hidden — enable it via the **Metadata** section, or change the defaults in Settings
- ✂️ Keep logs readable by selectively showing metadata values inline  
- ⚠️ Ensure the correct **log layout** is selected to avoid parsing issues  
- 🧠 Memory usage depends on log size. As a guideline, keep logs under **100,000 lines** to avoid performance issues  
- 🔑 When accessing protected HTTP sources, the app will prompt for credentials and store them securely  
- 📝 For advanced search/editing, use **Notepad++** via the **Open in Notepad++** button (configurable in Settings)  
- 🔐 Credentials are stored securely via the **Windows Credential Manager** and can be managed from there  

---

## 📧 Feedback

Found a bug or want to request a feature? Please open an issue on the [Issues page](https://github.com/Rambo3000/logscraper/issues).

---

