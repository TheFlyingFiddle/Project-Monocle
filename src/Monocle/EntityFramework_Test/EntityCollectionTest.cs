using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.Game;
using Monocle.Core;
using Monocle.Utils;
using Monocle.Content.Serialization;
using System.IO;

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
            Entity e = null;
            collection.ObjectAdded += (x) => { e = (Entity)x; };
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

        [Test]
        public void ObjectAddedInvokedWhenAEntityInTheCollecionGainsANewChild()
        {
            var entity0 = new Entity(new VariableCollection());
            var entity1 = new Entity(new VariableCollection());
            this.collection.Add(entity0);

            var called = false;
            this.collection.ObjectAdded += x => called = x == entity1;
                      
            entity1.Parent = entity0;

            Assert.IsTrue(called);
        }

        [Test]
        public void AllChildrenAddedWhenEntityIsAdded()
        {
            var entity0 = new Entity(new VariableCollection());
            var entity1 = new Entity(new VariableCollection());
            var entity2 = new Entity(new VariableCollection());
            var entity3 = new Entity(new VariableCollection());

            entity1.Parent = entity0;
            entity2.Parent = entity0;
            entity3.Parent = entity1;

            collection.Add(entity0);

            Assert.IsTrue(collection.Contains(entity0));
            Assert.IsTrue(collection.Contains(entity1));
            Assert.IsTrue(collection.Contains(entity2));
            Assert.IsTrue(collection.Contains(entity3));
        }

        [Test]
        public void AllChildrenAreRemovedWhenEntityIsRemoved()
        {
            var entity0 = new Entity(new VariableCollection());
            var entity1 = new Entity(new VariableCollection());
            var entity2 = new Entity(new VariableCollection());
            var entity3 = new Entity(new VariableCollection());

            entity1.Parent = entity0;
            entity2.Parent = entity0;
            entity3.Parent = entity1;

            collection.Add(entity0);
            collection.Remove(entity0);

            Assert.IsFalse(collection.Contains(entity0));
            Assert.IsFalse(collection.Contains(entity1));
            Assert.IsFalse(collection.Contains(entity2));
            Assert.IsFalse(collection.Contains(entity3));
        }

        [Test]
        public void AllComponentsAreRemovedWhenEntityIsRemoved()
        {
            var entity0 = new Entity(new VariableCollection());
            var entity1 = new Entity(new VariableCollection());
            var entity2 = new Entity(new VariableCollection());
            var entity3 = new Entity(new VariableCollection());

            entity1.Parent = entity0;
            entity2.Parent = entity0;
            entity3.Parent = entity1;



            entity0.AddComponent<FakeComp>();
            entity1.AddComponent<FakeComp>();
            entity1.AddComponent<FakeComp>();
            entity2.AddComponent<FakeComp>();
            entity3.AddComponent<FakeComp>();

            int count = 0;
            collection.ObjectRemoved += x =>
                {
                    if (x is Component)
                        count++;
                };

            collection.Add(entity0);
            collection.Remove(entity0);

            Assert.AreEqual(5, count);
        }

        [Test]
        public void AllComponentsAreAddedWhenEntityIsAdded()
        {
            var entity0 = new Entity(new VariableCollection());

            entity0.AddComponent<FakeComp>();
            entity0.AddComponent<FakeBehaviour>();

            collection.Add(entity0);
        }

        [Test]
        public void CanSerializeSimpleCollection()
        {
            var entity0 = new Entity(new VariableCollection());
            entity0.Name = "Entity0";
            var entity1 = new Entity(new VariableCollection());
            entity1.Name = "Entity1";
            
            collection.Add(entity0);
            collection.Add(entity1);

            Stream stream = new MemoryStream();
            AssetWriter.WriteAsset(stream, collection, new TypeWriterFactory());
            stream.Position = 0;
            var collection2 = AssetReader.ReadAsset<EntityCollection>(stream, new TypeReaderFactory());

            Assert.NotNull(collection2.Find(x => x.Name == "Entity0"));
            Assert.NotNull(collection2.Find(x => x.Name == "Entity1"));
           
        }

        [Test]
        public void CanSerializeTreeCollection()
        {
            var entity0 = new Entity(new VariableCollection());
            var entity1 = new Entity(new VariableCollection());
            var entity2 = new Entity(new VariableCollection());
            var entity3 = new Entity(new VariableCollection());

            entity1.Parent = entity0;
            entity2.Parent = entity0;
            entity3.Parent = entity1;

            entity0.AddComponent<FakeComp>();
            entity1.AddComponent<FakeComp>();
            entity1.AddComponent<FakeComp>();
            entity2.AddComponent<FakeComp>();
            entity3.AddComponent<FakeComp>();

            collection.Add(entity0);


            Stream stream = new MemoryStream();
            AssetWriter.WriteAsset(stream, collection, new TypeWriterFactory());
            stream.Position = 0;
            var collection2 = AssetReader.ReadAsset<EntityCollection>(stream, new TypeReaderFactory());
        }
    }
}