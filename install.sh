#!/bin/bash
# ipgeo installer

set -e

INSTALL_DIR="${HOME}/.local/bin"
BINARY_NAME="ipgeo"

# Detect OS and architecture
OS=$(uname -s)
ARCH=$(uname -m)

case "$OS" in
    Darwin)
        if [ "$ARCH" = "arm64" ]; then
            BINARY="ipgeo/publish/ipgeo"
        else
            echo "Unsupported architecture: $ARCH"
            exit 1
        fi
        ;;
    Linux)
        if [ "$ARCH" = "x86_64" ]; then
            BINARY="ipgeo/publish-linux/ipgeo"
        else
            echo "Unsupported architecture: $ARCH"
            exit 1
        fi
        ;;
    *)
        echo "Unsupported OS: $OS"
        exit 1
        ;;
esac

# Create install directory
mkdir -p "$INSTALL_DIR"

# Copy binary
cp "$BINARY" "$INSTALL_DIR/$BINARY_NAME"
chmod +x "$INSTALL_DIR/$BINARY_NAME"

echo "Installed ipgeo to $INSTALL_DIR/$BINARY_NAME"
echo ""
echo "Make sure $INSTALL_DIR is in your PATH:"
echo '  export PATH="$HOME/.local/bin:$PATH"'
echo ""
echo "Then run: ipgeo --help"