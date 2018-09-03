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
        public Logger Logger { get; set; }

        #endregion

        #region Messages

        public void InvalidSettings(Exception ex) {
            string error = string.Join(Environment.NewLine, CollectExceptionMessages(ex));
            Logger?.Error("Invalid application settings: " + error);
            MessageBox.Show(Properties.Resources.MessageInvalidSettings + Environment.NewLine + error,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void InvalidTilesConfig(Exception ex) {
            string error = string.Join(Environment.NewLine, CollectExceptionMessages(ex));
            Logger?.Error("Invalid tiles configuration: " + error);
            MessageBox.Show(Properties.Resources.MessageInvalidTilesConfig + Environment.NewLine + error,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void NoApplicationFound() {
            Logger?.Warning("No application to process, check the shortcuts locations");
            MessageBox.Show(Properties.Resources.MessageLookupNoApplication,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ApplicationsFound(int appsCount) {
            Logger?.Success($"Found {appsCount} applications to process");
            MessageBox.Show(string.Format(Properties.Resources.MessageLookupFound, appsCount),
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void NoApplicationSelected() {
            MessageBox.Show(Properties.Resources.MessageNoApplicationSelected,
                    string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void TilesGenerated(int processedCount, int total, List<string> processedApps) {
            string appsStr = string.Join(", ", processedApps);
            Logger?.Success($"Processed {processedCount}/{total} application(s) ({appsStr})");
            MessageBox.Show(string.Format(Properties.Resources.MessageProcessedApplications, processedCount, total, appsStr),
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void NoTilesGenerated() {
            Logger?.Info("No tiles were generated");
            MessageBox.Show(Properties.Resources.MessageNoTiles,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        #endregion

        #region Util

        /// <summary>
        /// Explore the exception hierarchy and collect all messages.
        /// </summary>
        /// <param name="ex">The parent exception</param>
        /// <returns>All the exception messages</returns>
        public List<string> CollectExceptionMessages(Exception ex) {
            Exception current = ex;
            var messages = new List<string>();
            do {
                if (!string.IsNullOrWhiteSpace(current.Message))
                    messages.Add(current.Message);
                current = current.InnerException;
            } while (current != null);
            return messages;
        }

        #endregion
    }
}
