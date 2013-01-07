using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.Game;
using Monocle.Utils;
using Monocle.Core;

namespace EntityFramework_Test
{
    [TestFixture]
    public class EntityTest
    {
        IEntityCollection entityCollection;
        Entity entity;

        [SetUp]
        public void Setup()
        {
            entityCollection = new EntityCollection();
            entity = (Entity)entityCollection.AddEntity();
        }

        [TearDown]
        public void TearDown()
        {
            entity.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
        }

        [Test]
        public void CanSetAndGetName()
        {
            entity.Name = "Daniel";
            Assert.AreEqual("Daniel", entity.Name);
        }

        [Test]
        public void CanAddAndRemoveTags()
        {
            Assert.IsFalse(entity.HasTag("Player"));
            
            entity.AddTag("Player");
            Assert.IsTrue(entity.HasTag("Player"));
            
            entity.RemoveTag("Player");
            Assert.IsFalse(entity.HasTag("Player"));          
        }

        [Test]
        public void CanAddComponent()
        {
            var comp = entity.AddComponent<FakeComp>();
            Assert.NotNull(comp);
        }

        [Test]
        public void CanAddMutipleComponentsOfSameType()
        {
            var comp = entity.AddComponent<FakeComp>();
            var comp1 = entity.AddComponent<FakeComp>();
            
            Assert.NotNull(comp1);
            Assert.AreNotSame(comp, comp1);
        }

        [Test]
        public void CanGetComponent()
        {
            var comp = entity.AddComponent<FakeComp>();
            Assert.AreSame(comp, entity.GetComponent<FakeComp>());
        }

        [Test]
        public void CanGetComponentThroughInterface()
        {
            var comp = entity.AddComponent<FakeComp>();
            Assert.AreSame(comp, entity.GetComponent<IFake>());
        }

        [Test]
        public void GetComponentThrowsExceptionIfItFails()
        {
            Assert.Throws<ComponentMissingException>(() => entity.GetComponent<FakeComp>());
        }

        [Test]
        public void CanGetComponents()
        {
            entity.AddComponent<FakeComp>();
            entity.AddComponent<FakeComp>();

            var components = entity.GetComponents<FakeComp>().GetEnumerator();

            components.MoveNext();
            var fake1 = components.Current;
            components.MoveNext();

            Assert.AreNotSame(fake1, components.Current);
        }

        [Test]
        public void GetComponetsOnlyReturnsComponentsOfTheSpecifiedType()
        {
            entity.AddComponent<FakeComp>();
            entity.AddComponent<FakeComp>();
            entity.AddComponent<FakeComp1>();

            foreach (var component in entity.GetComponents<FakeComp>())
            {
                Assert.IsInstanceOf<FakeComp>(component);
            }
        }

        [Test]
        public void CanTryGetComponent()
        {
            var comp = entity.AddComponent<FakeComp>();
            FakeComp result;
            if (entity.TryGetComponent<FakeComp>(out result))
                Assert.AreSame(comp, result);
            else
                Assert.Fail();
        }

        [Test]
        public void TryGetComponentFailsIfComponentNotFound()
        {
            FakeComp result;
            if (entity.TryGetComponent<FakeComp>(out result))
                Assert.Fail();
        }

        [Test]
        public void RemoveComponentActuallyRemovesAComponent()
        {
            var comp = entity.AddComponent<FakeComp>();
            entity.RemoveComponent(comp);

            Assert.Throws<ComponentMissingException>(() => entity.GetComponent<FakeComp>());

            //Cleanup;
            comp.Destroy();
        }

        [Test]
        public void CanAddVariable()
        {
            Variable<string> var = entity.AddVar<string>("Test", "Foo");
            Assert.NotNull(var);
        }

        [Test]
        public void CantAddDuplicateVariable()
        {
            entity.AddVar<string>("Test", "Foo");
            Assert.Throws<ArgumentException>(() => entity.AddVar<string>("Test", "Bur"));
        }

        [Test]
        public void CanGetVariable()
        {
            Variable<string> var = entity.AddVar<string>("Test", "Foo");
            Assert.AreSame(var, entity.GetVar<string>("Test"));
        }
        

        [Test]
        public void GetVarThrowsExceptionOnInvalidType()
        {
            Variable<string> var = entity.AddVar<string>("Test", "Foo");

            Assert.Throws<InvalidCastException>(() => entity.GetVar<int>("Test"));
        }

        [Test]
        public void GetVarThrowsExceptionOnNonExitingVariable()
        {
            Assert.Throws<ArgumentException>(() => entity.GetVar<string>("Test"));
        }

        [Test]
        public void TryGetVarCanGetAVariable()
        {
            Variable<string> var = entity.AddVar<string>("Test", "Foo");
            Variable<string> result;
            if (entity.TryGetVar<string>(var.Name, out result))
                Assert.AreSame(var, result);
            else
                Assert.Fail();
        }

        [Test]
        public void TryGetVarCantGetInvalidVariable()
        {
            entity.AddVar<string>("Test","Foo");

            Variable<string> stringVar;
            Variable<int> intVar;

            if (entity.TryGetVar<int>("Test", out intVar) ||
                entity.TryGetVar<string>("Foo", out stringVar))
                Assert.Fail();
        }

        [Test]
        public void CanRemoveVariable()
        {
            entity.AddVar<string>("Test", "Foo");
            entity.RemoveVar("Test");

            Assert.Throws<ArgumentException>(() => entity.GetVar<string>("Test")); 
        }

        [Test]
        public void CanClone()
        {
            var clone = (Entity)entity.Clone();

            //Note No entity or MonocleObject for that matter are EVER equal.
            //So this is a very hard thing to test. IDK how to do it without going through alot 
            //Of anoing trouble.
            Assert.NotNull(clone);

            //Cleanup.
            clone.Destroy();
        }

        [Test]
        public void CanParent()
        {
            var entity1 = entityCollection.AddEntity();
            entity.Parent = entity1;
            Assert.AreSame(entity.Parent, entity1);
        }

        [Test]
        public void UnParentingRemovesTheEntityFromTheParentsChildren()
        {
            var entity1 = entityCollection.AddEntity();
            entity.Parent = entity1;

            entity.Parent = null;

            Assert.IsFalse(entity1.Children.Contains(entity));
        }

        [Test]
        public void ParentingAddsEntityAsAChildToTheParent()
        {
            var entity1 = entityCollection.AddEntity();
            entity.Parent = entity1;
            Assert.IsTrue(entity1.Children.Contains(entity));
        }

        [Test]
        public void CanDestroy()
        {
            entity.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();

            Assert.IsTrue(entity == null);
        }

        [Test]
        public void DestroyAlsoDestroysAllChildren()
        {
            var entity1 = entityCollection.AddEntity();
            entity.Parent = entity1;

            entity1.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();

            Assert.IsTrue(entity == null);
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

    class FakeComp1 : Component
    {
        protected override Component Clone()
        {
            return new FakeComp1();
        }
    }
}
