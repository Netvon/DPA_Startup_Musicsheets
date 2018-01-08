using Core.Commands;
using Core.Editor;
using Core.Memento;
using Core.Models;
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
        public const string Hallo2CommandName = "hallo2";
        public const string Hallo3CommandName = "hallo3";

        //public static string Value { get; set; }

        class Dummy
        {
            public string Value { get; set; }
        }

        [CommandBinding(Name = TestCommandName)]
        class TestCommand : ICommand<Dummy>
        {
            public bool CanInvoke() => true;

            public void Invoke()
            {
                Value = TestCommandName;
            }
        }

        [CommandBinding(Name = HalloCommandName)]
        class HalloCommand : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke()
            {
                Value = HalloCommandName;
            }
        }

        [CommandBinding(Name = Hallo2CommandName)]
        class Hallo2Command : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke()
            {
                Value = Hallo2CommandName;
            }
        }

        [CommandBinding(Name = Hallo3CommandName)]
        class Hallo3Command : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke()
            {
                Value = Hallo3CommandName;
            }
        }

        [TestMethod]
        public void TestCommandInvoke()
        {
            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");
            commands.AddBinding(HalloCommandName, "(AltLeft|AltRight) T");

            commands.Handle("AltRight");
            commands.Handle("C");

            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast();
            Assert.AreEqual(TestCommandName, Value);

            Assert.IsFalse(commands.Handle("T"));
            Assert.AreEqual(TestCommandName, Value);

            commands.Handle("AltLeft");
            commands.Handle("T");
            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast();
            Assert.AreEqual(HalloCommandName, Value);
        }

        [TestMethod]
        public void TestCommandMultipleInvoke()
        {
            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");
            commands.AddBinding(HalloCommandName, "(AltLeft|AltRight) T");
            commands.AddBinding(Hallo2CommandName, "(AltLeft|AltRight) T 3");
            commands.AddBinding(Hallo3CommandName, "(AltLeft|AltRight) T 4");

            commands.Handle("AltRight");
            Assert.IsFalse(commands.Handle("T"));
            commands.Handle("3");

            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast();

            Assert.AreEqual(Hallo2CommandName, Value);
        }
    }
}
