using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Tile.Core.Config;
using Tile.Core.Engine;
using Tile.Core.Util;
using Tile.GUI.ViewModel;

namespace Tile.GUI.View
{
    /// <summary>
    /// Main application window
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// Tile generation settings
        /// </summary>
        private Settings _settings;

        /// <summary>
        /// The logger
        /// </summary>
        private Logger _logger;

        /// <summary>
        /// The tile generation engine
        /// </summary>
        private TileGenerator _generator;

        /// <summary>
        /// Tiles configuration
        /// </summary>
        private Dictionary<string, TileConfig> _tilesConfig;

        /// <summary>
        /// Applications shortcuts
        /// </summary>
        private Dictionary<string, AppShortcut> _apps;

        /// <summary>
        /// View model (data context)
        /// </summary>
        private MainWindowViewModel _viewModel;

        /// <summary>
        /// The message manager (used to separate logger and MessageBox calls)
        /// </summary>
        private MessageManager _msg;

        #endregion

        /// <summary>
        /// Initialize the window.
        /// </summary>
        public MainWindow() {
            // Message manager
            _msg = new MessageManager();

            Init();
            InitializeComponent();

            // Initialize the view model
            _viewModel = new MainWindowViewModel(_settings);
            DataContext = _viewModel;

            // Lookup applications
            LookupApplications();
        }

        #region Process steps

        /// <summary>
        /// Initialize the generation context: settings, logger,
        /// tiles configuration and tile generator.
        /// </summary>
        private void Init() {
            // Load settings
            try {
                _settings = Settings.LoadOrDefault(Settings.SETTINGS_PATH);
                _logger = new Logger(_settings.LogFilePath);
            } catch (Exception ex) {
                _msg.InvalidSettings(ex);
                Environment.Exit(1);
                return;
            }
            // Initialize the logger
            _msg.Logger = _logger;
            _logger.Init();
            _logger.Success("Loaded settings");
            // Load tiles configuration
            try {
                _tilesConfig = TileConfig.Load(_settings.TilesConfigPath);
            } catch (Exception ex) {
                _msg.InvalidTilesConfig(ex);
                Environment.Exit(2);
            }
            _logger.Success("Loaded tiles configuration");
            // Initialize the tile generator
            _generator = new TileGenerator(_settings, _logger);
            _logger.Success("Initialized tiles generator");
        }

        /// <summary>
        /// Lookup applications shortcuts based on settings.
        /// </summary>
        private void LookupApplications() {
            // Lookup applications (shortcuts & targets)
            var apps = _generator.LookupApps(_tilesConfig);
            if (apps.Count == 0) { // No application found
                _msg.NoApplicationFound();
                _apps = null;
                _viewModel.SelectedApps = null;
                _viewModel.IsReady = false;
            } else { // Ready to select applications and generate tiles
                _msg.ApplicationsFound(apps.Count);
                _apps = apps;
                _viewModel.SelectedApps = new ObservableCollection<CheckedItem>(
                    _apps.Select(a => new CheckedItem { Name = a.Key, IsChecked = true }));
                _viewModel.IsReady = true;
            }
        }

        /// <summary>
        /// Run the tile generation.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void GenerateTiles(object sender, RoutedEventArgs e) {
            var selectedApps = _viewModel.SelectedApps.Where(a => a.IsChecked).Select(a => a.Name);
            if (!selectedApps.Any()) {
                _msg.NoApplicationSelected();
                return;
            } // else: ready to generate tiles
            var apps = _apps.Keep(selectedApps);
            // Generate tiles
            var processedApps = _generator.GenerateTiles(_tilesConfig, apps);
            if (processedApps.Count > 0)
                _msg.TilesGenerated(processedApps.Count, apps.Count, processedApps);
            else
                _msg.NoTilesGenerated();
        }

        /// <summary>
        /// Reset tiles to their initial state.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void ResetTiles(object sender, RoutedEventArgs e) {
            var selectedApps = _viewModel.SelectedApps.Where(a => a.IsChecked).Select(a => a.Name);
            if (!selectedApps.Any()) {
                _msg.NoApplicationSelected();
                return;
            } // else: ready to reset tiles
            var apps = _apps.Keep(selectedApps);
            // Reset tiles
            var processedApps = _generator.ResetTiles(apps);
            if (processedApps.Count > 0)
                _msg.TilesReset(processedApps.Count, apps.Count, processedApps);
            else
                _msg.NoTilesReset();
        }

        #endregion

        #region Tile customization

        /// <summary>
        /// Copy the embedded tiles configuration file next to the application executable
        /// to allow the user to customize tile generation.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void ExposeTilesConfigurationFile(object sender, RoutedEventArgs e) {
            string path = _settings.TilesConfigPath;
            if (File.Exists(path) && !_msg.OverwriteTilesConfigFile()) {
                return;
            } // else
            try {
                TileConfig.ExportEmbeddedConfiguration(path);
                _msg.ExposedTilesConfigFile(path);
            } catch (Exception ex) {
                _msg.FailedExposingTilesConfigFile(ex);
            }
        }

        /// <summary>
        /// Reload the tiles configuration file.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void ReloadTilesConfigurationFile(object sender, RoutedEventArgs e) {
            try {
                _tilesConfig = TileConfig.Load(_settings.TilesConfigPath);
                _apps = null;
                _viewModel.SelectedApps = null;
                _viewModel.IsReady = false;
                _msg.LoadedTilesConfigFile();
                LookupApplications();
            } catch (Exception ex) {
                _msg.InvalidTilesConfig(ex);
            }
        }

        #endregion

    }
}