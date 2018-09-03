using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Tile.Core.Config
{
    /// <summary>
    /// Tile generation settings
    /// </summary>
    public class Settings
    {
        #region Constants

        /// <summary>
        /// Path to the assembly directory
        /// </summary>
        public static string APP_LOCATION_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Path to the custom settings file (overrides default values)
        /// </summary>
        public static string SETTINGS_PATH = Path.Combine(APP_LOCATION_PATH, "settings.json");

        /// <summary>
        /// Default path to the the log file
        /// </summary>
        public static string DEFAULT_LOG_PATH = Path.Combine(APP_LOCATION_PATH, "log.txt");

        #endregion

        #region Fields

        /// <summary>
        /// Path to the log file
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Directories where application shortcuts can be found
        /// </summary>
        public List<string> ShortcutsLocations {
            get => _shortcutsLocations;
            set {
                if (value == null || value.Count == 0 || !value.TrueForAll(l => Directory.Exists(l)))
                    throw new ArgumentException($"Shortcuts locations are not valid.");
                _shortcutsLocations = value;
            }
        }
        private List<string> _shortcutsLocations;

        /// <summary>
        /// Path to the tiles configuration file
        /// </summary>
        public string TilesConfigPath {
            get => _tilesConfigPath;
            set {
                if (value != null && !File.Exists(value) && !File.Exists(Path.Combine(APP_LOCATION_PATH, value)))
                    throw new ArgumentException($"The tiles configuration file path '{value}' can't be found.");
                _tilesConfigPath = value;
            }
        }
        private string _tilesConfigPath;

        /// <summary>
        /// Tiles and icons dimensions
        /// </summary>
        public TileSetSizes Sizes {
            get => _sizes;
            set {
                if (value == null || value.Medium == null || value.Small == null)
                    throw new ArgumentNullException("All tile sizes must be provided.");
                _sizes = value;
            }
        }
        private TileSetSizes _sizes;

        /// <summary>
        /// Indicate whether existing tile files
        /// must be overwritten or not
        /// </summary>
        public bool Overwrite { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the settings with default values
        /// </summary>
        public Settings() {
            LogFilePath = DEFAULT_LOG_PATH;
            ShortcutsLocations = new List<string> {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs")
            };
            Sizes = new TileSetSizes() { Medium = new TileSizes(270, 135), Small = new TileSizes(126, 78) };
            TilesConfigPath = null; // Load embedded configuration file by default
            Overwrite = false;
        }

        #endregion

        #region Loading

        /// <summary>
        /// Load and deserialize settings. Fail if the settings file doesn't exist.
        /// </summary>
        /// <param name="path">Path to the settings file</param>
        /// <returns>The settings</returns>
        public static Settings Load(string path) {
            if (!File.Exists(path))
                throw new FileNotFoundException("The settings can't be found");
            // else
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path),
                new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
        }

        /// <summary>
        /// Load settings from a file or return default settings
        /// if the file doesn't exist.
        /// </summary>
        /// <param name="path">Path to the settings file</param>
        /// <returns>The settings</returns>
        public static Settings LoadOrDefault(string path) {
            return File.Exists(path) ? Load(path) : new Settings();
        }

        #endregion
    }
}
