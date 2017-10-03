using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Models;

namespace CoreTests
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void TestLengthWhenDotted()
        {
            var note = new PitchNote()
            {
                BaseLength = 1,
                Dotts = 0
            };

            Assert.AreEqual(1,   note.Length);

            note.Dotts++;
            Assert.AreEqual(1.5, note.Length);
            note.Dotts++;
            Assert.AreEqual(1.75, note.Length);
            note.Dotts++;
            Assert.AreEqual(1.875, note.Length);
            note.Dotts++;
            Assert.AreEqual(1.9375, note.Length);
            note.Dotts++;
            Assert.AreEqual(1.96875, note.Length);
            note.Dotts++;
            Assert.AreEqual(1.984375, note.Length);
        }

        [TestMethod]
        public void TestBaseLengthMustBeEven()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var note = new PitchNote()
                {
                    BaseLength = 3,
                };
            });
        }
    }
}
