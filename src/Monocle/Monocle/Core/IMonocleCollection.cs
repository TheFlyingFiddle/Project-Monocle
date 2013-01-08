using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Game;

namespace Monocle.Core
{
    public interface IMonocleCollection
    {
        event Action<MonocleObject> ObjectAdded;
        event Action<MonocleObject> ObjectRemoved;
    }
}
