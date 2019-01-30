using NETCore.Encrypt;
using System;
using System.Linq;
using System.Text;

namespace FakeHash
{
  internal static class Hasher
  {
    public const int HeadLength = 1;

    private static (string key, string iv) KeyData { get; } = 
      ("Key must be 32 characters long..", "IV is only 16...");

    public static byte[] GetHash(byte[] input, int outputLength)
    {
      if (input == null)
      {
        throw new ArgumentNullException("Input parameter must not be null!");
      }
      if (input.Length == 0)
      {
        throw new ArgumentOutOfRangeException("Input parameter must not be empty!");
      }
      if (outputLength < 1)
      {
        throw new ArgumentOutOfRangeException("Output length must be a positive integer!");
      }

      // If the input is shorter than the output:
      // 
      // [         11 ] ["abcdefghijk"] ["             "]
      //                [ inputLength ] [ paddingLength ]
      // [ HeadLength ] [         payloadLength         ]
      // [                 outputLength                 ]

      int payloadLength = outputLength - HeadLength;
      int paddingLength;
      int inputLength = input.Length;

      if (inputLength < payloadLength)
      {
        paddingLength = payloadLength - inputLength;
      }
      else
      {
        paddingLength = 0;
        inputLength = payloadLength;
      }

      var paddingZeros = new byte[paddingLength];
      var header = new byte[] { (byte)inputLength };
      var encryptable = header
        .Concat(input)
        .Concat(paddingZeros)
        .Take(outputLength)
        .ToArray();

      // While creating the fake hash, we have to imitate the hash behavior.
      // If someone checks the md5 hash of 'A' and it is "00....0041" then checks 'B' 
      // which is "...0042", well that would be pretty suspicious. 
      // In Layman's terms, I chose AES to low-effort scramble the output.
      var encrypted = EncryptProvider.AESEncrypt(encryptable, KeyData.key, KeyData.iv);

      return encrypted;
    }

    // Yep. Unhash is not a word. For a reason.
    public static byte[] Unhash(byte[] input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("Input parameter is null!");
      }
      if (input.Length == 0)
      {
        throw new ArgumentOutOfRangeException("Input parameter is empty!");
      }

      var decryptedData = EncryptProvider.AESDecrypt(input, KeyData.key, KeyData.iv);
      var header = decryptedData[0];
      var payload = decryptedData.Skip(1).Take(header).ToArray();

      return payload;
    }

    public static string Unhash(byte[] input, Func<byte[], string> decoder)
    {
      decoder = decoder ?? Encoding.UTF8.GetString;
      var output = Unhash(input);

      return decoder(output);
    }
  }
}
