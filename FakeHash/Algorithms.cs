using System;
using System.Collections.Generic;
using System.Text;

namespace FakeHash
{
  public class Algorithms
  {
    public enum HashingAlgorithm : int
    {
      MD5 = 16,
      SHA1 = 20,
      SHA2_256 = 32,
      SHA2_512 = 64
    }

    public const HashingAlgorithm MD5 = HashingAlgorithm.MD5;
    public const HashingAlgorithm SHA1 = HashingAlgorithm.SHA1;
    public const HashingAlgorithm SHA2_256 = HashingAlgorithm.SHA2_256;
    public const HashingAlgorithm SHA2_512 = HashingAlgorithm.SHA2_512;
  }
}
