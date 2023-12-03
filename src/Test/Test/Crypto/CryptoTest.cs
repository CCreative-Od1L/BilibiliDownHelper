using Xunit.Abstractions;
using System.Security.Cryptography;
using Core.Utils;
using System.Text;

namespace Core.Test {
    public class CryptoTest {
        readonly ITestOutputHelper output;

        public CryptoTest(ITestOutputHelper testOutputHelper) {
            output = testOutputHelper;
        }

        readonly string PublicKeyPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Root, @"publicKey.txt");
        readonly string PrivateKeyPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Root, @"privateKey.txt");
        // [Fact]
        public void GenKeyFunc() {
            var rsa = RSA.Create();
            FileUtils.WriteText(PublicKeyPath, rsa.ToXmlString(false));
            FileUtils.WriteText(PrivateKeyPath, rsa.ToXmlString(true));     
        }
        // [Fact]
        public void RSAEncrypt() {
            string content = "Hello World";
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(FileUtils.ReadTextFile(PublicKeyPath));
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            string encryptedContent = Convert.ToBase64String(encryptedData);
            output.WriteLine(encryptedContent);

            RSADecrypt(encryptedContent);
        }
        public void RSADecrypt(string content) {
            string privateKey = "<RSAKeyValue><Modulus>2wPwxX0COZBTvDboZRXMvsskIS6LQA9+LemRcpt9nwlCEPBYGoEQzPk6cEiYLk/tZjCIqSzu7gy3JrnZtiXiLTiRzpOv7+vZKUZOrgwtdqGGQOHZJgHqLm1y+XdsD1JcD1cXKmvHToW+xyVRh7jhJNBT1v1pv5smB+UWfBpHs6re9WvduaOBGA6CY9iQ3KQ0jR9VpAPrygAcOFyZP3q22hZ/vN7webYEshb4iraG3tqdu5FfuvVopT8DE2QQ4IrS7HXKC5RCw/VH2+AdK+wlkdx/mdW8G2+twADFKpO6XPFI7+V1JM0qAVWhgm9TKOfjyoQVFQAJmj4BdIAbW+2jhQ==</Modulus><Exponent>AQAB</Exponent><P>5S7fh+WL3CKmQSPGW24nvHpzN0zIkO2zbUB9XFLru6LlPlGV6TFtprlGy8EwSndBYwQattVudD++VciohP+tSWiEBbToByQ9326x6feZ2PkAgkRRkjUGYF/vFSvk2rEND66+anNxUaDyzg+PmB50xXs5h9sAxNCzrGb1Pg86z4s=</P><Q>9KR+/lWa2tfJX1vAbwVfrg8FMZX0+xdZPrf89JvmVw7BuSiMN7ZKBKmN5o9LZ70o1QrLrxp/ofrfFuycUpZgDH6KWSjt/PmwxoBmogi7AZAPTFj1hJwME2Wn2OtdCNSn42s8GMbkdNFis6o+G0FXOz/x0oADUN7gitxq2UB1uy8=</Q><DP>3GUnMVjeFuR7XUk6B9L3A5n+EsAUYMs2MWpDI/XaHKeaLUPoFkkiWaLzIh62geYNS1s3FXxKrCBgub4t0TjPPDj2PUarm7KOjb+1+HnTyYQrqRpqF2BGsYeQM5vyiRyaouo408YQw58z4FW2kWM4iHDn8m35X0wUfUbFsNiUheE=</DP><DQ>QFj8Jj2yC5nyl6h0hz8smYXN4esFkv3jNYIDgPt4rLFu9xrZNtY1xeArONe8B79TfCXoyVf49kwcXdVCkN6IyQCHt0fywTWT78JaPIh7V7/ipjxH5+d7raZxbbcKs4Xr2v+bwRCfirKAea1vxI99OFJrcujE9oKJIT00xYwU63U=</DQ><InverseQ>SPJEfVJbAlMQKxfWbj3hGml4Paf7EQe9f1JCGqXqwxKRkJhpPrgd10Y2ULtpe+tqo1pOuQm9bnSN0WIHBDg1oYstFL9XjOb2BSW/XwDuICGGKnMoxvAUNjN+khFMot4IvVEdxP2ts26r8DBXgxFRKR5gDdGzdM0oTF1gu2c1b+Q=</InverseQ><D>2CGhXyKoKWpWV+zPlcHXlYkelGg8HvGrEV/nmvP7GhCm09R6/VzU2ZIxq1DYbQuPOUM3+ctP1jdSHrNVN71W4E1tBjEUdG2IloS9bGC0FNf9htXZ1voYwcxco/bH+q7KDpFhWQmXW9P8msYnGQrHk+7KbeUSkHVpKG1YrQu6Kn/ngt0rtLUtN+HtBcX1SRGSAdye03TBWbjq0wBZYM+rgAPgsgtlw6r/M4oxh5E7AR9+FtF7RHJrULM5kpS5ktsXn1eZm0CcznbJIMMobZ4V/A8TsYiLcUFiz83W40fl/e2av0nlhf5fj2UN+I4XAv20Kxbi3lrXMIJx99yiYfmBGQ==</D></RSAKeyValue>";
            string decryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new()) {
                rsa.FromXmlString(privateKey);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(content), false);
                decryptedContent = Encoding.UTF8.GetString(decryptedData);
            }
            output.WriteLine(decryptedContent);
        }
        // [Fact]
        public void InnerRsa() {
            byte[] innerRsaKey = [
                0x31, 0x0d, 0xba, 0x6a, 0x56, 0xf1, 0xa3, 0x59, 0xe5, 0xd4, 0xdb, 0x26, 0xc7, 0x1f, 0x04, 0xa2,
                0x95, 0x6a, 0x52, 0xef, 0x76, 0x91, 0xd9, 0x4b, 0x07, 0xbc, 0x0c, 0x6a, 0x2b, 0x38, 0x81, 0x50
            ];
            byte[] innerRsaIV = [
                0x11, 0x97, 0xac, 0x54, 0xa8, 0x3d, 0x56, 0x73, 0xdf, 0x8a, 0x83, 0x60, 0xd0, 0x93, 0xc2, 0x02
            ];

            byte[] content = Encoding.UTF8.GetBytes("HelloWorld");

            byte[] cipherBytes;
            using (Aes aes = Aes.Create()) {
                using MemoryStream ms = new();
                using CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(innerRsaKey, innerRsaIV), CryptoStreamMode.Write);
                cryptoStream.Write(content, 0, content.Length);
                cryptoStream.FlushFinalBlock();
                cipherBytes = ms.ToArray();
            }

            byte[] plainBytes;
            using (Aes aes = Aes.Create()) {
                using MemoryStream ms = new();
                using CryptoStream cryptoStream = new(ms, aes.CreateDecryptor(innerRsaKey, innerRsaIV), CryptoStreamMode.Write);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                plainBytes = ms.ToArray();
            }
            
            Assert.Equal(plainBytes, content);
        }
    }
}