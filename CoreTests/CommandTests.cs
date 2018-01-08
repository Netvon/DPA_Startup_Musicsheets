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

        class Dummy : IEquatable<Dummy>
        {
            public string Value { get; set; }

            public bool Equals(Dummy other)
            {
                return Value == other.Value;
            }
        }

        class DummyMemento : Memento<Dummy> { }

        [CommandBinding(Name = TestCommandName)]
        class TestCommand : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke<T>(Memento<T> memento)
            {
                if(memento is DummyMemento dm)
                {
                    dm.Save(new Dummy
                    {
                        Value = TestCommandName
                    });
                }
            }
        }

        [CommandBinding(Name = HalloCommandName)]
        class HalloCommand : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke<T>(Memento<T> memento)
            {
                if (memento is DummyMemento dm)
                {
                    dm.Save(new Dummy
                    {
                        Value = HalloCommandName
                    });
                }
            }
        }

        [CommandBinding(Name = Hallo2CommandName)]
        class Hallo2Command : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke<T>(Memento<T> memento)
            {
                if (memento is DummyMemento dm)
                {
                    dm.Save(new Dummy
                    {
                        Value = Hallo2CommandName
                    });
                }
            }
        }

        [CommandBinding(Name = Hallo3CommandName)]
        class Hallo3Command : ICommand
        {
            public bool CanInvoke() => true;

            public void Invoke<T>(Memento<T> memento)
            {
                if (memento is DummyMemento dm)
                {
                    dm.Save(new Dummy
                    {
                        Value = Hallo3CommandName
                    });
                }
            }
        }

        [TestMethod]
        public void TestCommandInvoke()
        {
            var memento = new DummyMemento();
            memento.Save(new Dummy());

            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");
            commands.AddBinding(HalloCommandName, "(AltLeft|AltRight) T");

            commands.Handle("AltRight", memento);
            commands.Handle("C", memento);

            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast(memento);
            Assert.AreEqual(TestCommandName, memento.Current.Value);

            Assert.IsFalse(commands.Handle("T", memento));
            Assert.AreEqual(TestCommandName, memento.Current.Value);

            commands.Handle("AltLeft", memento);
            commands.Handle("T", memento);
            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast(memento);
            Assert.AreEqual(HalloCommandName, memento.Current.Value);
        }

        [TestMethod]
        public void TestCommandMultipleInvoke()
        {
            var memento = new DummyMemento();
            memento.Save(new Dummy());

            var commands = new Commands(Assembly.GetAssembly(typeof(CommandTests)));
            commands.AddBinding(TestCommandName, "(AltLeft|AltRight) C");
            commands.AddBinding(HalloCommandName, "(AltLeft|AltRight) T");
            commands.AddBinding(Hallo2CommandName, "(AltLeft|AltRight) T 3");
            commands.AddBinding(Hallo3CommandName, "(AltLeft|AltRight) T 4");

            commands.Handle("AltRight", memento);
            Assert.IsFalse(commands.Handle("T", memento));
            commands.Handle("3", memento);

            Assert.IsTrue(commands.HasLastCommand);
            commands.InvokeLast(memento);

            Assert.AreEqual(Hallo2CommandName, memento.Current.Value);
        }
    }
}
