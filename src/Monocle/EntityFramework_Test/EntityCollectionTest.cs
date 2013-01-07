using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.EntityFramework;
using Monocle.Game;

namespace EntityFramework_Test
{
    [TestFixture]
    class EntityCollectionTest
    {
        private EntityCollection collection;

        [SetUp]
        public void Setup()
        {
            collection = new EntityCollection();
        }

        [TearDown]
        public void TearDown()
        {
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
        }


        [Test]
        public void EntityCanBeAdded()
        {
            var entity = collection.AddEntity();
            Assert.NotNull(entity);

            entity.Destroy();
        }

        [Test]
        public void EntityCreatedInvokedWhenAnEntityIsCreated()
        {
            IEntity e = null;
            collection.EntityCreated += (x) => { e = x; };
            var entity = collection.AddEntity();

            Assert.AreSame(e, entity);

            entity.Destroy();
        }

        [Test]
        public void EntityDestroyedInvokedWhenAnEntityIsDestroyed()
        {
            var entity = collection.AddEntity();
            var called = false;
            collection.EntityDestroyed += (x) => { called = true; };
            entity.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
            
            Assert.IsTrue(called);
        }

        [Test]
        public void ComponentCreatedInvokedWhenAComponentIsCreated()
        {
            var entity = collection.AddEntity();
            var called = false;
            collection.ComponentCreated += (x) => { called = true; };
            entity.AddComponent<FakeComp>();
            Assert.IsTrue(called);

            entity.Destroy();
        }

        [Test]
        public void ComponentDestroyedInvokedWhenAComponentIsDestroyed()
        {
            var entity = collection.AddEntity();
            var called = false;
            collection.ComponentDestroyed += (x) => { called = true; };
            entity.AddComponent<FakeComp>();
            entity.GetComponent<FakeComp>().Destroy();


            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
            Assert.IsTrue(called);

            entity.Destroy();
        }

        [Test]
        public void FindCanFindEntities()
        {
            var entity = collection.AddEntity();
            entity.Name = "Daniel";

            var result = collection.Find((e) => e.Name == "Daniel");

            Assert.AreSame(entity, result);
        }
    }
}