using NUnit.Framework;
using Compression;
using Compression.BWT;

namespace UnitTesting.BWT_Test {
    [TestFixture, Category("BurrowsWheelerTransform")]
    public class BWTTest {
        [Test]
        public void TransformReturnsBNNAAAFromBANANA() {
            byte[] input = {(byte)'^',(byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte)'|'};
            byte[] expected = {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] result = bwt.Transform(input);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void InverseTransformReturnsBANANAFromBNNAAA() {
            byte[] expected = {(byte)'^',(byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte)'|'};
            byte[] input = {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] result = bwt.InverseTransform(input);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TransformReturnsFromFemFladeFloedeboller() {
            string input = "^fem flade flødeboller på et fladt flødebolle fad|";
            string expected = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] inArr = ByteMethods.StringToByteArray(input);
            byte[] output = bwt.Transform(inArr);
            string actual = ByteMethods.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void InverseTransformReturnsFromFemFladeFloedeboller() {
            string expected = "^fem flade flødeboller på et fladt flødebolle fad|";
            string input = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] inArr = ByteMethods.StringToByteArray(input);
            byte[] output = bwt.InverseTransform(inArr);
            string actual = ByteMethods.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TransformFromText1() {
            string input = "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            string expected = "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] inArr = ByteMethods.StringToByteArray(input);
            byte[] output = bwt.Transform(inArr);
            string actual = ByteMethods.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InverseTransformFromText1() {
            string expected = "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            string input = "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            BurrowsWheelerTransform bwt = new BurrowsWheelerTransform();
            
            byte[] inArr = ByteMethods.StringToByteArray(input);
            byte[] output = bwt.InverseTransform(inArr);
            string actual = ByteMethods.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToStringFromFile3() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile input_file = new DataFile(path);
            
        }
    }
}