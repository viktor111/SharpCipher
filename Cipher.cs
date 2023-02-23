using System.Security.Cryptography;

namespace SharpCipher
{
    public class Cipher
    {
        private const int BlockBitSize = 128;
        private const int KeyBitSize = 256;
        private const int SaltBitSize = 128;
        private const int Iterations = 10000;

        public string Encrypt(string plainText, string key)
        {
            if(string.IsNullOrWhiteSpace(plainText))
                throw new CipherException("Plain text cannot be null");
            
            if(string.IsNullOrWhiteSpace(key))
                throw new CipherException("Key cannot be null");
            
            using var keyDerivationFunction = new Rfc2898DeriveBytes(key, SaltBitSize / 8, Iterations);
            using var aesManaged = Aes.Create();
            aesManaged.KeySize = KeyBitSize;
            aesManaged.BlockSize = BlockBitSize;

            aesManaged.GenerateIV();

            var saltBytes = keyDerivationFunction.Salt;
            var keyBytes = keyDerivationFunction.GetBytes(KeyBitSize / 8);
            var ivBytes = aesManaged.IV;

            using var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
            }

            var cipherTextBytes = memoryStream.ToArray();

            Array.Resize(ref saltBytes, saltBytes.Length + ivBytes.Length);
            Array.Copy(ivBytes, 0, saltBytes, SaltBitSize / 8, ivBytes.Length);

            Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
            Array.Copy(cipherTextBytes, 0, saltBytes, SaltBitSize / 8 + ivBytes.Length, cipherTextBytes.Length);

            return Convert.ToBase64String(saltBytes);
        }

        public string Decrypt(string ciphertext, string key)
        {
            if (string.IsNullOrWhiteSpace(ciphertext))
                throw new CipherException("Cipher text cannot be null");

            if (string.IsNullOrWhiteSpace(key))
                throw new CipherException("Key cannot be null");
            
            var saltBytes = new byte[SaltBitSize / 8];
            var ivBytes = new byte[BlockBitSize / 8];

            var allTheBytes = Convert.FromBase64String(ciphertext);

            Array.Copy(allTheBytes, 0, saltBytes, 0, saltBytes.Length);
            Array.Copy(allTheBytes, saltBytes.Length, ivBytes, 0, ivBytes.Length);

            var ciphertextBytes = new byte[allTheBytes.Length - saltBytes.Length - ivBytes.Length];
            Array.Copy(allTheBytes, saltBytes.Length + ivBytes.Length, ciphertextBytes, 0, ciphertextBytes.Length);

            using var keyDerivationFunction = new Rfc2898DeriveBytes(key, saltBytes, Iterations);
            var keyBytes = keyDerivationFunction.GetBytes(KeyBitSize / 8);

            using var aesManaged = Aes.Create();
            aesManaged.KeySize = KeyBitSize;
            aesManaged.BlockSize = BlockBitSize;

            using var decrypt = aesManaged.CreateDecryptor(keyBytes, ivBytes);
            using var memoryStream = new MemoryStream(ciphertextBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}