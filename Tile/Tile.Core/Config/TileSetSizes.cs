using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Tile.Core.Config
{
    /// <summary>
    /// Dimensions for medium and small tiles and icons
    /// </summary>
    public class TileSetSizes
    {
        /// <summary>
        /// Medium tile and icon dimensions
        /// </summary>
        public TileSizes Medium { get; set; }

        /// <summary>
        /// Small tile and icon dimensions
        /// </summary>
        public TileSizes Small { get; set; }
    }

    /// <summary>
    /// Tile and icon dimensions
    /// </summary>
    public class TileSizes
    {
        #region Fields

        /// <summary>
        /// Tile dimensions (width & height) as an array
        /// </summary>
        public int[] Tile {
            get => new int[] { TileSize.Width, TileSize.Height };
            set => TileSize = ParseSize(value);
        }

        /// <summary>
        /// Tile dimensions (width & height)
        /// </summary>
        [JsonIgnore]
        public Size TileSize { get; set; } = new Size(270, 270);

        /// <summary>
        /// Tile icon dimensions (width & height) as an array
        /// </summary>
        public int[] Icon {
            get => new int[] { IconSize.Width, IconSize.Height };
            set => IconSize = ParseSize(value);
        }

        /// <summary>
        /// Tile icon dimensions (width & height)
        /// </summary>
        [JsonIgnore]
        public Size IconSize { get; set; } = new Size(135, 135);

        #endregion

        #region Parsing

        /// <summary>
        /// Validate, parse a size array and map it to a Size object
        /// </summary>
        /// <param name="value">The value to validate and parse</param>
        /// <returns>The size object</returns>
        public static Size ParseSize(int[] value)
        {
            if (value == null || value.Length != 2 || value[0] <= 0 || value[1] <= 0)
                throw new ArgumentException($"The size {value} is invalid");
            return new Size(value[0], value[1]);
        }

        #endregion
    }

}
