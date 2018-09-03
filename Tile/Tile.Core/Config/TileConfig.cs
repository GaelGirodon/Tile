using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Tile.Core.Properties;

namespace Tile.Core.Config
{
    /// <summary>
    /// Application tile generation configuration
    /// </summary>
    public class TileConfig
    {
        #region Constants

        /// <summary>
        /// Path to embedded icons
        /// </summary>
        public static string EMBEDDED_ICONS_PATH = "Tile.Core.Resources.Icons";

        #endregion

        #region Fields

        /// <summary>
        /// Regex to find the application shortcut
        /// </summary>
        public string ShortcutRegex { get; set; }

        /// <summary>
        /// Path to the icon
        /// </summary>
        public string IconPath {
            get => _iconPath;
            set {
                if (value != null
                    && (value.StartsWith(EMBEDDED_ICONS_PATH)
                    && Assembly.GetExecutingAssembly().GetManifestResourceNames().Contains(value)
                    || File.Exists(value)))
                    _iconPath = value;
                else
                    throw new FileNotFoundException($"The icon '{value}' can't be found.");
            }
        }
        private string _iconPath;

        /// <summary>
        /// Tile background color
        /// </summary>
        public string BackgroundColor {
            get => ColorTranslator.ToHtml(_backgroundColor);
            set => _backgroundColor = ColorTranslator.FromHtml(value);
        }
        public Color BackgroundColorAsObj => _backgroundColor;
        private Color _backgroundColor = ColorTranslator.FromHtml("#333");

        /// <summary>
        /// Foreground text color (only light or dark)
        /// </summary>
        public string ForegroundColor {
            get => _foregroundColor.ToString();
            set => _foregroundColor = (TileForegroundColor)Enum.Parse(
                typeof(TileForegroundColor), value);
        }
        public TileForegroundColor ForegroundColorAsEnum => _foregroundColor;
        private TileForegroundColor _foregroundColor = TileForegroundColor.Light;

        /// <summary>
        /// Indicates if the application name must be shown on the medium tile
        /// </summary>
        public bool ShowNameOnMediumTile { get; set; } = true;

        /// <summary>
        /// Indicates the generation mode (how to generate the tiles)
        /// </summary>
        public string GenerationMode {
            get => _generationMode.ToString();
            set => _generationMode = (TileGenerationMode)Enum.Parse(
                typeof(TileGenerationMode), value);
        }
        public TileGenerationMode GenerationModeAsEnum => _generationMode;
        private TileGenerationMode _generationMode = TileGenerationMode.Adjusted;

        /// <summary>
        /// Icon scale for tile generation
        /// </summary>
        public TileIconScale IconScale { get; set; } = TileIconScale.Default;

        #endregion

        #region State

        /// <summary>
        /// Indicate wether the tile is ready to be generated or not
        /// </summary>
        /// <returns>true if the tile is ready to be generated</returns>
        public bool IsReady() {
            return _backgroundColor != null && _iconPath != null;
        }

        #endregion

        #region Loading

        /// <summary>
        /// Load and deserialize tiles configuration from a file
        /// or from local resource if the file doesn't exist.
        /// </summary>
        /// <param name="path">Path to the tiles configuration file</param>
        /// <returns>The tiles configuration</returns>
        public static Dictionary<string, TileConfig> Load(string path = null) {
            string content;
            if (path == null)
                content = Resources.TilesConfiguration;
            else if (File.Exists(path))
                content = File.ReadAllText(path);
            else
                throw new FileNotFoundException("The tiles configuration can't be found.");
            // else
            return JsonConvert.DeserializeObject<Dictionary<string, TileConfig>>(content);
        }

        #endregion
    }
}
