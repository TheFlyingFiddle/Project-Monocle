using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.GUI
{
    public class LookAndFeel
    {
        private Dictionary<string, VisibleElement> elements;

        public VisibleElement this[string key]
        {
            get { return this.elements[key]; }
        }

        public LookAndFeel(Dictionary<string, VisibleElement> elements)
        {
            this.elements = elements;
        }
    }

    public enum LookAndFeelAssets
    {
        Button_BG,
        Button_Down,
        Button_Over,
        Button_Pressed,
        CheckBox_Unchecked,
        CheckBox_Checked,
        CheckBox_Over,
        CheckBox_Pressed

    }
}
