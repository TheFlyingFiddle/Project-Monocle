using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class TextBase : GUIControl
    {
        public event EventHandler<TextChangedEventArgs> TextChanged;

        private string text;

        public string Text
        {
            get { return this.text; }
            set
            {
                var tmp = this.text;
                this.text = value;
                this.OnTextChanged(tmp, value);
            }
        }

        public TextBase(MouseDevice device) : base(device) { }

        private void OnTextChanged(string old, string _new)
        {
            var args = new TextChangedEventArgs(old, _new);
            if (TextChanged != null)
                TextChanged.Invoke(this, args);
        }
        
       
    }
}