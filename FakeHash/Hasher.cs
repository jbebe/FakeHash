using FakeHash.Crypto;
using FakeHash.Utils;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace FakeHash
{
  internal static class Hasher
  {
    private enum StoringMethod
    {
      Binary, Packed
    }

    private static (string key, string iv) KeyData { get; } = 
      ("Key must be 32 characters long..", "IV is only 16...");

    public const int HeadLength = 1;
    private static Lazy<ICrypto> Crypto { get; } =
      new Lazy<ICrypto>(() => new NetCoreEncrypt(KeyData.key, KeyData.iv));
    
    public static byte[] GetHash(StoringMethod method, byte[] input, int outputLength)
    {
      ValidateGetHashParams(input, outputLength);

      // Basic structure of the ecryptable data:
      // 
      // [0: binary data][             first 7 bits of data length              ] [   payload  ]
      // [1: packed type][size of custom type on 3 bits][first 4 bits of payload] [   payload  ]
      // \-------------------------------- 1st byte ----------------------------/ \--- rest ---/
      // 
      // Remarks:
      //  - Binary data:
      //    * 7 bits hold numbers up to 128.
      //    * We don't need more than that. Such a long hash function does not look legitimate.
      //    * This is the default mode as we just encrypt bytes.
      //  - Packed type:
      //    * Emphasis on packing as much payload as possible
      //    * No data length stored as it costs precious amount of bits
      //    * Best way to store alphanumeric values
      //

      byte[] encryptable;

      if (method == StoringMethod.Binary)
      {
        encryptable = GetHashBinary(input, outputLength);
      }
      else if (method == StoringMethod.Packed)
      {
        encryptable = GetHashPacked(input, outputLength);
      }
      else
      {
        throw new ArgumentOutOfRangeException("Unsupported storing method!");
      }

      // While creating the fake hash, we have to imitate the hash behavior.
      // If someone checks the md5 hash of 'A' and it is "00....0041" then checks 'B' 
      // which is "...0042", well that would be pretty suspicious. 
      // In Layman's terms, I chose AES to low-effort scramble the output.
      var encrypted = Crypto.Value.Encrypt(encryptable);

      return encrypted;
    }

    private static byte[] GetHashPacked(byte[] input, int outputLength)
    {
      throw new NotImplementedException();
    }

    private static byte[] GetHashBinary(byte[] input, int outputLength)
    {
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
      var header = new BitArray(8);
      header.Set(0, 0);
      header.Set(1, 7, inputLength);
      var encryptable = header
        .Concat(input)
        .Concat(paddingZeros)
        .Take(outputLength)
        .ToArray();

      return encryptable;
    }

    // Yep. Unhash is not a word. For a reason.
    public static byte[] Unhash(byte[] input)
    {
      ValidateUnhashParams(input);

      var decrypted = Crypto.Value.Decrypt(input);
      var payloadLength = decrypted[0];
      var payload = decrypted.Skip(1).Take(payloadLength).ToArray();

      return payload;
    }

    public static string Unhash(byte[] input, Func<byte[], string> decoder)
    {
      decoder = decoder ?? Encoding.UTF8.GetString;
      var output = Unhash(input);

      return decoder(output);
    }

    private static void ValidateGetHashParams(byte[] input, int outputLength)
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
    }

    private static void ValidateUnhashParams(byte[] input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("Input parameter is null!");
      }
      if (input.Length == 0)
      {
        throw new ArgumentOutOfRangeException("Input parameter is empty!");
      }
    }
  }
}
