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
            var reader = new LilipondMusicReader();
            reader.SetFilePath("../../../DPA_Musicsheets/Files/Alle-eendjes-zwemmen-in-het-water.ly");


            var result = await reader.ReadFromFileAsync();
        }
    }
}
