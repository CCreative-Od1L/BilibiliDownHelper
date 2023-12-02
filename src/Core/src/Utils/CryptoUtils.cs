using System.Security.Cryptography;

namespace Core.Utils {
    public static class CryptoUtils {
        static readonly Aes aesAlg;
        static readonly string AesKeyFileName = @"key.bin";
        static readonly string AesIVFileName = @"iv.bin";
        static CryptoUtils() {
            string? cryptoDirectory = CoreManager.directoryMgr.fileDirectory.Crypto;
            aesAlg = Aes.Create();
            
            if(string.IsNullOrEmpty(cryptoDirectory)) {
                CoreManager.directoryMgr.ResetToDefault("crypto");
            }
            if (!System.IO.Directory.Exists(cryptoDirectory)) {
                System.IO.Directory.CreateDirectory(cryptoDirectory!);
            }
            string AesKeyFilePath = Path.Combine(cryptoDirectory!, AesKeyFileName);
            if (File.Exists(AesKeyFilePath)) {
                aesAlg.Key = FileUtils.ReadBytesFile(AesKeyFilePath);
            } else {
                FileUtils.WriteBytes(AesKeyFilePath, aesAlg.Key);
            }
            
            string AesIVFilePath = Path.Combine(cryptoDirectory!, AesIVFileName);
            if (File.Exists(AesIVFilePath)) {
                aesAlg.IV = FileUtils.ReadBytesFile(AesIVFilePath);
            } else {
                FileUtils.WriteBytes(AesIVFilePath, aesAlg.IV);
            }
        }
        public static byte[] AesEncryptStringToBytes(string plainText) {
            if(string.IsNullOrEmpty(plainText)) { return []; }

            byte[] encrypted;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new()) {
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt)) {
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
            return encrypted;
        }
        public static string AesDecryptBytesToString(byte[] cipherText) {
            if(cipherText == null || cipherText.Length <= 0) { return string.Empty; }

            string plainText = string.Empty;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new(cipherText)) {
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                plainText = srDecrypt.ReadToEnd();
            }
            return plainText;
        }
    }
}