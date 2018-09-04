using System.ComponentModel;

namespace Tile.GUI.ViewModel
{
    /// <summary>
    /// Checked item to be used in a ListBox data-binding.
    /// </summary>
    public class CheckedItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Is the item checked?
        /// </summary>
        public bool IsChecked {
            get => _isChecked;
            set {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        private bool _isChecked = true;

        /// <summary>
        /// Item name
        /// </summary>
        public string Name {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _name;

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
