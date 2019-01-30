using System;
using System.Linq;
using System.Text;

namespace FakeHashTest.Helper
{
  static class RandomHelper
  {
    private static Lazy<Random> Random { get; } = new Lazy<Random>(() => new Random());

    public static byte[] GenerateRandomBytes(int size)
    {
      byte[] data = new byte[size];

      var rand = new Random();
      rand.NextBytes(data);

      return data;
    }

    public static string GenerateRandomString(int size)
    {
      const string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      var builder = new StringBuilder(size);

      foreach (int i in Enumerable.Range(0, size))
      {
        char randomCharacter = alphanumeric[Random.Value.Next(alphanumeric.Length - 1)];
        builder.Append(randomCharacter);
      }

      return builder.ToString();
    }
  }
}
