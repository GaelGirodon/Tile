using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tile.Core.Config;
using Tile.GUI.Properties;

namespace Tile.GUI.ViewModel
{
    /// <summary>
    /// Main window view model (MVVM)
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Data Binding

        /// <summary>
        /// Window title
        /// </summary>
        public string Title => $"{Resources.AppName} | {Resources.AppDescription}";

        /// <summary>
        /// Overwrite existing tiles.
        /// This setting is exposed from settings for data-binding.
        /// </summary>
        public bool Overwrite {
            get => _settings.Overwrite;
            set => _settings.Overwrite = value;
        }

        /// <summary>
        /// Applications to process.
        /// This setting is exposed from settings for data-binding.
        /// </summary>
        public ObservableCollection<CheckedItem> SelectedApplications {
            get => _selectedApplications;
            set {
                _selectedApplications = value;
                OnPropertyChanged("SelectedApplications");
            }
        }
        public ObservableCollection<CheckedItem> _selectedApplications;

        /// <summary>
        /// Indicate whether the tile generation is ready or not.
        /// </summary>
        public bool IsReady {
            get => _isReady;
            set {
                _isReady = value;
                OnPropertyChanged("IsReady");
            }
        }
        private bool _isReady;

        #endregion

        #region Constructor

        /// <summary>
        /// Tile generation settings
        /// </summary>
        private Settings _settings;

        public MainWindowViewModel(Settings settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _isReady = false;
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged event trigger
        /// </summary>
        /// <param name="PropertyName">Changed property name</param>
        protected void OnPropertyChanged(string PropertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
