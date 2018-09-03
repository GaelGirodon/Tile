namespace Tile.Core.Config
{
    /// <summary>
    /// Custom scale for icon in tile generation
    /// </summary>
    public struct TileIconScale
    {
        /// <summary>
        /// Default scale (1.0f ; 1.0f)
        /// </summary>
        public static TileIconScale Default => new TileIconScale() {
            MediumTile = 1.0f,
            SmallTile = 1.0f
        };

        /// <summary>
        /// Custom icon scale on the medium tile
        /// </summary>
        public float MediumTile { get; set; }

        /// <summary>
        /// Custom icon scale on the small tile
        /// </summary>
        public float SmallTile { get; set; }

        /// <summary>
        /// Initialize scales
        /// </summary>
        /// <param name="_mediumTile">Custom icon scale on the medium tile</param>
        /// <param name="_smallTile">Custom icon scale on the small tile</param>
        public TileIconScale(float _mediumTile, float _smallTile) {
            MediumTile = _mediumTile;
            SmallTile = _smallTile;
        }
    }
}
