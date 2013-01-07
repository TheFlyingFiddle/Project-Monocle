using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.Logic;
using Monocle.Content.Serialization;

namespace Logic_Test
{
    [TestFixture]
    class FSMComponentTest
    {
        FSMComponent fsm;

        [SetUp]
        public void Setup()
        {
            fsm = new FSMComponent();
        }

        [Test]
        public void CanAddState()
        {
            Assert.NotNull(fsm.AddState("Test"));
        }

        [Test]
        public void DuplicateStatesThrowsExceptionOnAdding()
        {
            fsm.AddState("Test");
            Assert.Throws<ArgumentException>(() => fsm.AddState("Test"));
        }

        [Test]
        public void CanRemoveState()
        {
            fsm.AddState("Test");
            Assert.NotNull(fsm.GetState("Test"));
            fsm.RemoveState("Test");
            Assert.Throws<ArgumentException>(() => fsm.GetState("Test"));
        }

        [Test]
        public void CanGetState()
        {
            var state = fsm.AddState("Test");
            Assert.AreSame(state, fsm.GetState("Test"));
        }

        [Test]
        public void CanRenameState()
        {
            var state = fsm.AddState("Test");
            state.Name = "FrogMan";

            Assert.AreSame(state, fsm.GetState("FrogMan"));
        }

        [Test]
        public void RemoveAlsoRemovesTransitionsToTheRemovedState()
        {
            var state1 = fsm.AddState("Test");
            var state2 = fsm.AddState("Foo");

            state2.AddTransition("Bar", "Test");
            Assert.AreEqual("Test", state2.GetTransition("Bar"));
            fsm.RemoveState(state1.Name);
            Assert.AreEqual(string.Empty, state2.GetTransition("Bar"));
        }

        [Test]
        public void RenameAlsoRenamesTransitionsToTheRenamedState()
        {
            var state1 = fsm.AddState("Test");
            var state2 = fsm.AddState("Foo");

            state2.AddTransition("Bar", "Test");
            Assert.AreEqual("Test", state2.GetTransition("Bar"));
            state1.Name = "Dance";
            Assert.AreEqual("Dance", state2.GetTransition("Bar"));
        }

        [Test]
        public void CanCopyState()
        {
            var state1 = fsm.AddState("Foo");
            var state2 = state1.Copy();
            var state3 = state1.Copy();
            var state4 = state2.Copy();

            Assert.AreEqual("Foo-Copy", state2.Name);
            Assert.AreEqual("Foo-Copy 2", state3.Name);
            Assert.AreEqual("Foo-Copy-Copy", state4.Name);          
        }

        [Test]
        public void Integration_Test()
        {
            var open = fsm.AddState("Open");
            var closed = fsm.AddState("Closed");
            var opening = fsm.AddState("Opening");
            var closing = fsm.AddState("Closing");

            open.AddTransition("CLOSE", "Closing");
            closed.AddTransition("OPEN", "Opening");
            closing.AddTransition("FINISHED", "Closed");
            opening.AddTransition("FINISHED", "Open");
                        
            fsm.Start();

            fsm.SendEvent("CLOSE");
            fsm.SendEvent("FINISHED");
            fsm.SendEvent("OPEN");
            fsm.SendEvent("FINISHED");
        }

        [Test]
        public void CanSerializeAndDeserializeFSM()
        {
            fsm.AddState("Test");
            
            var memStream = new System.IO.MemoryStream();

            var writer = new BinaryWriter(memStream, new TypeWriterFactory());
            var reader = new BinaryReader(memStream, new TypeReaderFactory());

            writer.Write(fsm);

            memStream.Position = 0;

            var fsmDeserialized = reader.Read<FSMComponent>();

    
        }
    }
}