using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Asteroids.Tools
{
    public enum ColorType
    {
        Draw,
        Hover,
        Collision,
        Disabled
    }

    public class ColorSet
    {
        // Constants.
        private readonly static Color DEFAULT_DRAW_COLOR = Color.White;
        private readonly static Color DEFAULT_HOVER_COLOR = Color.Yellow;
        private readonly static Color DEFAULT_COLLIDE_COLOR = Color.Red;
        private readonly static Color DEFAULT_DISABLED_COLOR = Color.Gray;

        // Fields.
        private Dictionary<ColorType, Color> colorset;
        
        // Get colors
        public Color DrawColor
        {
            get { return colorset[ColorType.Draw]; }
            set { SetDrawColor(value); }
        }
        
        public Color HoverColor
        {
            get { return colorset[ColorType.Hover]; }
            set { SetHoverColor(value); }
        }

        public Color CollisionColor
        {
            get { return colorset[ColorType.Collision]; }
            set { SetCollisionColor(value); }
        }

        public Color DisabledColor
        {
            get { return colorset[ColorType.Disabled]; }
            set { SetDisabledColor(value); }
        }

        // Constructor.
        public ColorSet()
        {
            // Set up the dictionary with basic colors.
            Initialize();
        }
        
        public ColorSet(Color? draw = null, Color? hover = null, Color? collide = null, Color? disabled = null)
        {
            Initialize(draw, hover, collide, disabled);
        }

        // Methods.
        private void Initialize(Color? draw = null, Color? hover = null, Color? collide = null, Color? disabled = null)
        {
            colorset = new Dictionary<ColorType, Color>();
            colorset.Add(ColorType.Draw, DEFAULT_DRAW_COLOR);
            colorset.Add(ColorType.Hover, DEFAULT_HOVER_COLOR);
            colorset.Add(ColorType.Collision, DEFAULT_COLLIDE_COLOR);
            colorset.Add(ColorType.Disabled, DEFAULT_DISABLED_COLOR);

            // Set the input colors.
            if (!NullColor(draw)) { SetDrawColor((Color)draw); }
            if (!NullColor(hover)) { SetDrawColor((Color)hover); }
            if (!NullColor(collide)) { SetDrawColor((Color)collide); }
            if (!NullColor(disabled)) { SetDrawColor((Color)disabled); }
        }

        public bool NullColor(Color? col)
        {
            return (col == null);
        }

        // Set colors.
        public void SetDrawColor(Color c)
        {
            SetColor(ColorType.Draw, c);
        }
        public void SetHoverColor(Color c)
        {
            SetColor(ColorType.Hover, c);
        }

        public void SetCollisionColor(Color c)
        { 
            SetColor(ColorType.Collision, c);
        }

        public void SetDisabledColor(Color c)
        {
            SetColor(ColorType.Disabled, c);
        }

        private void SetColor(ColorType colType, Color col)
        {
            colorset[colType] = col;
        }

    }
}
