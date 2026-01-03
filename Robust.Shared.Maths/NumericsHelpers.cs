using System;
using System.Runtime.Intrinsics;

namespace Robust.Shared.Maths
{
    public static partial class NumericsHelpers
    {
        #region Constructor & Environment Variables

        // Misnomer due to historical reasons.
        public const string AvxEnvironmentVariable = "ROBUST_NUMERICS_AVX";

        /// <summary>
        /// Whether AVX-256 is enabled.
        /// </summary>
        public static readonly bool Vector256Enabled;

        /// <summary>
        /// Whether AVX-512 is enabled.
        /// </summary>
        public static readonly bool Vector512Enabled;

        static NumericsHelpers()
        {
            var envVar = Environment.GetEnvironmentVariable(AvxEnvironmentVariable);
            var avxEnabled = envVar != null && bool.Parse(envVar);
            Vector256Enabled = Vector256.IsHardwareAccelerated && avxEnabled;
            Vector512Enabled = Vector512.IsHardwareAccelerated && avxEnabled;
        }

        #endregion

        #region Utils

        /// <summary>
        ///     Returns whether the specified array length is valid for loading into 256-bit registers.
        /// </summary>
        private static bool LengthValid256Single(int arrayLength)
        {
            return arrayLength >= 8;
        }

        /// <summary>
        /// Returns whether the specified array length is valid for loading into 512-bit registers.
        /// </summary>
        /// <param name="arrayLength">The length of the array.</param>
        /// <returns>True if the length is valid for 512-bit operations; otherwise, false.</returns>
        private static bool ValidLength512Single(int arrayLength)
        {
            return arrayLength >= 16;
        }

        #endregion

    }
}
