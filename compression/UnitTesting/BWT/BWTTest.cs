using Compression;
using Compression.BWT;
using NUnit.Framework;

namespace UnitTesting.BWT_Test {
    [TestFixture]
    [Category("BurrowsWheelerTransform")]
    public class BWTTest {
        [Test]
        public void ByteArrayToStringFromFile3() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            var input_file = new DataFile(path);
        }

        [Test]
        public void InverseTransformFromText1() {
            var expected =
                "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            var input =
                "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            var bwt = new BurrowsWheelerTransform();

            var inArr = ByteMethods.StringToByteArray(input);
            var output = bwt.InverseTransform(inArr);
            var actual = ByteMethods.ByteArrayToString(output);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InverseTransformReturnsBANANAFromBNNAAA() {
            byte[] expected =
                {(byte) '^', (byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte) '|'};
            byte[] input =
                {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            var bwt = new BurrowsWheelerTransform();

            var result = bwt.InverseTransform(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void InverseTransformReturnsFromFemFladeFloedeboller() {
            var expected = "^fem flade flødeboller på et fladt flødebolle fad|";
            var input = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            var bwt = new BurrowsWheelerTransform();

            var inArr = ByteMethods.StringToByteArray(input);
            var output = bwt.InverseTransform(inArr);
            var actual = ByteMethods.ByteArrayToString(output);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TransformFromText1() {
            var input =
                "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            var expected =
                "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            var bwt = new BurrowsWheelerTransform();

            var inArr = ByteMethods.StringToByteArray(input);
            var output = bwt.Transform(inArr);
            var actual = ByteMethods.ByteArrayToString(output);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TransformReturnsBNNAAAFromBANANA() {
            byte[] input =
                {(byte) '^', (byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte) '|'};
            byte[] expected =
                {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            var bwt = new BurrowsWheelerTransform();

            var result = bwt.Transform(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TransformReturnsFromFemFladeFloedeboller() {
            var input = "^fem flade flødeboller på et fladt flødebolle fad|";
            var expected = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            var bwt = new BurrowsWheelerTransform();

            var inArr = ByteMethods.StringToByteArray(input);
            var output = bwt.Transform(inArr);
            var actual = ByteMethods.ByteArrayToString(output);

            Assert.AreEqual(expected, actual);
        }
    }
}