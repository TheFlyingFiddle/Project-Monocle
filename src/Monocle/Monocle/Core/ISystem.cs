using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;

namespace Monocle.Core
{
    public interface ISystem
    {
        bool Enabled { get; set; }
        void Exceute(Time time);
    }

    public interface IRenderSystem
    {

    }
}
