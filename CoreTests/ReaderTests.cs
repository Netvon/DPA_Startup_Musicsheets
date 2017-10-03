using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.IO;
using Core.Models;
using System.Threading.Tasks;

namespace CoreTests
{
    [MusicReader(".mid", "fake")]
    public class MockReader : IMusicReader
    {
        public Task<Sheet> ReadFromFileAsync(string filePath, string name = null)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ReaderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var f = new MusicReaderFactory();
            var reader = f.GetReader("hallo.mid");

            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public void TestCustomTypes()
        {
            var asm = typeof(ReaderTests).Assembly;

            var f = new MusicReaderFactory(asm);
            var reader = f.GetReader("hallo.mid");

            Assert.IsTrue(reader is MockReader);
        }
    }
}
