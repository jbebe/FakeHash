using FakeHashTest.Helper;
using FakeHash.Utils;
using System;
using Xunit;
using FakeHash;

namespace FakeHashTest
{
  public class Test
  {
    [Fact]
    public void BinaryData_Long_Idempotence()
    {
      var algo = Algorithms.MD5;
      var effectiveByteCount = TotallyLegitHash.EffectiveByteCount(algo);
      var secret = RandomHelper.GenerateRandomBytes(1000);
      var secretShort = new Span<byte>(secret, 0, effectiveByteCount).ToArray();
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.Unhash(hash);

      Assert.Equal(effectiveByteCount, unhashedSecret.Length);
      Assert.Equal(secretShort, unhashedSecret);
    }

    [Fact]
    public void BinaryData_Short_Idempotence()
    {
      var algo = Algorithms.MD5;
      var secret = new byte[] { 1 };
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.Unhash(hash);

      Assert.Equal(secret.Length, unhashedSecret.Length);
      Assert.Equal(secret, unhashedSecret);
    }

    [Fact]
    public void Unicode_Short_Idempotence()
    {
      var algo = Algorithms.MD5;
      string secret = "abcd";
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.UnhashString(hash);

      Assert.Equal(secret.Length, unhashedSecret.Length);
      Assert.Equal(secret, unhashedSecret);
    }

    [Fact]
    public void Unicode_Long_Idempotence()
    {
      var algo = Algorithms.MD5;
      string secret = "a가b각c갂d간e갅f갆g갇h갈i갉j갊k갋l갌m갍n갎o갏p감q갑r값s갓t갔u강v갖w갗x갘y같z";
      var secretShort = secret.SubstringBytesToString(TotallyLegitHash.EffectiveByteCount(algo));
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.UnhashString(hash);

      Assert.Equal(secretShort, unhashedSecret);
    }

    [Fact]
    public void BinaryData_Short_Error()
    {
      var algo = Algorithms.MD5;
      var secret = new byte[] { 1 };

      Assert.Throws<ArgumentNullException>(() => TotallyLegitHash.GetHash(algo, null));
      Assert.Throws<ArgumentOutOfRangeException>(() => TotallyLegitHash.GetHash(algo, new byte[] { }));

      Assert.Throws<ArgumentNullException>(() => TotallyLegitHash.Unhash(null));
      Assert.Throws<ArgumentOutOfRangeException>(() => TotallyLegitHash.Unhash(new byte[] { }));
    }

    [Fact]
    public void Unicode_RealWorldScenario()
    {
      var algo = Algorithms.SHA2_256;
      string secret = "john.smith@example.com";
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.UnhashString(hash);

      Assert.Equal(secret, unhashedSecret);
    }

    [Fact]
    public void Unicode_RealWorldScenario2()
    {
      var algo = Algorithms.SHA2_256;
      string secret = "your_pathetic_password";
      var hash = TotallyLegitHash.GetHash(algo, secret);
      var unhashedSecret = TotallyLegitHash.UnhashString(hash);

      Assert.Equal(secret, unhashedSecret);
    }
  }
}
