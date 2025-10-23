#!/bin/bash

## This is an script on macOs. You need to save the file
## as setup-aspnet.sh and make it executable on terminal
## with the following command: 
# chmod +x setup-aspnet.sh
## and run it with the following command:
# ./setup-aspnet.sh

echo "🚀 Setting up ASP.NET Core development environment on macOS..."

# Check if Homebrew is installed
if ! command -v brew &> /dev/null; then
echo "📦 Installing Homebrew..."
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
else
echo "✅ Homebrew already installed"
fi

# Install .NET SDK
if ! command -v dotnet &> /dev/null; then
echo "📦 Installing .NET SDK..."
brew install --cask dotnet-sdk
else
echo "✅ .NET SDK already installed"
dotnet --version
fi

# Install VS Code
if ! command -v code &> /dev/null; then
echo "📦 Installing Visual Studio Code..."
brew install --cask visual-studio-code
else
echo "✅ VS Code already installed"
fi

# Install VS Code extensions for ASP.NET Core
echo "📦 Installing VS Code extensions..."
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.csdevkit
code --install-extension ms-dotnettools.vscode-dotnet-runtime
code --install-extension jchannon.csharpextensions