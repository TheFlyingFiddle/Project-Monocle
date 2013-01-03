using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EntityFramework;
using Moq;

namespace EntityFramework_Test
{
    [TestFixture]
    public class EntityTest
    {

        [Test]
        public void CanGetName()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            Assert.AreEqual("Hello", entity.Name);
        }

        [Test]
        public void HasTagWorks()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            entity.AddTag("Tag1");
            entity.AddTag("Tag2");

            Assert.IsTrue(entity.HasTag("Tag1"));
            Assert.IsTrue(entity.HasTag("Tag2"));
            Assert.IsFalse(entity.HasTag("Tag3"));
        }

        [Test]
        public void GetComponentRetrivesComponent()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fake = entity.AddComponent<FakeComp>();

            Assert.AreSame(fake, entity.GetComponent<Component>());
        }

        [Test]
        public void GetComponentCanRetriveComponentFromInterface()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fakeComp = entity.AddComponent<FakeComp>();
            
            Assert.AreSame(fakeComp, entity.GetComponent<IFake>());       
        }

        [Test]
        public void TryGetComponentRetrivesComponent()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fake = entity.AddComponent<FakeComp>();

            Component result;
            entity.TryGetComponent<Component>(out result);

            Assert.AreSame(fake, result);
        }


        [Test]
        public void TryGetComponentFailsIfThereIsNoComponentToRetrive()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            Component result;
            Assert.IsFalse(entity.TryGetComponent<Component>(out result));
            Assert.IsNull(result);
        }

        [Test]
        public void AddComponentDoesAddAComponent()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            Assert.Throws<ComponentMissingException>(() => entity.GetComponent<Component>());

            var fakeComp = entity.AddComponent<FakeComp>();
            Assert.AreSame(fakeComp, entity.GetComponent<Component>());
        }

        [Test]
        public void AddComponentNotifiesEntityCollection()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fakeComp = entity.AddComponent<FakeComp>();

            mock.Verify((m) => m.AddedComponent(fakeComp), Times.Once());
        }

        [Test]
        public void DeleteComponentRemovesComponent()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fakeComp = entity.AddComponent<FakeComp>();
            Assert.AreSame(fakeComp, entity.GetComponent<Component>());
            entity.DestroyComponent(fakeComp);
            Assert.Throws<ComponentMissingException>(() => entity.GetComponent<Component>());
        }

        [Test]
        public void DeleteComponentNotifiesEntityCollection()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            var fakeComp = entity.AddComponent<FakeComp>();
            entity.DestroyComponent(fakeComp);

            mock.Verify((m) => m.RemovedComponent(fakeComp), Times.Once());
        }

        [Test]
        public void AddVarAddsAVariable()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            var expected = "test";
            entity.AddVar("string_var", expected);
            entity.AddVar("int_var", 2);

            Assert.AreSame(expected, entity.GetVar<string>("string_var"));
            Assert.AreEqual(2, entity.GetVar<int>("int_var"));
        }

        [Test]
        public void CannotAddSameVariableTwice()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            entity.AddVar("string_var", "test");
            
            
            Assert.Throws<ArgumentException>(() => entity.AddVar("string_var", "test2"));
        }

        [Test]
        public void RemoveVarRemovesAVariable()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            entity.AddVar("string_var", "test");

            Assert.IsTrue(entity.RemoveVar("string_var"));
            Assert.Throws<ArgumentException>(() => entity.GetVar<string>("string_var"));
        }



        [Test]
        public void GetVarRetrivesVariable()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
          
            var expected = "test";
            entity.AddVar("string_var",expected);
            entity.AddVar("int_var", 2);

            Assert.AreSame(expected, entity.GetVar<string>("string_var"));
            Assert.AreEqual(2, entity.GetVar<int>("int_var"));
        }

        [Test]
        public void ExcpetionsAreThrownIfGetVarUsesInvalidArguments()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            entity.AddVar("int_var", 2);

            Assert.Throws<InvalidCastException>(() => entity.GetVar<string>("int_var"));
            Assert.Throws<ArgumentException>(() => entity.GetVar<string>("string_var"));
        }


        [Test]
        public void TryGetVarRetrivesVariable()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            var expected = "test";
            entity.AddVar("string_var", expected);
            entity.AddVar("int_var", 2);

            int resultInt;
            string resultString;

            entity.TryGetVar<int>("int_var", out resultInt);
            entity.TryGetVar<string>("string_var", out resultString);

            Assert.AreEqual(2, resultInt);
            Assert.AreEqual(expected, resultString);
        }

        [Test]
        public void TryGetComponentFailsIfThereIsNoVariableToRetriveOrInvalidVariableType()
        {
             
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            entity.AddVar("int_var", 2);
             
            string result;

            Assert.IsFalse(entity.TryGetVar<string>("string_var", out result));
            Assert.IsNull(result);
            Assert.IsFalse(entity.TryGetVar<string>("int_var", out result));
            Assert.IsNull(result);
        }

        [Test]
        public void SetVarActuallySetsAVariable()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            entity.AddVar("int_var", 2);

            entity.SetVar<int>("int_var", 10);
            Assert.AreEqual(10, entity.GetVar<int>("int_var"));
        }

        [Test]
        public void SetVarThrowsExceptionIfInvalidParameters()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);
            entity.AddVar("int_var", 2);

            Assert.Throws<InvalidCastException>(() => entity.SetVar<string>("int_var", "Hej"));
            Assert.Throws<ArgumentException>(() => entity.SetVar<string>("string_var", "Hej"));
        }

        [Test]
        public void AddTagAddsATag()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            Assert.IsFalse(entity.HasTag("tag1"));
            entity.AddTag("tag1");
            Assert.IsTrue(entity.HasTag("tag1"));
        }

        [Test]
        public void RemoveTagRemovesATag()
        {
            var mock = new Mock<InternalEntityCollection>();
            var entity = new Entity("Hello", mock.Object);

            entity.AddTag("tag1");
            entity.AddTag("tag1");
            Assert.IsTrue(entity.HasTag("tag1"));
            entity.RemoveTag("tag1");
            Assert.IsFalse(entity.HasTag("tag1"));
        }

    }

    interface IFake { }

    class FakeComp : Component, IFake
    {
        protected override Component Clone()
        {
            return new FakeComp();
        }
    }
}
