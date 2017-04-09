/// ColorSet.cs - Version 2
/// Author: Ian Effendi
/// Date: 3.26.2017

#region Using statements.

// System using statements.
using System.Collections.Generic;
using System.Linq;

// MonoGame using statements.
using Microsoft.Xna.Framework;

#endregion

namespace Asteroids.Tools
{

    #region Enums. // Contains the definition for the ColorType enum.

    /// <summary>
    /// The type of colors stored in the colorset.
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// Color to draw something in.
        /// </summary>
        Draw,

        /// <summary>
        /// Color to draw something in when the mouse hovers over it.
        /// </summary>
        Hover,

        /// <summary>
        /// Color to draw something on collision.
        /// </summary>
        Collision,

        /// <summary>
        /// Color to draw something when it's disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// Miscellaneous color type.
        /// </summary>
        Other
    }

    #endregion

    public class ColorSet
    {

        #region Constants. // Contains default Color values for draw, hover, collision, disabled, and misc. color types.

        /// <summary>
        /// The default color to draw an entity's sprite with.
        /// </summary>
        private readonly static Color DEFAULT_DRAW_COLOR = Color.White;

        /// <summary>
        /// The default color to draw an entity with when in the hover state.
        /// </summary>
        private readonly static Color DEFAULT_HOVER_COLOR = Color.Yellow;

        /// <summary>
        /// The default color to draw an entity with when in the collision state.
        /// </summary>
        private readonly static Color DEFAULT_COLLIDE_COLOR = Color.Red;

        /// <summary>
        /// The default color to draw an entity with when in the disabled state.
        /// </summary>
        private readonly static Color DEFAULT_DISABLED_COLOR = Color.Gray;

        /// <summary>
        /// The default color to draw an entity with when in any state.
        /// </summary>
        private readonly static Color DEFAULT_COLOR = Color.White;

        #endregion

        #region Fields. // Contains the hashtable associating ColorTypes with Colors.

        /// <summary>
        /// <see cref="Color"/> objects associated with particular <see cref="ColorType"/> types.  
        /// </summary>
        private Dictionary<ColorType, Color> colorset;

        #endregion

        #region Properties. // Contains public access routes for information stored for each ColorType.

        /// <summary>
        /// Returns the current <see cref="Color"/>  associated with <see cref="ColorType.Draw"/>. 
        /// </summary>
        public Color DrawColor
        {
            get { return colorset[ColorType.Draw]; }
            set { SetDrawColor(value); }
        }

        /// <summary>
        /// Returns the current <see cref="Color"/>  associated with <see cref="ColorType.Hover"/>. 
        /// </summary>
        public Color HoverColor
        {
            get { return colorset[ColorType.Hover]; }
            set { SetHoverColor(value); }
        }

        /// <summary>
        /// Returns the current <see cref="Color"/>  associated with <see cref="ColorType.Collision"/>. 
        /// </summary>
        public Color CollisionColor
        {
            get { return colorset[ColorType.Collision]; }
            set { SetCollisionColor(value); }
        }

        /// <summary>
        /// Returns the current <see cref="Color"/>  associated with <see cref="ColorType.Disabled"/>. 
        /// </summary>
        public Color DisabledColor
        {
            get { return colorset[ColorType.Disabled]; }
            set { SetDisabledColor(value); }
        }

        /// <summary>
        /// Returns the current <see cref="Color"/>  associated with <see cref="ColorType.Other"/>. 
        /// </summary>
        public Color OtherColor
        {
            get { return colorset[ColorType.Other]; }
            set { SetOtherColor(value); }
        }

        #endregion

        #region Constructors. // Empty, general, and copy constructors for use with the class.

        /// <summary>
        /// Empty constructor gives <see cref="colorset"/> default values for each <see cref="ColorType"/>.
        /// </summary>
        public ColorSet()
        {
            // Set up the dictionary with basic colors.
            Initialize();
        }

        /// <summary>
        /// Constructor that specifies values for this <see cref="ColorSet"/> instance, but, allows default values through the use of optional parameters. 
        /// </summary>
        /// <param name="draw"><see cref="Color"/> associated with <see cref="ColorType.Draw"/>.</param>
        /// <param name="hover"><see cref="Color"/> associated with <see cref="ColorType.Hover"/>.</param>
        /// <param name="collide"><see cref="Color"/> associated with <see cref="ColorType.Collision"/>.</param>
        /// <param name="disabled"><see cref="Color"/> associated with <see cref="ColorType.Disabled"/>.</param>
        /// <param name="other"><see cref="Color"/> associated with <see cref="ColorType.Other"/>.</param>
        public ColorSet(Color? draw = null, Color? hover = null, Color? collide = null, Color? disabled = null, Color? other = null)
        {
            Initialize(draw, hover, collide, disabled, other);
        }

        /// <summary>
        /// Copy-constructor gives this colorset values from the input parameter.
        /// </summary>
        /// <param name="_colorset">The <see cref="ColorSet"/> object to copy information from.</param>
        public ColorSet(ColorSet _colorset)
        {
            // Set up the dictionary with basic colors.
            Initialize();
            AssignColors(_colorset);
        }

        #endregion

        #region Methods.

        #region Initialization. // Initialize the ColorSet.

        /// <summary>
        /// Initialize the ColorSet with input values. Missing values will result in the assignment of default colors to affected categories.
        /// </summary>
        /// <param name="draw"><see cref="Color"/> associated with <see cref="ColorType.Draw"/>.</param>
        /// <param name="hover"><see cref="Color"/> associated with <see cref="ColorType.Hover"/>.</param>
        /// <param name="collide"><see cref="Color"/> associated with <see cref="ColorType.Collision"/>.</param>
        /// <param name="disabled"><see cref="Color"/> associated with <see cref="ColorType.Disabled"/>.</param>
        /// <param name="other"><see cref="Color"/> associated with <see cref="ColorType.Other"/>.</param>
        private void Initialize(Color? draw = null, Color? hover = null, Color? collide = null, Color? disabled = null, Color? other = null)
        {
            colorset = new Dictionary<ColorType, Color>();
            colorset.Add(ColorType.Draw, DEFAULT_DRAW_COLOR);
            colorset.Add(ColorType.Hover, DEFAULT_HOVER_COLOR);
            colorset.Add(ColorType.Collision, DEFAULT_COLLIDE_COLOR);
            colorset.Add(ColorType.Disabled, DEFAULT_DISABLED_COLOR);
            colorset.Add(ColorType.Other, DEFAULT_COLOR);

            // Set the input colors.
            if (!NullColor(draw)) { SetDrawColor((Color)draw); }
            if (!NullColor(hover)) { SetHoverColor((Color)hover); }
            if (!NullColor(collide)) { SetCollisionColor((Color)collide); }
            if (!NullColor(disabled)) { SetDisabledColor((Color)disabled); }
            if (!NullColor(other)) { SetOtherColor((Color)other); }
        }

        #endregion

        #region Assign Color methods. // Assign colors, in different ways, to the ColorSet.

        /// <summary>
        /// Assign colors, selectively, from a dictionary of associations and from a variable array of colortypes. If no parameters are provided, just assign the colorset entirely.
        /// </summary>
        /// <param name="_colorset">A series of associations between <see cref="ColorType"/> keys and <see cref="Color"/> values.</param>
        /// <param name="types"><see cref="ColorType"/> types to assign associations for.</param>
        public void AssignColors(ColorSet _colorset, params ColorType[] types)
        {
            if (types.Count() > 0)
            {
                foreach (ColorType colType in types)
                {
                    SetColor(colType, _colorset.GetColor(colType));
                }
            }
            else
            {
                AssignColors(_colorset);
            }
        } 
        
        /// <summary>
        /// Assign the colors from the input colorset.
        /// </summary>
        /// <param name="_colorset">A series of associations between <see cref="ColorType"/> keys and <see cref="Color"/> values.</param>
        public void AssignColors(ColorSet _colorset)
        {
            SetDrawColor(_colorset.DrawColor);
            SetHoverColor(_colorset.HoverColor);
            SetCollisionColor(_colorset.CollisionColor);
            SetDisabledColor(_colorset.DisabledColor);
            SetOtherColor(_colorset.OtherColor);            
        }

        /// <summary>
        /// Assign colors, selectively, from a dictionary of associations and from a variable array of colortypes. If no parameters are provided, just assign the colorset entirely.
        /// </summary>
        /// <param name="_colorset">A series of associations between <see cref="ColorType"/> keys and <see cref="Color"/> values.</param>
        /// <param name="types"><see cref="ColorType"/> types to assign associations for.</param>
        public void AssignColors(Dictionary<ColorType, Color> _colorset, params ColorType[] types)
        {

            if (types.Count() > 0)
            {
                foreach (ColorType colType in types)
                {
                    SetColor(colType, _colorset[colType]);
                }
            }
            else
            {
                AssignColors(_colorset);
            }
        }

        /// <summary>
        /// Assign the colors from the input colorset.
        /// </summary>
        /// <param name="_colorset">A series of associations between <see cref="ColorType"/> keys and <see cref="Color"/> values.</param>
        public void AssignColors(Dictionary<ColorType, Color> _colorset)
        {
            SetDrawColor(_colorset[ColorType.Draw]);
            SetHoverColor(_colorset[ColorType.Hover]);
            SetCollisionColor(_colorset[ColorType.Collision]);
            SetDisabledColor(_colorset[ColorType.Disabled]);
            SetOtherColor(_colorset[ColorType.Other]);
        }

        #endregion

        #region Service methods. // Checks if the color is null.

        public bool NullColor(Color? col)
        {
            return (col == null);
        }

        #endregion

        #region Mutator methods. // Changes the values of the colors.

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

        public void SetOtherColor(Color c)
        {
            SetColor(ColorType.Disabled, c);
        }

        private void SetColor(ColorType colType, Color col)
        {
            if (colorset.ContainsKey(colType))
            {
                colorset[colType] = col;
            }
            else
            {
                colorset.Add(colType, col);
            }
        }

        #endregion

        #region Accessor methods. // Retrives colors of a certain type.

        public Color GetColor(ColorType colType)
        {
            if (colorset.ContainsKey(colType))
            {
                return colorset[colType];
            }
            else
            {
                return DEFAULT_COLOR;
            }
        }

        #endregion

        #endregion

    }
}
