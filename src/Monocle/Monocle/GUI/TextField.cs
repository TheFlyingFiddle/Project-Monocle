using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace Monocle.GUI
{
    public class TextField : TextBase
    {
        private static readonly TextFieldRenderer Default_Renderer = new TextFieldRenderer();

        private int markerPosition;

        public bool Editable
        {
            get;
            set;
        }

        public TextField(MouseDevice device) : base(device)
        {
            this.markerPosition = 0;
            this.Text = string.Empty;
            this.Renderer = Default_Renderer;
        }

        protected override void OnKeyDown(OpenTK.KeyPressEventArgs args)
        {
            base.OnKeyDown(args);
            if (args.KeyChar == '\u0008')
            {
                if (this.DeleteChar(this.markerPosition - 1))
                    this.markerPosition--;
            }
            else if (args.KeyChar == '\u007F')
            {
                this.DeleteChar(this.markerPosition);
            } 
            else
            {
                this.Text.Insert(this.markerPosition, args.KeyChar.ToString());
                this.markerPosition++;

            }
        }

        private bool DeleteChar(int position)
        {
            throw new NotImplementedException();
        }

    }
}
