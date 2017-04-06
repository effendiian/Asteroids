using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Attributes
{
    public class Padding
    {
        // Fields.
        private float xPadding;
        private float yPadding;

        // Constructor.
        public Padding(float padding)
        {
            xPadding = padding;
            yPadding = padding;
        }

        public Padding(float xPad, float yPad)
        {
            xPadding = xPad;
            yPadding = yPad;
        }

        // Properties.
        public Vector2 PaddingVector
        {
            get { return new Vector2(X, Y); }
        }

        public float X
        {
            get { return this.xPadding; }
        }

        public float Y
        {
            get { return this.yPadding; }
        }

        // Methods.
        public void SetPadding(float padding)
        {
            xPadding = padding;
            yPadding = padding;
        }

        public void SetPadding(float xPad, float yPad)
        {
            xPadding = xPad;
            yPadding = yPad;
        }

        public float GetXPadding()
        {
            return GetX(1);
        }

        public float GetYPadding()
        {
            return GetY(1);
        }

        public float GetX(float interval)
        {
            return xPadding * interval;
        }

        public float GetY(float interval)
        {
            return yPadding * interval;
        }

        public Vector2 Get(float interval)
        {
            return new Vector2(GetX(interval), GetY(interval));
        }
    }
}
