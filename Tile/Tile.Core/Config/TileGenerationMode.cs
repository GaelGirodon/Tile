namespace Tile.Core.Config
{
    /// <summary>
    /// Tile generation mode.
    /// Indicate how the tile will be generated.
    /// </summary>
    public enum TileGenerationMode
    {
        /// <summary>
        /// Default centered mode, an icon and a background color
        /// are used to generate both medium and small tiles.
        /// </summary>
        Centered,

        /// <summary>
        /// Adjusted mode, an icon and a background color
        /// are used to generate both medium and small tiles.
        /// The icon is slighty shifted up on the medium tile.
        /// </summary>
        Adjusted,

        /// <summary>
        /// Custom mode, an image is used to generate
        /// both medium and small tiles.
        /// The image will fill the entire tile.
        /// </summary>
        Custom
    }
}