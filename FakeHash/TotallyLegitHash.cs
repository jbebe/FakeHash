using FakeHash.Utils;
using System;
using static FakeHash.Algorithms;

namespace FakeHash
{
  public static class TotallyLegitHash
  {
    public static int EffectiveByteCount(HashingAlgorithm algo)
      => EffectiveByteCount((int)algo);

    public static int EffectiveByteCount(int outputLength)
      => outputLength - Hasher.HeadLength;

    public static byte[] GetHash(HashingAlgorithm algo, string input, Func<string, byte[]> encoder = null)
    {
      int algoLength = (int)algo;
      int effectiveByteLength = EffectiveByteCount(algoLength);
      var binaryInput = input.SubstringBytesToArray(effectiveByteLength, encoder);

      return Hasher.GetHash(binaryInput, algoLength);
    }

    public static byte[] GetHash(HashingAlgorithm algo, byte[] input)
    {
      int algoLength = (int)algo;
      return Hasher.GetHash(input, algoLength);
    }

    public static byte[] Unhash(byte[] input)
      => Hasher.Unhash(input);

    public static string UnhashString(byte[] input, Func<byte[], string> decoder = null)
      => Hasher.Unhash(input, decoder);
  }
}
