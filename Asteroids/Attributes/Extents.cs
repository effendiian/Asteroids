/// Extents.cs - Version 3
/// Author: Ian Effendi
/// Date: 3.30.2017
/// Last Updated: 4.6.2017
/// File Description: Extents is used to limit vector values.

#region Using statements.

// Import the system packages.
using System;

// Import the Monogame packages.
using Microsoft.Xna.Framework;

#endregion

namespace Asteroids.Attributes
{
    /// <summary>
    /// Extents is used to limit vector values with a magnitude. It holds a maximum magnitude and a current value.
    /// </summary>
    public class Extents
    {
                
        #region Fields. // Contains a magnitude, a current value, and a metric for increment/decrements.
        
        /// <summary>
        /// Maximum magnitude to clamp to.
        /// </summary>
        private float max; // Maximum magnitude.

        /// <summary>
        /// The current value an extent holds.
        /// </summary>
        private float value; // Current value.

        /// <summary>
        /// Increment and decrement metric.
        /// </summary>
        private float increment; // Metric.
        
        #endregion

        #region Flags.

        /// <summary>
        /// Flag determines if minimum is zero or if minimum is the negative magnitude.
        /// </summary>
        private bool clampToZero = false; // If true, minimum is zero. Else, it's negative maximum.

        #endregion

        #region Properties. // Publicly accessible routes for private data.

        /// <summary>
        /// The maximum value an extent can contain. Always positive.
        /// </summary>
        public float Maximum
        {
            get { return this.max; }
            set { this.max = Math.Abs(value); }
        }

        /// <summary>
        /// The minimum value. If the extent clamps at zero, the minimum will be zero.
        /// </summary>
        public float Minimum
        {
            get
            {
                if (clampToZero) { return 0; }
                else { return (-1 * this.max); }
            }
        }

        /// <summary>
        /// The current value the extent holds.
        /// </summary>
        public float Value
        {
            get { return this.value; }
            set { SetValue(value); }
        }

        /// <summary>
        /// The amount to increase a value by.
        /// </summary>
        public float Metric
        {
            get { return this.increment; }
        }

        /// <summary>
        /// Determines if zero is the minimum or not.
        /// </summary>
        public bool ZeroIsMinimum
        {
            get { return clampToZero; }
            set { clampToZero = value; }
        }

        /// <summary>
        /// Finds the difference between the maximum magnitude and the minimum value.
        /// </summary>
        public float Distance
        {
            get { return (float)Math.Sqrt(Math.Pow(Maximum, 2) + Math.Pow(Minimum, 2)); }
        }

        /// <summary>
        /// Finds the halfway point between the magnitude maximum and the minimum.
        /// </summary>
        public float Center
        {
            get { return Minimum + (Distance / 2); }
        }

        #endregion

        #region Constructors. // Detailed ways to create an Extents object.

        /// <summary>
        /// Creates an Extents with a default maximum of 10f and a minimum of 0f.
        /// </summary>
        public Extents()
        {
            SetClampMode(clampToZero);
            SetMaximum(10.0f);
            SetIncrement(1.0f);
            Validate();
            SetValue(Center);
        }

        /// <summary>
        /// Creates a custom Extents with no set value.
        /// </summary>
        /// <param name="_max">Maximum magnitude.</param>
        /// <param name="_increment">Metric for increments and decrements.</param>
        /// <param name="_clampToZero">Flag determining if the minimum should be zero.</param>
        public Extents(float _max = 10.0f, float _increment = 1.0f, bool _clampToZero = true)
        {
            SetClampMode(_clampToZero);
            SetMaximum(_max);
            SetIncrement(_increment);
            Validate();
            SetValue(Center);
        }

        /// <summary>
        /// Creates a custom Extents with a value.
        /// </summary>
        /// <param name="_max">Maximum magnitude.</param>
        /// <param name="_increment">Metric for increments and decrements.</param>
        /// <param name="_clampToZero">Flag determining if the minimum should be zero.</param>
        /// <param name="_value">Current value to set the Extents to.</param>
        public Extents(float _max = 10.0f, float _increment = 1.0f, bool _clampToZero = true, float _value = 0.0f)
        {
            SetClampMode(_clampToZero);
            SetMaximum(_max);
            SetIncrement(_increment);
            Validate();
            SetValue(_value);
        }

        #endregion

        #region Methods. // Methods that aid the Extents object in completing its tasks.

        #region Mutator Methods. // Methods created to set data and pass through validation functions.

        /// <summary>
        /// Determines if zero is the minimum (or not).
        /// </summary>
        /// <param name="mode">Value given to <see cref="clampToZero"/>.</param>
        public void SetClampMode(bool mode)
        {
            this.clampToZero = mode;
        }

        /// <summary>
        /// Sets the maximum.
        /// </summary>
        /// <param name="value">Value given to <see cref="max"/>.</param>
        public void SetMaximum(float value)
        {
            this.max = Math.Abs(value);
        }

        /// <summary>
        /// Sets the metric value.
        /// </summary>
        /// <param name="value">Value given to <see cref="increment"/>.</param>
        public void SetIncrement(float inc)
        {
            this.increment = Math.Abs(inc);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="val">Value given to <see cref="value"/>.</param>
        public void SetValue(float val)
        {
            this.value = val;
            Clamp();
        }

        /// <summary>
        /// Sets the value, after casting as a float.
        /// </summary>
        /// <param name="val">Value given to <see cref="value"/>.</param>
        public void SetValue(int val)
        {
            SetValue((float)val);
            Clamp();
        }
        
        #endregion

        #region Service Methods. // Methods created to aid in the validation of set data.
        
        /// <summary>
        /// Validates the maximum and minimum values.
        /// </summary>
        private void Validate()
        {
            while (Maximum < Minimum)
            {
                this.max = Math.Abs(Maximum);
            }

            Clamp();
        }

        /// <summary>
        /// Clamps <see cref="value"/> between <see cref="Maximum"/> and <see cref="Minimum"/>.
        /// </summary>
        private void Clamp()
        {
            this.value = MathHelper.Clamp(this.value, Minimum, Maximum);
        }

		/// <summary>
		/// Clamps <see cref="val"/> between <see cref="Maximum"/> and <see cref="Minimum"/>.
		/// </summary>
		/// <param name="val">Value being clamped.</param>
		public float Clamp(float val)
		{
			return MathHelper.Clamp(val, Minimum, Maximum);
		}

		/// <summary>
		/// Increases <see cref="value"/> by <see cref="increment"/> and then clamps it between the <see cref="Minimum"/> and <see cref="Maximum"/> values.
		/// </summary>
		public void Increment()
        {
            Increment(increment);
        }

        /// <summary>
        /// Increases <see cref="value"/> by a custom amount and then clamps it between the <see cref="Minimum"/> and <see cref="Maximum"/> values.
        /// </summary>
        /// <param name="val">Amount to increase <see cref="value"/> by.</param>
        public void Increment(float val)
        {
            this.value += val;
            Clamp();
        }

        /// <summary>
        /// Decreases <see cref="value"/> by <see cref="increment"/> and then clamps it between the <see cref="Minimum"/> and <see cref="Maximum"/> values.
        /// </summary>
        public void Decrement()
        {
            Increment(-increment);
        }

        /// <summary>
        /// Decreases <see cref="value"/> by a custom amount and then clamps it between the <see cref="Minimum"/> and <see cref="Maximum"/> values.
        /// </summary>
        /// <param name="val">Amount to decrease <see cref="value"/> by.</param>
        public void Decrement(float val)
        {
            Increment(-val);
        }

        #region Proximity methods? // Methods dealing with proximity to certain values.

        /// <summary>
        /// Determines if the input value is close to the <see cref="Minimum"/> value.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <returns>Returns true if the value is in the lesser half of the distance between the maximum and minimum.</returns>
        public bool CloseToMinimum(float val)
        {
            return CloseToMinimum(val, Center);
        }

        /// <summary>
        /// Determines if the input value is close to the <see cref="Minimum"/> value.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <param name="radius">Distance considered close.</param>
        /// <returns>Returns true if the value is close.</returns>
        public bool CloseToMinimum(float val, float radius)
        {
            return ((val < (Minimum + radius)));
        }

        /// <summary>
        /// Determines if the input value is close to the <see cref="Maximum"/> value.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <returns>Returns true if the value is in the greater half of the distance between the maximum and minimum.</returns>
        public bool CloseToMaximum(float val)
        {
            return CloseToMaximum(val, Center);
        }

        /// <summary>
        /// Determines if the input value is close to the <see cref="Maximum"/> value.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <param name="radius">Distance considered close.</param>
        /// <returns>Returns true if the value is close.</returns>
        public bool CloseToMaximum(float val, float radius)
        {
            return ((val > (Maximum - radius)));
        }

        /// <summary>
        /// Determines if the input value is close to zero.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <returns>Returns true if the value is within half of the total distance between the maximum and minimum, away from the zero.</returns>
        public bool CloseToZero(float val)
        {
            return CloseToZero(val, Center);
        }
        
        /// <summary>
        /// Determines if the input value is close to zero.
        /// </summary>
        /// <param name="val">Value to check.</param>
        /// <param name="radius">Distance considered close.</param>
        /// <returns>Returns true if the value is close to zero.</returns>
        public bool CloseToZero(float val, float radius)
        {
            return ((val > (0 - radius)) && (val < (0 + radius)));
        }

        #endregion

        #endregion

        #endregion

    }
}
