using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeHash.Crypto
{
  class NetCoreEncrypt : ICrypto
  {
    public string Key { get; }
    public string IV { get; }

    public NetCoreEncrypt(string key, string iv)
    {
      Validate(key, iv);
      Key = key;
      IV = iv;
    }

    public byte[] Decrypt(byte[] input)
      => EncryptProvider.AESDecrypt(input, Key, IV);

    public byte[] Encrypt(byte[] input)
      => EncryptProvider.AESEncrypt(input, Key, IV);

    private static void Validate(string key, string iv)
    {
      if (key.Length != 32)
      {
        throw new ArgumentOutOfRangeException("Key must be 32 bytes!");
      }
      if (iv.Length != 16)
      {
        throw new ArgumentOutOfRangeException("IV must be 16 bytes!");
      }
    }
  }
}
