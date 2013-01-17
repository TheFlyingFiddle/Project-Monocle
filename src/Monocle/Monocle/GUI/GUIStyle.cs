using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using OpenTK.Graphics;

namespace Monocle.GUI
{
	public class GUIStyle
	{
        public string StyleID
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public GUIState Normal
        {
            get;
            set;
        }

        public GUIState Hover
        {
            get;
            set;
        }

        public GUIState Active
        {
            get;
            set;
        }

        public GUIState Focused
        {
            get;
            set;
        }

        public GUIState Pressed
        {
            get;
            set;
        }

        public TextureFont Font
        {
            get;
            set;
        }

        public Rect Padding
        {
            get;
            set;
        }

        public Frame Icon
        {
            get;
            set;
        }

        public TextAlignment Alignment
        {
            get;
            set;
        }

        public bool WordWrap
        {
            get;
            set;
        }
	}

    public class GUIState
    {
        public Frame Frame;
        public Color4 FrameTint;
        public Color4 TextColor;
    }
}
