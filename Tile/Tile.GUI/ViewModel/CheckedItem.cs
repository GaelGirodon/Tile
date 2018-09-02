namespace Tile.GUI.ViewModel
{
    /// <summary>
    /// Checked item to be used in a ListBox data-binding.
    /// </summary>
    public class CheckedItem
    {
        /// <summary>
        /// Is the item checked?
        /// </summary>
        public bool IsChecked { get; set; } = true;

        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; }
    }
}
