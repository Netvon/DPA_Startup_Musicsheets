﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.IO;
using Core.Models;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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

        [TestMethod]
        public void TestZip()
        {
            

            // https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=netframework-4.7.1
            using (var zipToOpen = new FileStream("../../../DPA_Musicsheets/Files/Tant_qu_il_y_aura_des_toiles.mxl", FileMode.Open))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    var entry = archive.Entries.First(e => {
                        return e.FullName.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase)
                            && !e.FullName.StartsWith("META-INF", StringComparison.InvariantCultureIgnoreCase);
                    });

                    using(var reader = XmlReader.Create(entry.Open(), new XmlReaderSettings() { Async = true, DtdProcessing = DtdProcessing.Parse }))
                    {
                        var doc = XDocument.Load(reader);
                        var index = doc.DocumentType.PublicId.IndexOf("MusicXML 3.0", StringComparison.InvariantCultureIgnoreCase);
                        if (index == -1)
                            return;

                        var parts = doc.Root.Descendants("part");

                        foreach (var part in parts)
                        {
                            var meassures = part.Descendants("measure");

                            foreach (var meassure in meassures)
                            {
                                var attributes = part.Descendants("attributes").SelectMany(x => x.Elements());
                                var notes = part.Descendants("note");
                                var n = 'n';
                            }

                            var h2 = 'h';
                        }

                        var h = 'g';
                    }
                }
            }
        }
    }
}
