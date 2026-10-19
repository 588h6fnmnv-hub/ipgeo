# ipgeo - Terminal IP Geolocator

A simple, free, open-source terminal tool to look up IP address geolocation and network information.

## Features

- **IP Lookup** - Enter any IPv4 or IPv6 address
- **Auto-detect** - `--me` flag detects your public IP automatically
- **Interactive Mode** - Run without arguments for repeated lookups
- **Free & Open Source** - No API keys required, uses ip-api.com
- **Cross-platform** - Runs on macOS, Linux, Windows

## Installation

### Download Pre-built Binaries

| Platform | Download |
|----------|----------|
| macOS (Apple Silicon) | [ipgeo](ipgeo/publish/ipgeo) |
| Linux (x64) | [ipgeo](ipgeo/publish-linux/ipgeo) |
| Windows (x64) | [ipgeo.exe](ipgeo/publish-win/ipgeo.exe) |

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
────────────────────────────────────────
IP:          8.8.8.8
Status:      success
Country:     United States (US)
Region:      Virginia (VA)
City:        Ashburn
Coordinates: 39.0300, -77.5000
Timezone:    America/New_York
ISP:         Google LLC
Org:         Google Public DNS
AS:          AS15169 Google LLC
Proxy:       Yes
Hosting:     Yes
────────────────────────────────────────
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