using System.Collections;
using System.Numerics;

namespace LogScraper.Utilities.Extensions
{
    public static class BitArrayExtensions
    {
        /// <summary>
        /// Counts the number of bits set to true in the given BitArray.
        /// </summary>
        /// <param name="bitArray">The BitArray to count set bits in.</param>
        /// <returns>The number of bits set to true.</returns>
        public static int CountSetBits(this BitArray bitArray)
        {
            var integers = new int[(bitArray.Length + 31) / 32];
            bitArray.CopyTo(integers, 0);

            int count = 0;
            foreach (int value in integers)
                count += BitOperations.PopCount((uint)value);

            return count;
        }
    }
}
