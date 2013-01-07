using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using Monocle.EntityFramework;

namespace EntityFramework_Test
{
    [TestFixture]
    class BehaviourTest
    {
        private bool corutineCalled = false;

        [Test]
        public void TestCoroutines()
        {
            var collection = new EntityCollection();
            var entity = collection.AddEntity();
            var comp = entity.AddComponent<FakeBehaviour>();

            comp.StartCoroutine(Coroutine());
            comp.RunCoroutines();

            Assert.IsTrue(corutineCalled);
            
        }

        private IEnumerable Coroutine()
        {
            corutineCalled = true;
            yield return 0;
        }


    }

    class FakeBehaviour : Behaviour
    {
        protected override Component Clone()
        {
            return new FakeBehaviour();
        }
    }
}
