using NUnit.Framework;
using Compression.NBWT;
using Compression.RLE;

namespace UnitTesting.RLE {
    [TestFixture, Category("ByteChangeEncoding")]
    public class ByteChangeEncodingTests {
        [Test]
        public void AddsEntries_AAABAAABAA_as_ABABA_1001100110() {
            byte[] input = BWT.StringToByteArray("AAABAAABAA");
            byte[] expected = new byte[7];
            byte[] a = BWT.StringToByteArray("ABABA");
            byte[] b = {153, 128};
            a.CopyTo(expected, 0);
            b.CopyTo(expected, a.Length);

            byte[] actual = ByteChangeEncoder.EncodeBytes(input).ToBytes();
            
            Assert.AreEqual(expected, actual);
        }
    }
}