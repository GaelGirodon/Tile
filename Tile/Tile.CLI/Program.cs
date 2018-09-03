using System;
using Tile.Core.Config;
using Tile.Core.Engine;
using Tile.Core.Util;

namespace Tile.CLI
{
    class Program
    {
        #region Main

        /// <summary>
        /// Run the program
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        [STAThread]
        static int Main(string[] args) {
            var program = new Program();
            try {
                return program.Run(args);
            } catch (Exception e) {
                Console.Error.WriteLine($"Failed generating tiles: {e.Message}");
                return 1;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private Logger _logger;

        #endregion

        #region Run

        /// <summary>
        /// Load configuration, initialize and run the program.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Exit code</returns>
        private int Run(string[] args = null) {
            var settings = Settings.LoadOrDefault(Settings.SETTINGS_PATH);
            _logger = new Logger(settings.LogFilePath);
            _logger.Init();
            _logger.Success("Loaded settings");
            var tilesConfig = TileConfig.Load(settings.TilesConfigPath);
            _logger.Success("Loaded tiles configuration");
            var generator = new TileGenerator(settings, _logger);
            _logger.Success("Initialized tiles generator");

            var apps = generator.LookupApps(tilesConfig);
            if (apps.Count == 0) {
                _logger.Warning("No applications to process, check the shortcuts locations");
                return 1;
            } else {
                _logger.Success($"Found {apps.Count} applications to process");
                if (args.Length > 0)
                    apps = apps.Keep(args); // Only process given apps
                var processedApps = generator.GenerateTiles(tilesConfig, apps);
                if (processedApps.Count > 0)
                    _logger.Success($"Processed {processedApps.Count}/{apps.Count} application(s) ({string.Join(", ", processedApps)})");
                else
                    _logger.Info("No tiles were generated");
            }

            return 0;
        }

        #endregion
    }
}
