using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class CheckBox : ToggleButton
    {
        private static ToggleButtonRenderer renderer = new ToggleButtonRenderer(LookAndFeelAssets.CheckBox_Unchecked.ToString(),
                                                                                LookAndFeelAssets.CheckBox_Checked.ToString(),
                                                                                LookAndFeelAssets.CheckBox_Over.ToString(),
                                                                                LookAndFeelAssets.CheckBox_Pressed.ToString());
        public CheckBox(MouseDevice device) : base(device) { }
    }
}
