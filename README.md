# 🔍 Resource Extraction Utility

![ASCII Logo](img/logo.png)  
*A powerful tool for extracting embedded resources from .NET assemblies*

[![Windows Build](https://img.shields.io/badge/Windows-Supported-green?logo=windows)](https://github.com/bl4d3rvnner7/ResourceExtractor)
[![Linux Build](https://img.shields.io/badge/Linux-Supported-green?logo=linux)](https://github.com/bl4d3rvnner7/ResourceExtractor)
[![macOS Build](https://img.shields.io/badge/macOS-Supported-green?logo=apple)](https://github.com/bl4d3rvnner7/ResourceExtractor)

## 📥 Installation

### Prerequisites
- **Windows**: .NET Framework 4.0+ or .NET Core 3.1+
- **Linux/macOS**: Mono runtime (`sudo apt install mono-complete`)

### Quick Start
```bash
# Clone repository
git clone https://github.com/bl4d3rvnner7/ResourceExtractor.git
cd ResourceExtractor
```

## 🛠 Building

### Windows
```batch
build.bat
```

### Linux/macOS
```bash
chmod +x build.sh
./build.sh
```

## 🚀 Usage

```bash
# Windows
ExtractResources.exe Resources.resources

# Linux/macOS
mono ExtractResources Resources.resources
```

![Example Usage](img/example.png)  
*Example output showing resource extraction*

## ✨ Features

- **Multi-format Support**  
  Extracts binaries (EXE/DLL), text, images (PNG/BMP), and ZIP archives
- **Smart Detection**  
  Auto-detects file types using magic numbers
- **Organized Output**  
  Creates `extracted_bin/`, `extracted_txt/`, and `extracted_img/` directories
- **Cross-platform**  
  Works on Windows, Linux, and macOS
- **Clean Output**  
  Color-coded console output with timestamps

## 🏗 Code Structure

```csharp
// Core Functions:
- FileTypeDetector      // Magic number detection
- ResourceParser        // .resources file handling 
- Logger               // Colorful console output
```

## 📜 License

MIT License - See [LICENSE](LICENSE) for details

---

> **Pro Tip**: Drag and drop `.resources` files directly onto the executable for quick extraction!

[![Open in Visual Studio Code](https://img.shields.io/badge/-Open%20in%20VSCode-blue?logo=visualstudiocode)](https://vscode.dev/github/bl4d3rvnner7/ResourceExtractor)