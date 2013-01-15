using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    public sealed class KeyEventArgs : EventArgs
    {
        public readonly char Character;
        public KeyEventArgs(char character) { this.Character = character; }
    }
}
