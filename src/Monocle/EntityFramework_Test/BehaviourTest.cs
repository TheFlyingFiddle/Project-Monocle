using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using Monocle.Core;
using Monocle.Utils;

namespace EntityFramework_Test
{
    [TestFixture]
    class BehaviourTest
    {
        private bool corutineCalled = false;

        [Test]
        public void TestCoroutines()
        {
            var entity = new Entity(new VariableCollection());
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
