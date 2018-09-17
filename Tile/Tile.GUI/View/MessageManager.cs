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

        public void InvalidLogFile(Exception ex) {
            string error = string.Join(Environment.NewLine, CollectExceptionMessages(ex));
            Logger?.Error("Unable to write to the log file: " + error);
            MessageBox.Show(Properties.Resources.MessageInvalidLogFile + Environment.NewLine + error,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void NoApplicationFound() {
            Logger?.Warning("No application to process, check the shortcuts locations");
            MessageBox.Show(Properties.Resources.MessageLookupNoApplication,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ApplicationsFound(int appsCount) {
            Logger?.Success($"Found {appsCount} applications to process");
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

        public void TilesReset(int processedCount, int total, List<string> processedApps) {
            string appsStr = string.Join(", ", processedApps);
            Logger?.Success($"Processed {processedCount}/{total} application(s) ({appsStr})");
            MessageBox.Show(string.Format(Properties.Resources.MessageResetTiles, processedCount, total, appsStr),
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void NoTilesReset() {
            Logger?.Info("No tiles were reset");
            MessageBox.Show(Properties.Resources.MessageNoTilesToReset,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void LoadedTilesConfigFile() {
            Logger?.Info("Successfully loaded the tiles configuration file.");
            MessageBox.Show(Properties.Resources.MessageLoadedTilesConfigFile,
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool OverwriteTilesConfigFile() {
            return MessageBox.Show(Properties.Resources.MessageOverwriteTilesConfigFile,
                string.Empty, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ExposedTilesConfigFile(string path) {
            Logger?.Info("Successfully exposed the tiles configuration file.");
            string msg = string.Format(Properties.Resources.MessageExposedTilesConfigFile, path);
            MessageBox.Show(msg, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void FailedExposingTilesConfigFile(Exception ex) {
            string error = string.Join(Environment.NewLine, CollectExceptionMessages(ex));
            Logger?.Warning("Failed exposing the tiles configuration file: " + error);
            MessageBox.Show(Properties.Resources.MessageFailedExposingTilesConfigFile + Environment.NewLine + error,
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
