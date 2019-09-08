# Tile

A simple Windows 10 utility to customize the start menu tiles.

![Demonstration of the application](./Tile/resources/dist/tile-demo.gif)

## QuickStart

- Download the [latest release](https://github.com/GaelGirodon/Tile/releases)
- Extract the archive
- Launch `Tile.exe` and follow the instructions

## About

**Tile** automatically looks for applications in the start menu
and generates custom tile assets for these applications.
Then, you only need to pin the programs you want on the start menu
and their tile will have a custom look.

*Tile* doesn't modify your start menu: no tile will be added and the layout
will be preserved. Custom assets are generated for supported applications
using Windows built-in Tile customization mechanism
(*Tile* is just an automation tool to make things easier).
When you will pin these applications on the start menu, they will
automatically have a custom tile.

![Examples of generated tiles](./Tile/resources/dist/mosaic-tiles.png)

## Features

### Main features

- Look for applications shortcuts in the start menu
- Locate target executable files
- Dynamically generate tiles for these applications

### Supported applications

Many applications are supported without any additional setup.

> Open [TilesConfiguration.json](Tile/Tile.Core/Resources/TilesConfiguration.json)
> file to get all supported applications

### Customization

#### About customization

A tile is generated using an **icon** and a **configuration**.
A [configuration file](Tile/Tile.Core/Resources/TilesConfiguration.json)
is provided in the application by default.

This configuration can be customized by creating a `tiles.json` file
next to `Tile.exe` that will override the default one.

#### Sample configuration

```json
{
  "VLC Media Player": {
    "ShortcutRegex": "^VLC media player$",
    "IconPath": "Icons/VLC.png",
    "BackgroundColor": "#FF7900",
    "ForegroundText": "Light",
    "ShowNameOnMediumTile": true,
    "GenerationMode": "Adjusted"
  }
}
```

#### Configuration specification

| Field                  | Description                                                        |
| ---------------------- | ------------------------------------------------------------------ |
| `ShortcutRegex`        | A regex to find the application shortcut                           |
| `IconPath`             | Path to the icon (can be relative to `Tile.exe` directory)         |
| `BackgroundColor`      | Tile background color (in hexadecimal format: `#FFFFFF`)           |
| `ForegroundText`       | Foreground text color (only `Light` or `Dark`)                     |
| `ShowNameOnMediumTile` | Show the application name on the medium tile or not                |
| `GenerationMode`       | The [generation mode](Tile/Tile.Core/Config/TileGenerationMode.cs) |

### Settings

Create a [`settings.json`](Tile/Tile.Core/Config/Settings.cs) file
next to `Tile.exe` to customize **Tile**.

#### Settings specification

| Field                | Description                          |
| -------------------- | ------------------------------------ |
| `LogFilePath`        | Path to the log file                 |
| `ShortcutsLocations` | Additional shortcuts locations       |
| `TilesConfigPath`    | Path to the tiles configuration file |
| `Sizes`              | Tiles and icons size                 |
| `Overwrite`          | Overwrite existing tiles             |

#### Sample settings file

```json
{
  "LogFilePath": "log.txt",
  "ShortcutsLocations": [
    "C:\\Absolute\\Path\\To\\Directory\\Containing\\Shortcuts"
  ],
  "TilesConfigPath": "tiles.json",
  "Sizes": {
    "Medium": {
      "Tile": [512, 512],
      "Icon": [240, 240]
    },
    "Small": {
      "Tile": [256, 256],
      "Icon": [160, 160]
    }
  },
  "Overwrite": false
}
```

## Tasks

- **Features**
  - [ ] Add new tiles presets
  - [ ] Improve error handling
  - [ ] Improve tiles configuration reload (live reload?)
  - [ ] Make tiles easier to customize
- **Release, packaging & documentation**  
  - [ ] Improve the release process (bump version number...)
  - [ ] Merge all `.dll` into a single `.exe` file
  - [ ] Improve [README.md](README.md)
    - [ ] Add a _FAQ/troubleshoot_ section
    - [ ] Add a _How it works_ section
    - [ ] Add a contribution guide

## License

**Tile** is licensed under the GNU General Public License.
