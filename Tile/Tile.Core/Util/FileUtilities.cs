using Shell32;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tile.Core.Util
{
    /// <summary>
    /// Various utilities for tile generation
    /// </summary>
    public static class FileUtilities
    {
        /// <summary>
        /// Get all the files matching a search pattern
        /// in a list of directories
        /// </summary>
        /// <param name="searchPattern">Search string files must match</param>
        /// <param name="paths">Directories to explore</param>
        /// <returns>Paths to found files</returns>
        public static List<string> FindFiles(string searchPattern, List<string> paths) {
            var files = new List<string>();
            foreach (var path in paths)
                try {
                    files.AddRange(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories));
                } catch (Exception) {
                    continue;
                }
            return files;
        }

        /// <summary>
        /// Get the file targetted by a shortcut
        /// </summary>
        /// <param name="shortcutPath">Path to the shortcut file (.lnk)</param>
        /// <returns>Path to the file targetted by the shortcut</returns>
        public static string GetShortcutTargetFile(string shortcutPath) {
            string pathOnly = Path.GetDirectoryName(shortcutPath);
            string filenameOnly = Path.GetFileName(shortcutPath);

            var shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null) {
                ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }
            return string.Empty;
        }
    }
}
