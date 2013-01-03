using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logic;
using Moq;

namespace Logic_Test
{
    [TestFixture]
    class StateTests
    {
        [Test]
        public void EnterCalledOnActions()
        {
            var mock = new Mock<IStateAction>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            var state = new State("D", new IStateAction[] { mock.Object }, new Dictionary<string, int>());

            state.Enter();
            mock.Verify((m) => m.Enter(), Times.Once());
        }

        [Test]
        public void ExitCalledOnActions()
        {
            var mock = new Mock<IStateAction>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            var state = new State("D", new IStateAction[] { mock.Object }, new Dictionary<string, int>());

            state.Exit();
            mock.Verify((m) => m.Exit(), Times.Once());
        }


        [Test]
        public void TestClone()
        {
            var mock = new Mock<IStateAction>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            var state = new State("D", new IStateAction[] { mock.Object }, new Dictionary<string, int>());

            Assert.AreEqual(state, state.Clone());       
        }

        [Test]
        public void TestGetTransition()
        {

            var mock = new Mock<IStateAction>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            var state = new State("D", new IStateAction[] { mock.Object }, new Dictionary<string, int>() { {"event1", 2 } , {"event2", 3} });

            Assert.AreEqual(2, state.GetTransition("event1"));
            Assert.AreEqual(3, state.GetTransition("event2"));
            Assert.AreEqual(-1, state.GetTransition("hello"));
        }

        [Test]
        public void SendMessageCalledOnActions()
        {
            var mock = new Mock<IStateAction>();
            mock.Setup((m) => m.Clone()).Returns(mock.Object);
            var state = new State("D", new IStateAction[] { mock.Object }, new Dictionary<string, int>() { { "event1", 2 }, { "event2", 3 } });

            state.SendMessage("Hello");

            mock.Verify((s) => s.SendMessage("Hello", null, Utils.MessageOptions.DontRequireReceiver), Times.Once());

        }
    }
}
