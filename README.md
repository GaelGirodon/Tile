# Tile

A simple Windows utility to generate and set up the start menu tiles.

**Tile** automatically looks for applications shortcuts, generate tiles
and set them up in the start menu.

## Quickstart

- Download the [latest release](https://github.com/GaelGirodon/Tile/releases)
- Extract the archive
- Launch `Tile.exe` and follow the instructions

## Features

### Main features

- Look for applications shortcuts in the start menu
- Locate target executable files
- Dynamically generate tiles and set them up

### Supported applications

The following applications are supported without any additional setup:

- 7-Zip
- Adobe Reader
- FileZilla
- Inkscape
- NetBeans
- Notepad++
- TeamViewer
- VirtualBox
- VLC Media Player

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
| `BackgroundColor`      | Tile background color                                              |
| `ForegroundText`       | Foreground text color (only `Light` or `Dark`)                     |
| `ShowNameOnMediumTile` | Show the application name on the medium tile or not                |
| `GenerationMode`       | The [generation mode](Tile/Tile.Core/Config/TileGenerationMode.cs) |

#### Shorcuts locations

To customize directories where application shortcuts can be found,
create a `settings.json` file next to `Tile.exe` with the following content:

```json
{
  "ShortcutsLocations": [
    "C:/Absolute/Path/To/Directory/Containing/Shortcuts"
  ]
}
```

## Tasks

- **Features**
  - [ ] Add new tiles presets
  - [ ] Implement the "Clean tiles" feature
  - [ ] Prevent replacing "original" tiles
  - [ ] Improve error handling
- **Release, packaging & documentation**  
  - [ ] Improve release process (bump version number...)
  - [ ] Merge all `.dll` into a single `.exe` file
  - [ ] Improve [README.md](README.md)
  - [ ] Add a contribution guide
  - [ ] Improve the application icon

## License

**Tile** is licensed under the GNU General Public License.
