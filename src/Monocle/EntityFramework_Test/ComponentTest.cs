using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Monocle.EntityFramework;
using Monocle.Game;

namespace EntityFramework_Test
{
    [TestFixture]
    class ComponentTest
    {
        [Test]
        public void CanCopy()
        {
            var collection = new EntityCollection();
            var entity = collection.AddEntity();
            var entity1 = collection.AddEntity();
            var comp = entity.AddComponent<FakeComp>();
            var copy = comp.Copy(entity1);


            Assert.AreSame(comp.Owner, entity);
            Assert.AreSame(copy.Owner, entity1);
            Assert.NotNull(entity1.GetComponent<FakeComp>());



            entity.Destroy();
            entity1.Destroy();
            MonocleObject.LifeTimeManager.DestroyObjectsFlaggedForDestruction();
        }

    }
}
