using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    public class TextChangedEventArgs : EventArgs
    {
        public readonly string OldText;
        public readonly string NewText;

        public TextChangedEventArgs(string old, string _new)
        {
            this.OldText = old;
            this.NewText = _new;
        }
    }
}
