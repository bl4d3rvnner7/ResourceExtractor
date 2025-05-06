@echo off
echo Building for Windows
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe ExtractResources.cs -reference:System.Drawing.dll -out:ExtractResources.exe
echo Build complete! Use: ExtractResources.exe
pause