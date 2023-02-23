![](https://github.com/viktor111/SharpCipher/actions/workflows/dotnet.yml/badge.svg)
# SharpCipher
SharpCipher is a C# implementation of a symmetric encryption algorithm that uses the AES encryption algorithm in CBC mode. It also employs key derivation using PBKDF2 with SHA-1 as the underlying hash function.

## Download
`dotnet add package SharpCipher`

## Usage
To use SharpCipher, you can create a new instance of the Cipher class and call its Encrypt and Decrypt methods to encrypt and decrypt data, respectively. Here's an example:

```csharp
using SharpCipher;

// ...

var cipher = new Cipher();
var key = "my secret key";
var plaintext = "Hello, world!";
var ciphertext = cipher.Encrypt(plaintext, key);
var decrypted = cipher.Decrypt(ciphertext, key);

Console.WriteLine(decrypted); // Output: Hello, world!
```
Note that you should use a strong and unique key for each encryption to ensure the confidentiality of the data.

## API
`Cipher`

The `Cipher` class provides methods for encrypting and decrypting data using AES in CBC mode with key derivation using PBKDF2.
Encrypt(plainText: string, key: string) -> string

Encrypts the input plaintext using AES in CBC mode with a randomly generated IV and key derived from the input key using PBKDF2.

- `plainText` - The plaintext to encrypt.
- `key` - The key to use for encryption.

Returns the ciphertext as a base64-encoded string.

Throws a `CipherException` if the plaintext or key is null white space or empty.
Decrypt(ciphertext: string, key: string) -> string

Decrypts the input ciphertext using AES in CBC mode with the key derived from the input key using PBKDF2.

- `ciphertext` - The ciphertext to decrypt, as a base64-encoded string.
- `key` - The key to use for decryption.

Returns the decrypted plaintext as a string.

Throws a `CipherException` if the ciphertext or key is null white space or empty.
CipherException

The `CipherException` class represents an exception that occurred during encryption or decryption.
