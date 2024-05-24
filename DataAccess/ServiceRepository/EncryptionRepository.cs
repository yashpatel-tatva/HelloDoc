namespace DataAccess.ServiceRepository
{
    public static class EncryptionRepository
    {
        //private static readonly string EncryptionKey = GenerateRandomKey(256);

        //public static string Encrypt(string plainText)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Convert.FromBase64String(EncryptionKey);
        //        aesAlg.IV = GenerateRandomIV(); // Generate a random IV for each encryption

        //        aesAlg.Padding = PaddingMode.PKCS7; // Set the padding mode to PKCS7

        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    swEncrypt.Write(plainText);
        //                }
        //            }
        //            return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
        //        }
        //    }
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    cipherText = cipherText.Trim();
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);

        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Convert.FromBase64String(EncryptionKey);
        //        aesAlg.IV = cipherBytes.Take(16).ToArray();

        //        aesAlg.Padding = PaddingMode.PKCS7; // Set the padding mode to PKCS7

        //        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msDecrypt = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16))
        //        {
        //            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                {
        //                    return srDecrypt.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}

        //private static byte[] GenerateRandomIV()
        //{
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        var randomBytes = new byte[16];
        //        rng.GetBytes(randomBytes);
        //        return randomBytes;
        //    }
        //}

        //private static string GenerateRandomKey(int size)
        //{
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        var randomBytes = new byte[size / 8];
        //        rng.GetBytes(randomBytes);
        //        return Convert.ToBase64String(randomBytes);
        //    }
        //}


        //public static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        //public static string Encrypt(string plainText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] array;

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.IV = iv;

        //        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
        //                {
        //                    streamWriter.Write(plainText);
        //                }

        //                array = memoryStream.ToArray();
        //            }
        //        }
        //    }

        //    return Convert.ToBase64String(array);
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] buffer = Convert.FromBase64String(cipherText);

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.IV = iv;
        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream(buffer))
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
        //                {
        //                    return streamReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}


        private static readonly char[] Digits = "0123456789".ToCharArray();
        private static readonly char[] Alphabets = "ABCDEFGHIJ".ToCharArray();

        public static string Encrypt(string plainText)
        {
            return new string(plainText.Select(ch => EncryptCharacter(ch)).ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            return new string(cipherText.Select(ch => DecryptCharacter(ch)).ToArray());
        }

        private static char EncryptCharacter(char ch)
        {
            int index = Array.IndexOf(Digits, ch);
            return index >= 0 ? Alphabets[index] : ch;
        }

        private static char DecryptCharacter(char ch)
        {
            int index = Array.IndexOf(Alphabets, ch);
            return index >= 0 ? Digits[index] : ch;
        }



    }


}
