using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Infrastructure.Encryption
{
    public static class TripleDes
    {
        private static readonly string CryptingKey = "KeyForCrypting";
        public static string Encrypted(int Id)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(CryptingKey));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB;
            byteBuff = Encoding.UTF8.GetBytes(Id.ToString());

            string encoded = Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

            return encoded;
        }
        public static int Decrypted(string CryptedId)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(CryptingKey));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB;
            try
            {
                byteBuff = Convert.FromBase64String(CryptedId);
            }
            catch (Exception)
            {
                return -1;
            }
            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return Convert.ToInt32(plaintext);

        }
    }
}
