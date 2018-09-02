using System.Collections.Generic;

namespace Tile.Core.Util
{
    /// <summary>
    /// Dictionary extension methods
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Return a new dictionary with only the values
        /// identified by the given keys.
        /// </summary>
        /// <typeparam name="TKey">Keys type</typeparam>
        /// <typeparam name="TValue">Values type</typeparam>
        /// <param name="input">Input dictionary</param>
        /// <param name="keys">Keys to keep</param>
        /// <returns>The filtered dictionary</returns>
        public static Dictionary<TKey, TValue> Keep<TKey, TValue>(this Dictionary<TKey, TValue> input, IEnumerable<TKey> keys)
        {
            var output = new Dictionary<TKey, TValue>();
            foreach (var key in keys)
                if (input.ContainsKey(key))
                    output.Add(key, input[key]);
            return output;
        }
    }
}
