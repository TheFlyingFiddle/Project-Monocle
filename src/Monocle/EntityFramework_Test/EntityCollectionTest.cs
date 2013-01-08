using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.Game;
using Monocle.Core;
using Monocle.Utils;

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
            var entity = new Entity(new VariableCollection());
            collection.Add(entity);

            Assert.NotNull(collection.Find((x) => true));
            entity.Destroy();
        }

        [Test]
        public void ObjectAddedInvokedWhenAnEntityIsAdded()
        {
            IEntity e = null;
            collection.ObjectAdded += (x) => { e = (IEntity)x; };
            var entity = new Entity(new VariableCollection());
            collection.Add(entity);

            Assert.AreSame(e, entity);

            entity.Destroy();
        }

        [Test]
        public void ObjectRemovedInvokedWhenAnEntityIsDestroyed()
        {
            var entity = new Entity(new VariableCollection());
            this.collection.Add(entity);
            var called = false;
            collection.ObjectRemoved += (x) => { called = true; };
            entity.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
            
            Assert.IsTrue(called);
        }

        [Test]
        public void ComponentCreatedInvokedWhenAComponentIsCreated()
        {
            var entity = new Entity(new VariableCollection());
            this.collection.Add(entity);
            var called = false;
            collection.ObjectAdded += (x) => { called = true; };
            entity.AddComponent<FakeComp>();
            Assert.IsTrue(called);

            entity.Destroy();
        }

        [Test]
        public void ComponentDestroyedInvokedWhenAComponentIsDestroyed()
        {
            var entity = new Entity(new VariableCollection());
            this.collection.Add(entity);
            var called = false;
            collection.ObjectRemoved += (x) => { called = true; };
            entity.AddComponent<FakeComp>();
            entity.GetComponent<FakeComp>().Destroy();


            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
            Assert.IsTrue(called);

            entity.Destroy();
        }

        [Test]
        public void FindCanFindEntities()
        {
            var entity = new Entity(new VariableCollection());
            this.collection.Add(entity);
            entity.Name = "Daniel";

            var result = collection.Find((e) => e.Name == "Daniel");

            Assert.AreSame(entity, result);
        }
    }
}