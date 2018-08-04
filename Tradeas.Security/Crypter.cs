using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tradeas.Security
{
    public static class Crypter
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string InitVector = "M56BW1z2iUWtWmzK";
        private const string Salt = "Tr@d3@s.As1n";

        // This constant is used to determine the keysize of the encryption algorithm
        private const int Keysize = 256;

        //Encrypt
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public static string EncryptString(string plainText, string passPhrase)
        {
            var salt = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(passPhrase, salt, 10000);
            var keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            byte[] cipherTextBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string passPhrase)
        {
            var salt = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(passPhrase, salt, 10000);
            var keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            int? decryptedByteCount;
            byte[] plainTextBytes;
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    plainTextBytes = new byte[cipherTextBytes.Length];
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                }
            }
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount.Value);
        }
    }
}