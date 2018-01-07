using Core.IO;
using Core.IO.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTests
{
    [TestClass]
    public class LyReaderTests
    {
        [TestMethod]
        public async Task TestLy()
        {
            var writerFactory = new SheetWriterFactory();
            var writerExt = writerFactory.GetAllSupportedExtension();

            var readerFactory = new SheetReaderFactory();
            var readerExt = readerFactory.GetAllSupportedExtension();

            var reader = readerFactory.GetReader("../../../DPA_Musicsheets/Files/Twee-emmertjes-water-halen.ly");
            //reader.SetFilePath("../../../DPA_Musicsheets/Files/Alle-eendjes-zwemmen-in-het-water.ly");

            reader.SetFilePath("../../../DPA_Musicsheets/Files/Twee-emmertjes-water-halen.ly");


            var resultA = await reader.ReadFromFileAsync();
            var resultB = await reader.ReadFromFileAsync();

            var writerA = writerFactory.GetWriter("hallo.ly");
            var strResultA = await writerA.WriteToString(resultA);
            var strResultB = await writerA.WriteToString(resultB);

            var nothing = string.Empty;
        }
    }
}
