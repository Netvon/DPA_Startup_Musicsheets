using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.IO;
using Core.Models;
using System.Threading.Tasks;

namespace CoreTests
{
    [SheetReader(".mid", "fake")]
    public class MockReader : ISheetReader
    {
        public Task<Sheet> ReadFromFileAsync()
        {
            throw new NotImplementedException();
        }

        public void SetFilePath(string path)
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
            var f = new SheetReaderFactory();
            var reader = f.GetReader("hallo.mid");

            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public void TestCustomTypes()
        {
            var asm = typeof(ReaderTests).Assembly;

            var f = new SheetReaderFactory(asm);
            var reader = f.GetReader("hallo.mid");

            Assert.IsTrue(reader is MockReader);
        }

        [TestMethod]
        public async Task TestFile()
        {
            var f = new SheetReaderFactory();
            var reader = f.GetReader("../../../DPA_Musicsheets/Files/Alle-eendjes-zwemmen-in-het-water.mid");
            await reader.ReadFromFileAsync();


            Assert.IsTrue(true);
        }
    }
}
