using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Attributes
{
    public class Gears
    {

        // Fields.
        private float minimum;
        private float maximum;
        private float increment;
        private int gears;

        // Properties.
        public int GearCount
        {
            get { return gears; }
            set { this.gears = value; }
        }

        public Gears() : this(4, 0, 1000)
        {
            // Default values.
        }

        public Gears(int _gears, float _min, float _max)
        {
            this.minimum = _min;
            this.maximum = _max;
            this.gears = _gears;            
        }
        
        // Calculates thresholds and returns a value based off of it.
        public int GetGear(float value)
        {
            float increment = Math.Abs(maximum - minimum) / gears;
            float valueCheck = minimum;
            int currentGear = 0;

            while (value > valueCheck)
            {
                valueCheck += increment;
                currentGear++;
            }

            return currentGear;
        }




    }
}
