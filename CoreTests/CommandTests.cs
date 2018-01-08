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

        [CommandBinding(Name = TestCommandName)]
        class TestCommand : ICommand
        {
            public void Invoke()
            {
                Console.WriteLine("Hallo!");
            }
        }

        [TestMethod]
        public void TestCommandInvoke()
        {
            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");

            commands.Handle("AltRight");
            Assert.IsTrue(commands.Handle("C"));
        }
    }
}
