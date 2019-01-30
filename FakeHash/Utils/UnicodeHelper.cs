using FakeHash.Utils;
using NETCore.Encrypt;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FakeHash.Utils
{
  public static class UnicodeHelper
  {
    public static string SubstringBytesToString(this string input, int length, Func<string, byte[]> encoder = null, Func<byte[], string> decoder = null)
    {
      decoder = decoder ?? Encoding.UTF8.GetString;
      var bytes = SubstringBytesToArray(input, length, encoder);
      return decoder(bytes);
    }

    public static byte[] SubstringBytesToArray(this string input, int length, Func<string, byte[]> encoder = null)
    {
      encoder = encoder ?? Encoding.UTF8.GetBytes;
      byte[] byteArray = encoder(input);

      if (byteArray.Length > length)
      {
        int bytePointer = length;

        // Check high bit to see if we're potentially in the middle of a multi-byte char
        if (bytePointer >= 0
            && (byteArray[bytePointer] & Convert.ToByte("10000000", 2)) > 0)
        {
          // If so, keep walking back until we have a byte starting with `11`,
          // which means the first byte of a multi-byte UTF8 character.
          while (bytePointer >= 0
              && Convert.ToByte("11000000", 2) != (byteArray[bytePointer] & Convert.ToByte("11000000", 2)))
          {
            bytePointer--;
          }
        }

        // See if we had 1s in the high bit all the way back. If so, we're toast. Return empty string.
        if (0 != bytePointer)
        {
          return byteArray.Take(length).ToArray();
        }
      }

      return byteArray;
    }
  }
}
