# ipgeo - Terminal IP Geolocator

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Build](https://github.com/588h6fnmnv-hub/ipgeo/actions/workflows/build.yml/badge.svg)
![Release](https://img.shields.io/github/v/release/588h6fnmnv-hub/ipgeo?include_prereleases)
![License](https://img.shields.io/badge/license-MIT-green)
![Platform](https://img.shields.io/badge/platform-macOS%20%7C%20Linux%20%7C%20Windows-blue)

A simple, free, open-source terminal tool to look up IP address geolocation and network information.

## Features

- **IP Lookup** - Enter any IPv4 or IPv6 address
- **Auto-detect** - `--me` flag detects your public IP automatically
- **Interactive Mode** - Run without arguments for repeated lookups
- **Private IP Fallback** - Auto-detects public IP when private IP entered
- **Reverse DNS** - PTR, A, AAAA records
- **WHOIS/RDAP** - Network registration info
- **Dark/Hacker Mode UI** - Beautiful terminal panels with colors and icons
- **Free & Open Source** - No API keys required, uses ip-api.com
- **Cross-platform** - Runs on macOS, Linux, Windows

## Installation

### Download Pre-built Binaries

| Platform | Download |
|----------|----------|
| macOS (Apple Silicon) | [ipgeo](https://github.com/588h6fnmnv-hub/ipgeo/releases/latest/download/ipgeo) |
| Linux (x64) | [ipgeo](https://github.com/588h6fnmnv-hub/ipgeo/releases/latest/download/ipgeo-linux) |
| Windows (x64) | [ipgeo.exe](https://github.com/588h6fnmnv-hub/ipgeo/releases/latest/download/ipgeo-windows.exe) |

Or build from source:

```bash
git clone https://github.com/588h6fnmnv-hub/ipgeo
cd ipgeo
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

## Usage

```bash
# Look up a specific IP
./ipgeo 8.8.8.8
./ipgeo 2001:4860:4860::8888

# Detect and look up your own public IP
./ipgeo --me

# Interactive mode (repeated lookups)
./ipgeo

# Help
./ipgeo --help
```

## Example Output

```
$ ./ipgeo 8.8.8.8
┌────────────────────────────────────────────────────────────────────┐
│                       IP GEOLOCATION RESULT                        │
├────────────────────────────────────────────────────────────────────┤
│ ●  IP Address  8.8.8.8                                             │
│ ●  Status      ✓ success                                           │
│                                                                    │
│ ▸ LOCATION                                                         │
│ ──────────                                                         │
│ ●  Country     United States (US)                                  │
│ ●  Region      Virginia (VA)                                       │
│ ●  City        Ashburn                                             │
│ ●  Coordinates 39.0300° N, -77.5000° W                            │
│ ●  Timezone    America/New_York                                    │
│                                                                    │
│ ▸ NETWORK                                                          │
│ ─────────                                                          │
│ ●  ISP           Google LLC                                        │
│ ●  Organization  Google Public DNS                                 │
│ ●  ASN          AS15169 Google LLC                                 │
│                                                                    │
│ ▸ SECURITY                                                         │
│ ─────────                                                          │
│ ●  Proxy/VPN  ✓ No                                                 │
│ ●  Hosting    ✓ No                                                 │
│                                                                    │
│ 📍 Map: 🌐 https://maps.google.com/?q=39.0300,-77.5000             │
└────────────────────────────────────────────────────────────────────┘
```

## Interactive Mode

Run `ipgeo` without arguments to enter interactive mode:

```
ipgeo › 8.8.8.8
✓ Lookup complete: 8.8.8.8

┌────────────────────────────────────────────────────────────────────┐
│                       IP GEOLOCATION RESULT                        │
├────────────────────────────────────────────────────────────────────┤
│ ●  IP Address  8.8.8.8                                             │
│ ●  Status      ✓ success                                           │
│                                                                    │
│ ▸ LOCATION                                                         │
│ ──────────                                                         │
│ ●  Country     United States (US)                                  │
│ ●  Region      Virginia (VA)                                       │
│ ●  City        Ashburn                                             │
│ ●  Coordinates 39.0300° N, -77.5000° W                            │
│ ●  Timezone    America/New_York                                    │
│                                                                    │
│ ▸ NETWORK                                                          │
│ ─────────                                                          │
│ ●  ISP           Google LLC                                        │
│ ●  Organization  Google Public DNS                                 │
│ ●  ASN          AS15169 Google LLC                                 │
│                                                                    │
│ ▸ SECURITY                                                         │
│ ─────────                                                          │
│ ●  Proxy/VPN  ✓ No                                                 │
│ ●  Hosting    ✓ No                                                 │
│                                                                    │
│ 📍 Map: 🌐 https://maps.google.com/?q=39.0300,-77.5000             │
└────────────────────────────────────────────────────────────────────┘


ipgeo › :help

COMMANDS:
  :help, :h       Show this help
  :history        Show query history
  :clear, :cls    Clear screen
  :me             Detect and show your public IP
  :geo <IP>       Look up an IP address
  :stats          Show session statistics
  :clear          Clear screen
  :quit, :q, :exit Exit program

SHORTCUTS:
  ↑/↓             Navigate history
  Ctrl+C          Exit

EXAMPLES:
  8.8.8.8
  2001:4860:4860::8888
  :me
  :geo 1.1.1.1
```

## Data Source

Uses [ip-api.com](http://ip-api.com) - free tier (45 requests/minute, no API key required).

Returns:
- Country, Region, City
- Latitude/Longitude coordinates
- Timezone
- ISP, Organization, ASN
- Proxy/Hosting detection flags

## License

MIT License - Free for any use.