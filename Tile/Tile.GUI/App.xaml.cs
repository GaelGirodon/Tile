using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Tile.GUI.View;

namespace Tile.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Global exception handler.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            string error = string.Join(Environment.NewLine, MessageManager.CollectExceptionMessages(e.Exception));
            MessageBox.Show($"An unexpected error has occurred, the application is going to close now." + Environment.NewLine
                + $"{error}", "Unhandled error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Environment.Exit(1);
        }
    }
}
