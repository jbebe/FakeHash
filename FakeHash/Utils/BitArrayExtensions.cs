using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeHash.Utils
{
  static class BitArrayExtensions
  {
    /// <summary>
    /// Set numeric value at index.
    /// </summary>
    public static BitArray Set(this BitArray array, int index, int value)
    {
      if (index < 0 || index >= array.Length)
      {
        throw new ArgumentOutOfRangeException("Index must be a valid value!");
      }
      if (value != (value & 1))
      {
        throw new ArgumentOutOfRangeException("Value must be 0 or 1!");
      }

      array.Set(index, Convert.ToBoolean(value));

      return array;
    }

    /// <summary>
    /// Set numeric value at range.
    /// </summary>
    public static BitArray Set(this BitArray array, int index, int count, int value)
    {
      if (index < 0 || index >= array.Length)
      {
        throw new ArgumentOutOfRangeException("Index must be a valid value!");
      }
      if (count <= 0 || (index + count) >= array.Length)
      {
        throw new ArgumentOutOfRangeException("Count must be a valid value!");
      }
      if (value != (value & 1))
      {
        throw new ArgumentOutOfRangeException("Value must be 0 or 1!");
      }

      var valueAsBits = new BitArray(count);
      foreach (int i in Enumerable.Range(0, count))
      {
        int bitMask = 1 << i;
        int offset = i;
        bool bitAtPosition = ((value & bitMask) >> offset) == 1;
        valueAsBits[count - i - 1] = bitAtPosition;
      }

      valueAsBits.CopyTo(array, index);

      return array;
    }

    /// <summary>
    /// Set numeric value at range.
    /// </summary>
    public static BitArray CopyTo(this BitArray arrayFrom, BitArray arrayTo, int index)
    {
      foreach (int i in Enumerable.Range(0, arrayFrom.Length))
      {
        bool bit = arrayFrom[i];
        arrayTo.Set(index + i, bit);
      }

      return arrayFrom;
    }
  }
}
