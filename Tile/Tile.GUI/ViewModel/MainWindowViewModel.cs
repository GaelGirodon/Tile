using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Tile.Core.Config;

namespace Tile.GUI.ViewModel
{
    /// <summary>
    /// Main window view model (MVVM)
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Data Binding

        /// <summary>
        /// Overwrite existing tiles.
        /// This setting is exposed from settings for data-binding.
        /// </summary>
        public bool Overwrite {
            get => _settings.Overwrite;
            set => _settings.Overwrite = value;
        }

        /// <summary>
        /// Select or unselect all applications at once.
        /// </summary>
        public bool SelectAllApps {
            get => _selectAllApps;
            set {
                _selectAllApps = value;
                OnPropertyChanged("SelectAllApps");
                _selectedApps?.ToList().ForEach(a => a.IsChecked = value);
                OnPropertyChanged("SelectedApps");
            }
        }
        private bool _selectAllApps = true;

        /// <summary>
        /// Applications to process.
        /// This property is exposed from settings for data-binding.
        /// </summary>
        public ObservableCollection<CheckedItem> SelectedApps {
            get => _selectedApps;
            set {
                _selectedApps = value;
                OnPropertyChanged("SelectedApps");
                UpdateSelectAllAppsProperty();
                _selectedApps?.ToList().ForEach(app
                    => app.PropertyChanged += (object sender, PropertyChangedEventArgs e)
                    => UpdateSelectAllAppsProperty());
            }
        }
        private ObservableCollection<CheckedItem> _selectedApps;

        /// <summary>
        /// Update the SelectAllApps property based on SelectedApps property value.
        /// If all the apps are checked/unchecked, the value of the SelectAllApps property
        /// must be updated to true/false.
        /// </summary>
        private void UpdateSelectAllAppsProperty() {
            if (_selectedApps == null || _selectedApps.Count == 0) {
                _selectAllApps = false;
                OnPropertyChanged("SelectAllApps");
                return;
            }
            // else
            var isCheckedCount = _selectedApps.Count(a => a.IsChecked);
            if (isCheckedCount == _selectedApps.Count || isCheckedCount == 0) {
                _selectAllApps = _selectedApps.FirstOrDefault()?.IsChecked ?? false;
                OnPropertyChanged("SelectAllApps");
            }
        }

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
