using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using JetBrains.Annotations;

namespace Robust.Shared.Maths;

public static unsafe partial class NumericsHelpers
{
    /*
     Partial class for Add implementations that handle Vector512<float> types as well as spans of them.
     */

    /// <summary>
    /// Adds a to b and stores the result in a.
    /// </summary>
    [PublicAPI]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(Span<Vector512<float>> a, Span<Vector512<float>> b)
    {
        Add(a, b, a);
    }

    /// <summary>
    /// Adds a to b and stores the result in result.
    /// </summary>
    [PublicAPI]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ReadOnlySpan<Vector512<float>> a, ReadOnlySpan<Vector512<float>> b, Span<Vector512<float>> result)
    {
        if (a.Length != b.Length || a.Length != result.Length)
            throw new ArgumentException("Length of arrays must be the same!");

        if (Vector512Enabled)
        {
            AddVector512(a, b, result);
            return;
        }

        if (Vector256Enabled)
        {
            AddVector256(AsVector256Span(a), AsVector256Span(b), AsVector256Span(result));
            return;
        }

        AddVector128(AsVector128Span(a), AsVector128Span(b), AsVector128Span(result));
    }

    /// <summary>
    /// Adds a to b and stores the result in result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void AddVector512(ReadOnlySpan<Vector512<float>> a, ReadOnlySpan<Vector512<float>> b, Span<Vector512<float>> result)
    {
        for (var i = 0; i < a.Length; i++)
        {
            result[i] = Vector512.Add(a[i], b[i]);
        }
    }

    /// <summary>
    /// Adds a to b and stores the result in result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void AddVector256(ReadOnlySpan<Vector256<float>> a, ReadOnlySpan<Vector256<float>> b, Span<Vector256<float>> result)
    {
        for (var i = 0; i < a.Length; i++)
        {
            result[i] = Vector256.Add(a[i], b[i]);
        }
    }

    /// <summary>
    /// Adds a to b and stores the result in result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void AddVector128(ReadOnlySpan<Vector128<float>> a, ReadOnlySpan<Vector128<float>> b, Span<Vector128<float>> result)
    {
        for (var i = 0; i < a.Length; i++)
        {
            result[i] = Vector128.Add(a[i], b[i]);
        }
    }

    #region Span Reinterpretation

    // Probably the most insane section I've ever written.

    /// <summary>
    /// Reinterprets a span of <see cref="Vector512{T}"/> as a span of <see cref="Vector256{T}"/> over the same backing memory.
    /// The resulting span length is 2x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Span<Vector256<float>> AsVector256Span(Span<Vector512<float>> source)
    {
        // conversion: vec512 is 64 bytes, vec256 is 32 bytes, so the resulting span will be twice as long
        return MemoryMarshal.Cast<Vector512<float>, Vector256<float>>(source);
    }

    /// <summary>
    /// Reinterprets a read\-only span of <see cref="Vector512{T}"/> as a read\-only span of <see cref="Vector256{T}"/> over the same backing memory.
    /// The resulting span length is 2x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlySpan<Vector256<float>> AsVector256Span(ReadOnlySpan<Vector512<float>> source)
    {
        return MemoryMarshal.Cast<Vector512<float>, Vector256<float>>(source);
    }

    /// <summary>
    /// Reinterprets a span of <see cref="Vector512{T}"/> as a span of <see cref="Vector128{T}"/> over the same backing memory.
    /// The resulting span length is 4x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Span<Vector128<float>> AsVector128Span(Span<Vector512<float>> source)
    {
        // conversion: vec512 is 64 bytes, vec128 is 16 bytes, so the resulting span will be four times as long
        return MemoryMarshal.Cast<Vector512<float>, Vector128<float>>(source);
    }

    /// <summary>
    /// Reinterprets a read\-only span of <see cref="Vector512{T}"/> as a read\-only span of <see cref="Vector128{T}"/> over the same backing memory.
    /// The resulting span length is 4x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlySpan<Vector128<float>> AsVector128Span(ReadOnlySpan<Vector512<float>> source)
    {
        return MemoryMarshal.Cast<Vector512<float>, Vector128<float>>(source);
    }

    /// <summary>
    /// Reinterprets a span of <see cref="Vector256{T}"/> as a span of <see cref="Vector128{T}"/> over the same backing memory.
    /// The resulting span length is 2x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Span<Vector128<float>> AsVector128Span(Span<Vector256<float>> source)
    {
        // conversion: vec256 is 32 bytes, vec128 is 16 bytes, so the resulting span will be twice as long
        return MemoryMarshal.Cast<Vector256<float>, Vector128<float>>(source);
    }

    /// <summary>
    /// Reinterprets a read\-only span of <see cref="Vector256{T}"/> as a read\-only span of <see cref="Vector128{T}"/> over the same backing memory.
    /// The resulting span length is 2x the input length.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlySpan<Vector128<float>> AsVector128Span(ReadOnlySpan<Vector256<float>> source)
    {
        return MemoryMarshal.Cast<Vector256<float>, Vector128<float>>(source);
    }

    #endregion
}
