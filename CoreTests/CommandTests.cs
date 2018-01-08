using Core.Commands;
using Core.Editor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace CoreTests
{
    [TestClass]
    public class CommandTests
    {
        public const string TestCommandName = "test";
        public const string HalloCommandName = "hallo";

        public static string Value { get; set; }

        [CommandBinding(Name = TestCommandName)]
        class TestCommand : ICommand
        {
            public void Invoke()
            {
                Value = TestCommandName;
            }
        }

        [CommandBinding(Name = HalloCommandName)]
        class HalloCommand : ICommand
        {
            public void Invoke()
            {
                Value = HalloCommandName;
            }
        }

        [TestMethod]
        public void TestCommandInvoke()
        {
            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");
            commands.AddBinding(HalloCommandName, "(AltLeft|AltRight) T");

            commands.Handle("AltRight");
            Assert.IsTrue(commands.Handle("C"));
            Assert.AreEqual(TestCommandName, Value);

            Assert.IsFalse(commands.Handle("T"));
            Assert.AreEqual(TestCommandName, Value);

            commands.Handle("AltLeft");
            Assert.IsTrue(commands.Handle("T"));
            Assert.AreEqual(HalloCommandName, Value);
        }
    }
}
