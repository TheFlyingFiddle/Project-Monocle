using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.EntityGUI
{
    interface IFocusManager
    {
        GUIControl FocusedControl { get; }
        
        bool FocusPrev();
        bool FocusNext();

        void ContainerFocusLost();
        void ContainerFocusGained();

        void GiveFocus(GUIControl control);
        void ContainerItemRemoved(GUIControl control);
        void ContainerItemAdded(GUIControl control);
    }

    class SimpleFocusManager : IFocusManager
    {
        private readonly List<GUIControl> focusableCollection;
        private int focusedIndex;

        public SimpleFocusManager()
        {
            this.focusableCollection = new List<GUIControl>();
            focusedIndex = -1;
        }
        
        public GUIControl FocusedControl
        {
            get
            {
                return this.focusedIndex < this.focusableCollection.Count && this.focusedIndex >= 0
                    ? this.focusableCollection[focusedIndex] : null;
            }
        }

        public void GiveFocus(GUIControl control)
        {
            var index = this.focusableCollection.IndexOf(control);
            if (index != -1 && this.focusableCollection[index].Focusable)
                this.ChangeFocus(index);
        }

        private void ChangeFocus(int newFocusIndex)
        {
            if (newFocusIndex == this.focusedIndex)
                return;

            var focused = this.FocusedControl;
            if (focused != null) 
                focused.Focused = false;
            
            this.focusedIndex = newFocusIndex;

            focused = this.FocusedControl;
            if (focused != null) 
                focused.Focused = true;
        }


        public bool FocusPrev()
        {
            int index = this.focusedIndex, count = 0;
            bool hasCrossed = false;
            while (true)
            {
                index -= 1;
                count++;
                if (index >= 0)
                {
                    if (this.focusableCollection[index].Focusable)
                    {
                        this.ChangeFocus(index);
                        return !hasCrossed;
                    }
                }
                else
                {
                    hasCrossed = true;
                    index = this.focusableCollection.Count;
                }

                if (count > this.focusableCollection.Count)
                    return false;
            }
        }

        public bool FocusNext()
        {
            int index = this.focusedIndex, count = 0;
            bool hasCrossed = false;
            while (true)
            {
                index += 1;
                count++;
                if (index < this.focusableCollection.Count)
                {
                    if (this.focusableCollection[index].Focusable)
                    {
                        this.ChangeFocus(index);
                        return !hasCrossed;
                    }
                } 
                else 
                {
                     hasCrossed = true;
                     index = -1;
                }

                if (count > this.focusableCollection.Count)
                    return false;
            }
        }

        public void ContainerFocusLost()
        {
            this.focusedIndex = 0;
        }

        public void ContainerFocusGained()
        {
            var focused = this.FocusedControl;
            if (focused != null) focused.Focused = true;
        }

        public void ContainerItemRemoved(GUIControl control)
        {
            this.focusableCollection.Remove(control);

            if (this.focusableCollection.Count == 0)
            {
                this.focusedIndex = -1;
            }
            else if (this.focusedIndex == this.focusableCollection.Count)
            {
                this.focusedIndex--;
            }
        }

        public void ContainerItemAdded(GUIControl control)
        {
            var focused = this.FocusedControl;
            this.focusableCollection.Add(control);
            this.focusableCollection.Sort((x, y) => x.FocusIndex.CompareTo(y.FocusIndex));

            if (focused != null)
                this.focusedIndex = this.focusableCollection.IndexOf(focused); 
        }
    }
}