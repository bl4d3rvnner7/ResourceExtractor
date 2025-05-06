using System;
using System.Collections;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

public class AdvancedResourceExtractor
{
    // Console colors
    private static ConsoleColor infoColor = ConsoleColor.Cyan;
    private static ConsoleColor successColor = ConsoleColor.Green;
    private static ConsoleColor warningColor = ConsoleColor.Yellow;
    private static ConsoleColor errorColor = ConsoleColor.Red;
    private static ConsoleColor highlightColor = ConsoleColor.Magenta;

    public static void ExtractResources(string resourcesFilePath)
    {
        try
        {
            // Create output directories
            Directory.CreateDirectory("extracted_bin");
            Directory.CreateDirectory("extracted_txt");
            Directory.CreateDirectory("extracted_img");
            
            Log("Initializing resource extraction...", infoColor);
            Log($"Processing file: {Path.GetFileName(resourcesFilePath)}", highlightColor);

            using (var stream = File.OpenRead(resourcesFilePath))
            using (var reader = new ResourceReader(stream))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    string key = entry.Key.ToString();
                    object resourceValue = RuntimeHelpers.GetObjectValue(entry.Value);

                    Log($"Found resource: {key}", infoColor);

                    if (resourceValue is string stringValue)
                    {
                        string txtPath = Path.Combine("extracted_txt", $"{key}.txt");
                        File.WriteAllText(txtPath, stringValue);
                        LogSuccess($"Extracted string resource to {txtPath}");
                    }
                    else if (resourceValue is byte[] byteArray)
                    {
                        string extension = DetectFileExtension(byteArray);
                        string binPath = Path.Combine("extracted_bin", $"{key}{extension}");
                        File.WriteAllBytes(binPath, byteArray);
                        
                        LogSuccess($"Extracted binary resource to {binPath} ({byteArray.Length} bytes)");

                        // Try to detect if it's actually a hidden string
                        TryDetectHiddenString(byteArray, key);
                    }
                    else if (resourceValue is System.Drawing.Bitmap bitmap)
                    {
                        string imgPath = Path.Combine("extracted_img", $"{key}.png");
                        bitmap.Save(imgPath, ImageFormat.Png);
                        LogSuccess($"Extracted bitmap resource to {imgPath}");
                    }
                    else
                    {
                        LogWarning($"Unsupported resource type for {key}: {resourceValue?.GetType().Name ?? "null"}");
                    }
                }
            }
            
            LogSuccess("Resource extraction completed successfully!");
        }
        catch (Exception ex)
        {
            LogError($"Error extracting resources: {ex.Message}");
        }
    }

    private static string DetectFileExtension(byte[] data)
    {
        var signatures = new Dictionary<string, byte[]>
        {
            { ".exe", new byte[] { 0x4D, 0x5A } },
            { ".dll", new byte[] { 0x4D, 0x5A } },
            { ".zip", new byte[] { 0x50, 0x4B, 0x03, 0x04 } },
            { ".png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
        };

        foreach (var sig in signatures)
        {
            if (data.Length >= sig.Value.Length)
            {
                bool match = true;
                for (int i = 0; i < sig.Value.Length; i++)
                {
                    if (data[i] != sig.Value[i])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return sig.Key;
            }
        }
        return ".bin";
    }

    private static void TryDetectHiddenString(byte[] data, string key)
    {
        bool likelyText = true;
        foreach (byte b in data)
        {
            if (b < 32 && b != 0x0A && b != 0x0D && b != 0x09)
            {
                likelyText = false;
                break;
            }
        }

        if (likelyText)
        {
            try
            {
                string txtPath = Path.Combine("extracted_txt", $"{key}_as_text.txt");
                string possibleText = System.Text.Encoding.UTF8.GetString(data);
                File.WriteAllText(txtPath, possibleText);
                LogSuccess($"Found possible text in binary resource: {txtPath}");
            }
            catch
            {
                // Ignore encoding errors
            }
        }
    }

    // Logging methods with colors and formatting
    private static void Log(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [R-Extractor] {message}");
        Console.ResetColor();
    }

    private static void LogSuccess(string message) => Log($"✓ {message}", successColor);
    private static void LogWarning(string message) => Log($"⚠ {message}", warningColor);
    private static void LogError(string message) => Log($"✗ {message}", errorColor);

    static void Main(string[] args)
    {
        Console.Title = "Resource Extractor Pro";
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($@"
    ▄▄▄  ▄▄▄ .▐▄• ▄  ▄▄▄·▄▄▄        
    ▀▄ █·▀▄.▀· █▌█▌▪▐█ ▄█▀▄ █·▪     
    ▐▀▀▄ ▐▀▀▪▄ ·██·  ██▀·▐▀▀▄  ▄█▀▄ 
    ▐█•█▌▐█▄▄▌▪▐█·█▌▐█▪·•▐█•█▌▐█▌.▐▌
    .▀  ▀ ▀▀▀ •▀▀ ▀▀.▀   .▀  ▀ ▀█▄▀▪
        ");
        Console.ResetColor();
    
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("    === Resource Extraction Utility ===");
        Console.WriteLine("    ===        Version 1.0.0        ===");
        Console.ResetColor();
        Console.WriteLine();

        if (args.Length == 0)
        {
            LogError("    Usage: ResourceExtractor.exe <Resources.resources>");
            LogWarning("    Drag and drop a .resources file onto this executable to extract it");
            Console.ReadKey();
            return;
        }

        string inputFile = args[0];
        if (!File.Exists(inputFile))
        {
            LogError($"    File not found: {inputFile}");
            Console.ReadKey();
            return;
        }

        ExtractResources(inputFile);
        
        // Keep console open if running directly
        if (Debugger.IsAttached || args.Length == 0)
        {
            Console.WriteLine("\n    Press any key to exit...");
            Console.ReadKey();
        }
    }
}