using System.Drawing;

namespace Tile.Core.Util
{
    /// <summary>
    /// Size extension methods
    /// </summary>
    static class SizeExtensions
    {
        /// <summary>
        /// Scale a size
        /// </summary>
        /// <param name="size">The original size</param>
        /// <param name="scale">Scale amount</param>
        /// <returns>A new size, scaled</returns>
        public static Size Scale(this Size size, float scale) {
            return scale == 1.0f ? size : new SizeF(size.Width * scale, size.Height * scale).ToSize();
        }
    }
}
