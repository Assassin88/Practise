using System.IO;
using System.Security.Cryptography;

namespace Sababa.Data.Storage.Classes
{
    public class Crypt
    {
        private readonly string _password;
        public Crypt(string password)
        {
            _password = password;
        }

        /// <summary>
        /// Encrypts the input array of bytes
        /// </summary>
        /// <param name="data">Array byte</param>
        /// <returns>Returns encrypted byte array</returns>
        public byte[] Encrypt(byte[] data)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            var ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(_password, null)).GetBytes(16),
                new byte[16]);

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        /// <summary>
        /// Returns the decrypted data to external users
        /// </summary>
        /// <param name="data">Encrypted byte array</param>
        /// <returns>Returns decrypt array byte</returns>
        public byte[] Decrypt(byte[] data)
        {
            using (var memStream = new MemoryStream())
            {
                InternalDecrypt(data).CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// Decodes the passed byte array
        /// </summary>
        /// <param name="data">Encrypted byte array</param>
        /// <returns>Returns decrypt array byte</returns>
        private CryptoStream InternalDecrypt(byte[] data)
        {
            var sa = Rijndael.Create();
            var ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(_password, null)).GetBytes(16),
                new byte[16]);

            var ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}
