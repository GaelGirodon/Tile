using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Tile.Core.Config;
using Tile.Core.Properties;
using Tile.Core.Util;

namespace Tile.Core.Engine
{
    /// <summary>
    /// Core Tile generator
    /// </summary>
    public class TileGenerator
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private Logger _logger;

        /// <summary>
        /// Application settings
        /// </summary>
        private Settings _settings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the tile generator with the given logger and settings.
        /// </summary>
        /// <param name="settings">Generation settings</param>
        /// <param name="logger">The logger</param>
        public TileGenerator(Settings settings, Logger logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Tile generation

        /// <summary>
        /// Look for applications shortcuts matching tiles configuration.
        /// </summary>
        /// <param name="tilesConfig">Tiles configuration</param>
        /// <returns>Found applications</returns>
        public Dictionary<string, AppShortcut> LookupApps(Dictionary<string, TileConfig> tilesConfig)
        {
            // Find all shortcuts in the given locations
            var shortcuts = FileUtilities.FindFiles("*.lnk", _settings.ShortcutsLocations);
            var apps = new Dictionary<string, AppShortcut>();
            if (shortcuts.Count == 0)
            {
                _logger.Error("No shortcuts were found in the given locations");
                return apps;
            } // else
            _logger.Success($"{shortcuts.Count} shortcuts were found in the given locations");

            // Find shortcuts associated to tiles configuration
            foreach (var t in tilesConfig)
            {
                Regex r = new Regex(t.Value.ShortcutRegex, RegexOptions.IgnoreCase);
                // Look for shortcuts matching tile configuration
                var matches = shortcuts.Where(s => r.IsMatch(Path.GetFileNameWithoutExtension(s)));

                if (matches.Count() > 1)
                    _logger.Warning($"Found more than 1 shortcut for '{t.Key}', skipping this item...");
                else if (matches.Count() == 1)
                {
                    var shortcut = matches.First();
                    var target = FileUtilities.GetShortcutTargetFile(shortcut);
                    if (Path.GetExtension(target) == ".exe")
                    {
                        // The application can be processed
                        _logger.Success($"The '{t.Key}' shortcut and executable file found!");
                        apps.Add(t.Key, new AppShortcut { ShortcutPath = shortcut, ExecutablePath = target });
                    }
                    else
                        _logger.Warning($"The '{t.Key}' shortcut is not associated to a .exe file,"
                            + " skipping this item...");
                }
            }
            return apps;
        }

        /// <summary>
        /// Generate tiles for the given applications using the tiles configuration.
        /// </summary>
        /// <param name="tilesConfig">The tiles configuration</param>
        /// <param name="apps">The applications information</param>
        /// <returns>Processed applications names</returns>
        public List<string> GenerateTiles(Dictionary<string, TileConfig> tilesConfig, Dictionary<string, AppShortcut> apps)
        {
            var processedApps = new List<string>();
            if (apps.Count == 0)
            {
                _logger.Info("No tile can be generated for the requested applications");
                return processedApps;
            } // else
            _logger.Info($"Ready to process the following applications: {string.Join(", ", apps.Select(app => app.Key))}");
            
            foreach (var app in apps)
            {
                try
                {
                    Generate(tilesConfig[app.Key], app.Value, _settings.Overwrite);
                    processedApps.Add(app.Key);
                    _logger.Success($"'{app.Key} tile successfully generated!");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error processing '{app.Key}': {ex.Message}");
                }
            }
            GC.Collect();
            return processedApps;
        }

        /// <summary>
        /// Generate tiles and set it up
        /// </summary>
        /// <param name="tileConfig">Tile configuration</param>
        public void Generate(TileConfig tileConfig, AppShortcut app, bool overwrite = false)
        {
            Image mediumTile, smallTile;
            
            // Tile generation
            if (tileConfig.GenerationModeAsEnum == TileGenerationMode.Custom)
            {
                mediumTile = GenerateTile(TileDimensions.MEDIUM_TILE_SIZE, tileConfig.BackgroundColorAsObj,
                    tileConfig.IconPath, TileDimensions.MEDIUM_TILE_SIZE);
                smallTile = GenerateTile(TileDimensions.SMALL_TILE_SIZE, tileConfig.BackgroundColorAsObj,
                    tileConfig.IconPath, TileDimensions.SMALL_TILE_SIZE);
            }
            else
            {
                mediumTile = GenerateTile(TileDimensions.MEDIUM_TILE_SIZE, tileConfig.BackgroundColorAsObj,
                    tileConfig.IconPath, TileDimensions.MEDIUM_ICON_SIZE.Scale(tileConfig.IconScale.MediumTile),
                    tileConfig.GenerationModeAsEnum == TileGenerationMode.Adjusted ? -32 : 0);
                smallTile = GenerateTile(TileDimensions.SMALL_TILE_SIZE, tileConfig.BackgroundColorAsObj,
                    tileConfig.IconPath, TileDimensions.SMALL_ICON_SIZE.Scale(tileConfig.IconScale.SmallTile));
            }

            // Store the tiles in Assets folder
            string appPath = Path.GetDirectoryName(app.ExecutablePath);
            string assetsPath = Path.Combine(Path.GetDirectoryName(app.ExecutablePath),
                AssetsConstants.AssetsFolderName);
            if (!overwrite && Directory.Exists(assetsPath))
                throw new Exception("Assets directory already exists");
            Directory.CreateDirectory(assetsPath);

            // Generate the XML file (background color, logo paths, foreground color, and XML filename)
            string xml = Path.Combine(appPath, Path.GetFileNameWithoutExtension(app.ExecutablePath)
                + AssetsConstants.VisualElementsManifestXmlFileExtension);
            if (!overwrite && File.Exists(xml))
            {
                Directory.Delete(assetsPath);
                throw new Exception("Visual Elements Manifest XML file already exists");
            }
            File.WriteAllText(xml, GenerateXMLVisualElements(
                tileConfig.BackgroundColorAsObj, tileConfig.ForegroundColorAsEnum, tileConfig.ShowNameOnMediumTile));

            // Save tiles
            mediumTile.Save(Path.Combine(assetsPath, AssetsConstants.MediumTileFileName), ImageFormat.Png);
            smallTile.Save(Path.Combine(assetsPath, AssetsConstants.SmallTileFileName), ImageFormat.Png);

            // Set shortcut last write time to now (to update tile)
            File.SetLastWriteTime(app.ShortcutPath, DateTime.Now);
        }

        /// <summary>
        /// Generate a tile (an image) using given parameters
        /// </summary>
        /// <param name="tileSize">Image dimensions (in pixels)</param>
        /// <param name="backgroundColor">Background color of the tile</param>
        /// <param name="iconFile">Icon to draw on the tile</param>
        /// <param name="iconSize">Icon dimensions (in pixels)</param>
        /// <param name="yOffset">Icon Y offset
        /// (X centered and Y centered by default)</param>
        /// <returns>The generated tile</returns>
        public Image GenerateTile(Size tileSize, Color backgroundColor,
            string iconFile, Size iconSize, int yOffset = 0)
        {
            // Create a new image
            var tile = new Bitmap(tileSize.Width, tileSize.Height);
            var graphics = Graphics.FromImage(tile);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Set the background color to "backgroundColor"
            graphics.FillRectangle(new SolidBrush(backgroundColor),
                new Rectangle(Point.Empty, tileSize));

            // Load the icon
            Image icon = iconFile.StartsWith(TileConfig.EMBEDDED_ICONS_PATH)
                ? Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(iconFile))
                : Image.FromFile(iconFile);

            // Draw the icon on the image
            // - Full size if iconSize == null
            // - Or iconSize
            // - Offset it using yOffset value
            graphics.DrawImage(icon,
                tileSize.Width / 2 - iconSize.Width / 2,
                tileSize.Height / 2 - iconSize.Height / 2 + yOffset,
                iconSize.Width, iconSize.Height);

            // Return generated tile
            return tile;
        }

        /// <summary>
        /// Generate the VisualElementsManifest XML file
        /// </summary>
        /// <returns>XML file content</returns>
        public string GenerateXMLVisualElements(Color backgroundColor,
            TileForegroundColor foregroundColor, bool showNameOnMediumTile)
            => Resources.VisualElementsManifest
                .Replace("{BackgroundColor}", ColorTranslator.ToHtml(backgroundColor))
                .Replace("{ShowNameOnSquare150x150Logo}", showNameOnMediumTile ? "on" : "off")
                .Replace("{ForegroundText}", foregroundColor.ToString().ToLower())
                .Replace("{Square150x150Logo}", AssetsConstants.AssetsFolderName + '\\' + AssetsConstants.MediumTileFileName)
                .Replace("{Square70x70Logo}", AssetsConstants.AssetsFolderName + '\\' + AssetsConstants.SmallTileFileName);

        #endregion
    }
}
