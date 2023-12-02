using Xunit.Abstractions;
using System.Security.Cryptography;

namespace Core.Test {
    public class CryptoTest {
        readonly ITestOutputHelper output;

        public CryptoTest(ITestOutputHelper testOutputHelper) {
            output = testOutputHelper;
        }

        // [Fact]
        public void CryptoMainFunc() {
            using Aes myAes = Aes.Create();
            byte[] encrypted = AesEncryptStringToBytes("Hello world", myAes.Key, myAes.IV);
            string roundtrip = AesDecryptBytesToString(encrypted, myAes.Key, myAes.IV);

            Assert.Equal("Hello world", roundtrip);
            output.WriteLine(roundtrip);
        }

        private string AesDecryptBytesToString(byte[] encrypted, byte[] key, byte[] iV)
        {
            throw new NotImplementedException();
        }

        private byte[] AesEncryptStringToBytes(string v, byte[] key, byte[] iV)
        {
            throw new NotImplementedException();
        }
    }
}