using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tile.Core.Engine
{
    /// <summary>
    /// An application shortcut with an executable target path.
    /// </summary>
    public class AppShortcut
    {
        /// <summary>
        /// Path to the application shortcut
        /// </summary>
        public string ShortcutPath { get; set; }

        /// <summary>
        /// Path to the target executable file
        /// </summary>
        public string ExecutablePath { get; set; }
    }
}
