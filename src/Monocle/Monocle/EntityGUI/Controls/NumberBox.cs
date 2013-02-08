using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using OpenTK.Graphics;
using Monocle.Graphics;
using OpenTK.Input;
using System.Globalization;

namespace Monocle.EntityGUI
{
    class NumberBox : TextBox
    {
        private const string VALUE_CHANGED_ID = "VALUE_CHANGED";

        private decimal value, maxValue, minValue;
        public decimal Value
        {
            get { return value; }
            set 
            {
                var tmp = this.value;
                if(value > this.maxValue) {
                    this.value = maxValue;
                } else if(value < this.minValue) {
                    this.value = minValue;
                } else 
                    this.value = value;

                if(tmp != value) {
                    this.OnValueChanged(new ValueChangedEventArgs<decimal>(tmp, this.value));
                }
            }
        }

        private void ChangeText()
        {
            if (this.Text == "")
            {
                if (this.minValue < 0) 
                {
                    this.editText.SetText("0");

                    if (this.value != 0)
                    {
                        var tmp = this.value;
                        this.value = 0;
                        this.OnValueChanged(new ValueChangedEventArgs<decimal>(tmp, this.value));
                    }
                    else
                    {
                        this.value = 0;
                    }
                }
                else 
                {
                    this.editText.SetText(this.minValue.ToString(CultureInfo.InvariantCulture));

                    if (this.value != this.minValue)
                    {
                        var tmp = this.value;
                        this.value = this.minValue;
                        this.OnValueChanged(new ValueChangedEventArgs<decimal>(tmp, this.value));
                    }
                    else
                    {
                        this.value = this.minValue;
                    }
                }
                return;
            }
            
            this.editText.SetText(this.value.ToString(CultureInfo.InvariantCulture));
            this.editText.MarkerIndex = this.editText.Length;
        }

        public decimal MaxValue
        {
            get { return this.maxValue; }
            set 
            {
                if(value < this.minValue)
                    throw new ArgumentException("The max value cannot be less then the min value.");

                this.maxValue = value;
                if(this.value > this.maxValue)
                    this.Value = maxValue;
            }
        }

        public decimal MinValue
        {
            get { return this.minValue; }
            set 
            {
                if(value > this.maxValue) 
                    throw new ArgumentException("The min value cannot be less then the min value.");

                this.minValue = value;
                if(this.value < this.minValue)
                    this.Value = minValue;
            }
        }


        public NumberBox(Font font, decimal startValue, decimal minValue = decimal.MinValue, decimal maxValue = decimal.MaxValue)
            : base(font, startValue.ToString(), 20)
        {
            this.value = startValue;
            this.maxValue = maxValue;
            this.minValue = minValue;
        }

        protected internal override void OnTextChanged(TextChangedEventArgs args)
        {
            base.OnTextChanged(args);
            decimal _old, _new;
            if (args.OldValue == "" || args.OldValue == "-" || args.OldValue == "-.")
            {
                _old = value;
            }
            else
            {
                _old = decimal.Parse(args.OldValue, CultureInfo.InvariantCulture);
            }

            if (args.NewValue == "" || args.NewValue == "-" || args.NewValue == "-.")
            {
                _new = value;
            }
            else
            {
                _new = decimal.Parse(args.NewValue, CultureInfo.InvariantCulture);
            }
            if(_old != _new) 
            {
                this.Value = _new;
            }
        }

        protected override void OnFocusLost(EventArgs eventArgs)
        {
            base.OnFocusLost(eventArgs);
            this.ChangeText();
        }


        protected override bool CharacterFilter(char c)
        {
            return char.IsDigit(c) ||
                c == '.' && !this.Text.Contains('.') ||
                c == '-' && !this.Text.Contains('-') && editText.MarkerIndex == 0
                || c == TextEditor.BackSpace || c == TextEditor.Delete;
        }

        protected internal void OnValueChanged(ValueChangedEventArgs<decimal> args)
        {
            this.Invoke(VALUE_CHANGED_ID, args);
        }

        

        public event EventHandler<ValueChangedEventArgs<decimal>> ValueChanged
        {
            add
            {
                this.AddEvent(VALUE_CHANGED_ID, value);
            }
            remove
            {
                this.RemoveEvent(VALUE_CHANGED_ID, value);
            }
        }

    }
}