### Have you ever dreamt of unhashing things?
Of course you have. They told you to hash that data because it's unsafe to store a password or even an email address in the database. So what? It's not like you would let anyone close to that sensitive data by making one mistake in your input sanitization. Someone, maybe, but not you. Anyway. Here's my solution to all your problems.

### Idea
Encrypt the data instead of hashing it. Just make it look like it was hashed. Sure, you lose precious information if the chosen hash size is smaller than the input but that's a sacrifice we have to make.

### Example

When the secret is shorter than the hash length:

```C#
var algo = Algorithms.SHA2_256;
string secret = "your_pathetic_password";
var hash = TotallyLegitHash.GetHash(algo, secret);
var unhashedSecret = TotallyLegitHash.UnhashString(hash);
// secret == unhashedSecret
```

When the secret is longer than the hash length:

```C#
var algo = Algorithms.MD5;
string secret = "abcdefghijklmnopqrstuvwxyz";
var secretShort = secret.SubstringBytesToString(TotallyLegitHash.EffectiveByteCount(algo)); // Helper functions
var hash = TotallyLegitHash.GetHash(algo, secret);
var unhashedSecret = TotallyLegitHash.UnhashString(hash);
// secretShort == unhashedSecret
```

And binary of course:

```C#
var algo = Algorithms.MD5;
var effectiveByteCount = TotallyLegitHash.EffectiveByteCount(algo);
var secret = RandomHelper.GenerateRandomBytes(1000);
var secretShort = new Span<byte>(secret, 0, effectiveByteCount).ToArray();
var hash = TotallyLegitHash.GetHash(algo, secret);
var unhashedSecret = TotallyLegitHash.Unhash(hash);

// secretShort == unhashedSecret
```

### "Architecture"

![diagram](https://github.com/jbebe/FakeHash/raw/master/diagram.png "Diagram")
