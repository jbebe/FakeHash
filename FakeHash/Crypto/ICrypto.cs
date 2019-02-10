using System;
using System.Collections.Generic;
using System.Text;

namespace FakeHash.Crypto
{
  interface ICrypto
  {
    byte[] Encrypt(byte[] input);
    byte[] Decrypt(byte[] input);
  }
}
