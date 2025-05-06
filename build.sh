#!/bin/bash
echo "Building for Linux/macOS..."
mcs ExtractResources.cs -out:ExtractResources -r:System.Drawing
chmod +x ExtractResources
echo "Build complete! Use: mono ExtractResources"
