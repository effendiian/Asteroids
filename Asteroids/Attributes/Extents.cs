using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Attributes
{
    public class Extents
    {
        // Extents holds a maximum magnitude, and current value.

        #region Fields.

        // Fields.
        private float max; // Maximum magnitude.
        private float value; // Current value.
        private float increment; // Metric.
        private bool clampToZero = false; // If true, minimum is zero. Else, it's negative maximum.

        #endregion

        #region Properties.

        public float Maximum
        {
            get { return this.max; }
            set { this.max = Math.Abs(value); }
        }

        public float Minimum
        {
            get
            {
                if (clampToZero) { return 0; }
                else { return (-1 * this.max); }
            }
        }

        public float Value
        {
            get { return this.value; }
            set { SetValue(value); }
        }

        public float Metric
        {
            get { return this.increment; }
        }

        public bool ZeroIsMinimum
        {
            get { return clampToZero; }
            set { clampToZero = value; }
        }

        public float Distance
        {
            get { return (float)Math.Sqrt(Math.Pow(Maximum, 2) + Math.Pow(Minimum, 2)); }
        }

        public float Center
        {
            get { return Minimum + (Distance / 2); }
        }

        #endregion

        #region Constructors.

        // An empty Extents.
        public Extents()
        {
            SetClampMode(clampToZero);
            SetMaximum(10.0f);
            SetIncrement(1.0f);
            Validate();
            SetValue(Center);
        }

        // Extents with no value term.
        public Extents(float _max = 10.0f, float _increment = 1.0f, bool _clampToZero = true)
        {
            SetClampMode(_clampToZero);
            SetMaximum(_max);
            SetIncrement(_increment);
            Validate();
            SetValue(Center);
        }

        // Extents with provided value term.
        public Extents(float _max = 10.0f, float _increment = 1.0f, bool _clampToZero = true, float _value = 0.0f)
        {
            SetClampMode(_clampToZero);
            SetMaximum(_max);
            SetIncrement(_increment);
            Validate();
            SetValue(_value);
        }

        #endregion

        #region Mutator Methods.

        public void SetClampMode(bool mode)
        {
            this.clampToZero = mode;
        }

        public void SetMaximum(float value)
        {
            this.max = Math.Abs(value);
        }

        public void SetIncrement(float inc)
        {
            this.increment = Math.Abs(inc);
        }

        public void SetValue(float val)
        {
            this.value = val;
            Clamp();
        }
        public void SetValue(int val)
        {
            SetValue((float)val);
            Clamp();
        }
        
        #endregion

        #region Service Methods.

        // Validate the maximum and minimum values.
        private void Validate()
        {
            while (Maximum < Minimum)
            {
                this.max = Math.Abs(Maximum);
            }

            Clamp();
        }

        // Clamp the value being held.
        private void Clamp()
        {
            this.value = MathHelper.Clamp(this.value, Minimum, Maximum);
        }

        // Increment/Decrement methods.
        public void Increment()
        {
            Increment(increment);
        }

        public void Increment(float val)
        {
            this.value += val;
            Clamp();
        }

        public void Decrement()
        {
            Increment(-increment);
        }

        public void Decrement(float val)
        {
            Increment(-val);
        }
        
        // Proximity methods?
        public bool CloseToMinimum(float val)
        {
            return ((val < (Minimum + Center)));
        }

        public bool CloseToMaximum(float val)
        {
            return ((val > (Maximum - Center)));
        }

        public bool CloseToZero(float val)
        {
            return ((val > (0 - Center)) && (val < (0 + Center)));
        }

        #endregion
        
    }
}
