using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EntityFramework
{
    class Program
    {
        public static void Main(string[] args)
        {
            var entityCollection = new EntityCollection(new EntityCreator());
            var system = new UpdateSystem();
            system.TrackEntityCollection(entityCollection);

            var entity = entityCollection.CreateEntity("John");
            entity.AddComponent<UpdatableComponent>();

            while (true)
            {
                Thread.Sleep(1000);
                system.Update();
            }          
        }
    }
}
