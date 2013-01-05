using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logic;
using Moq;
using Utils;

namespace Logic_Test
{
    [TestFixture]
    class FSMTests
    {
        [Test]
        public void EnterCalledInStateOnStart()
        {
            var mock = new Mock<IndexedState>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);

            var fsm = new FSM(new VariableCollection(), new IndexedState[] { mock.Object }, 0);

            fsm.Start();

            mock.Verify((s) => s.Enter(), Times.Once());
        }

        [Test]
        public void SendEventWorks()
        {
            var mock = new Mock<IndexedState>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            mock.Setup((m) => m.GetTransition("goto1")).Returns(1);
            
            var mock1 = new Mock<IndexedState>();
            mock1.Setup((m) => m.Clone()).Returns(mock1.Object);
            mock1.Setup((m) => m.GetTransition("goto0")).Returns(0);

            var fsm = new FSM(new VariableCollection(), new IndexedState[] { mock.Object , mock1.Object}, 0);


            fsm.SendEvent("goto1");

            mock.Verify((m) => m.Exit(), Times.Once());
            mock1.Verify((m) => m.Enter(), Times.Once());

            fsm.SendEvent("goto1");
            fsm.SendEvent("goto0");

            mock1.Verify((m) => m.Exit(), Times.Once());
        }

        [Test]
        public void GetVariableWorks()
        {
            var mock = new Mock<IndexedState>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);

            var variableCollection = new VariableCollection();
            variableCollection.AddVariable("test", 2);
            variableCollection.AddVariable("test2", "Hello");

            var fsm = new FSM(variableCollection, new IndexedState[] { mock.Object }, 0);

            Assert.AreEqual(2, fsm.GetVariable<int>("test"));
            Assert.AreEqual("hello", fsm.GetVariable<string>("ets"));
        }


        [Test]
        public void SetVariableWorks()
        {
            var mock = new Mock<IndexedState>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);

            var variableCollection = new VariableCollection();
            Variable<int> var1 = variableCollection.AddVariable("test", 2);
            Variable<string> var2 = variableCollection.AddVariable("test2", "Hello");

            var fsm = new FSM(variableCollection, new IndexedState[] { mock.Object }, 0);

            var1.Data = 10;
            Assert.AreEqual(10, fsm.GetVariable<int>("test").Data);

            var2.Data = "dance";
            Assert.AreEqual("dance", fsm.GetVariable<string>("ets").Data);
        }


        [Test]
        public void SendMessageCalledOnActiveState()
        {
            var mock = new Mock<IndexedState>();
            var fsm = new FSM(new VariableCollection(), new IndexedState[] { mock.Object }, 0);

            fsm.Start();
            fsm.SendMessage("Hello");

            mock.Verify((s) => s.SendMessage("Hello", null, Utils.MessageOptions.DontRequireReceiver), Times.Once());
        }

        [Test]
        public void ExitCalledInStateOnTransition()
        {
            var mock = new Mock<IndexedState>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);

            var fsm = new FSM(new VariableCollection(), new IndexedState[] { mock.Object }, 0);

            fsm.Start();
            mock.Verify((s) => s.Enter(), Times.Once());
        }

    }
}
