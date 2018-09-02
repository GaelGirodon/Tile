using System;
using System.Collections.Generic;
using System.Windows;
using Tile.Core.Util;

namespace Tile.GUI.View
{
    /// <summary>
    /// Application message manager
    /// </summary>
    class MessageManager
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private Logger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the message manager
        /// </summary>
        /// <param name="logger">The logger</param>
        public MessageManager(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Messages

        public void NoApplicationFound()
        {
            _logger.Warning("No application to process, check the shortcuts locations");
            MessageBox.Show(Properties.Resources.MessageLookupNoApplication,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ApplicationsFound(int appsCount)
        {
            _logger.Success($"Found {appsCount} applications to process");
            MessageBox.Show(string.Format(Properties.Resources.MessageLookupFound, appsCount),
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void NoApplicationSelected()
        {
            MessageBox.Show(Properties.Resources.MessageNoApplicationSelected,
                    string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void TilesGenerated(int processedCount, int total, List<string> processedApps)
        {
            string appsStr = string.Join(", ", processedApps);
            _logger.Success($"Processed {processedCount}/{total} application(s) ({appsStr})");
            MessageBox.Show(string.Format(Properties.Resources.MessageProcessedApplications, processedCount, total, appsStr),
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void NoTilesGenerated()
        {
            _logger.Info("No tiles were generated");
            MessageBox.Show(Properties.Resources.MessageNoTiles,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        #endregion
    }
}
