using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Core.Utils {
    public static class CryptoUtils {
        static readonly Aes aesAlg;
        static readonly string AesCryptFileName = @"aes.bin";
        static readonly byte[] InnerAesCryptKey = [
            0x31, 0x0d, 0xba, 0x6a, 0x56, 0xf1, 0xa3, 0x59,
            0xe5, 0xd4, 0xdb, 0x26, 0xc7, 0x1f, 0x04, 0xa2,
            0x95, 0x6a, 0x52, 0xef, 0x76, 0x91, 0xd9, 0x4b,
            0x07, 0xbc, 0x0c, 0x6a, 0x2b, 0x38, 0x81, 0x50,
        ];
        static readonly byte[] InnerAesCryptIV = [
            0x11, 0x97, 0xac, 0x54, 0xa8, 0x3d, 0x56, 0x73,
            0xdf, 0x8a, 0x83, 0x60, 0xd0, 0x93, 0xc2, 0x02,
        ];
        static CryptoUtils() {
            string? cryptoDirectory = CoreManager.directoryMgr.fileDirectory.Crypto;

            if(string.IsNullOrEmpty(cryptoDirectory)) {
                CoreManager.directoryMgr.ResetToDefault("crypto");
                CoreManager.logger.Info(nameof(CryptoUtils), "Crypto文件夹路径为空，尝试初始化路径。");
            }
            if (!Directory.Exists(cryptoDirectory)) {
                Directory.CreateDirectory(cryptoDirectory!);
                CoreManager.logger.Info(nameof(CryptoUtils), "Crypto文件夹不存在，尝试创建文件夹。");
            }
            aesAlg = Aes.Create();
            AesInit(cryptoDirectory!);
            CoreManager.logger.Info(nameof(CryptoUtils), "Crypto工具集初始化完成。");
        }
        static void AesInit(string cryptoDirectory) {
            string AesCryptFilePath = Path.Combine(cryptoDirectory, AesCryptFileName);
            byte[] bytesBuf = [];
            if (File.Exists(AesCryptFilePath)) {
                bytesBuf = InnerAesDecryptBytesToBytes(FileUtils.ReadBytesFile(AesCryptFilePath));
                CoreManager.logger.Info(nameof(AesInit), "Aes外层解密完成。");

                aesAlg.Key = bytesBuf.Take(aesAlg.KeySize / 8).ToArray();
                aesAlg.IV = bytesBuf.TakeLast(bytesBuf.Length - aesAlg.KeySize / 8).ToArray();
                CoreManager.logger.Info(nameof(AesInit), "Aes内层密钥载入完成。");
            } else {
                bytesBuf = [.. aesAlg.Key, .. aesAlg.IV];
                FileUtils.WriteBytes(AesCryptFilePath, InnerAesEncryptBytesToBytes(bytesBuf));
                CoreManager.logger.Info(nameof(AesInit), "Aes外层加密且完成写入");
            }
        }
        public static byte[] AesEncryptStringToBytes(string plainText) {
            if(string.IsNullOrEmpty(plainText)) { return []; }

            byte[] encrypted;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            try {
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
                CoreManager.logger.Info(nameof(AesEncryptStringToBytes), "Aes内层加密完成。");
                return InnerAesEncryptBytesToBytes(encrypted);
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(AesEncryptStringToBytes), e);
                return [];   
            }            
        }
        public static string AesDecryptBytesToString(byte[] cipherBytes) {
            if(cipherBytes == null || cipherBytes.Length <= 0) { return string.Empty; }
            try {
                cipherBytes = InnerAesDecryptBytesToBytes(cipherBytes);
                string plainText = string.Empty;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new(cipherBytes)) {
                    using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using StreamReader srDecrypt = new(csDecrypt);
                    plainText = srDecrypt.ReadToEnd();
                }
                CoreManager.logger.Info(nameof(AesDecryptBytesToString), "Aes内层解密完成。");
                return plainText;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(AesDecryptBytesToString), e);
                return string.Empty;
            }
        }
        static byte[] InnerAesEncryptBytesToBytes(byte[] plainByte) {
            byte[] cipherBytes;
            
            using MemoryStream ms = new();
            using CryptoStream cryptoStream = new(
                ms, 
                aesAlg.CreateEncryptor(InnerAesCryptKey, InnerAesCryptIV), 
                CryptoStreamMode.Write);
            cryptoStream.Write(plainByte, 0, plainByte.Length);
            cryptoStream.FlushFinalBlock();
            cipherBytes = ms.ToArray();
            CoreManager.logger.Info(nameof(AesEncryptStringToBytes), "Aes外层加密完成。");
            return cipherBytes;
        }

        static byte[] InnerAesDecryptBytesToBytes(byte[] cipherBytes) {
            byte[] plainBytes;

            using MemoryStream ms = new();
            using CryptoStream cryptoStream = new(
                ms, 
                aesAlg.CreateDecryptor(InnerAesCryptKey, InnerAesCryptIV), 
                CryptoStreamMode.Write);
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
            cryptoStream.FlushFinalBlock();
            plainBytes = ms.ToArray();
            CoreManager.logger.Info(nameof(AesDecryptBytesToString), "Aes外层解密完成");
            return plainBytes;
        }
    }
}