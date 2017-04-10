/// Entity.cs - Version 4
/// Author: Ian Effendi
/// Date: 3.12.2017

#region Using statements

// System using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add the XNA using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Add project using statements.
using Asteroids.Tools;
using Asteroids.Attributes;

#endregion

namespace Asteroids.Entities
{

    // EntityType enum: Player, Asteroid, Test, Star, Particle, Generator.
    // CollisionBehavior enum: ColorChange, Die, Hurt, Null.
    // ScrollBehavior enum: Die, Spawn, Wrap, Bounce, Null.

    #region Enums. // EntityType, CollisionBehavior, and ScrollBehavior are located here.

    /// <summary>
	/// EntityType enum describes the type of entities that can be created.
	/// </summary>
    public enum EntityType
    {
		/// <summary>
		/// The player object is controlled by the player.
		/// </summary>
        Player,

		/// <summary>
		/// An asteroid a player can destroy.
		/// </summary>
        Asteroid,
		
		/// <summary>
		/// A test object.
		/// </summary>
        Test,

		/// <summary>
		/// A star.
		/// </summary>
        Star,

		/// <summary>
		/// A particle.
		/// </summary>
        Particle,

		/// <summary>
		/// A generator entity, that will spawn other entities.
		/// </summary>
		Generator
    }

    /// <summary>
	/// CollisionBehavior is an enum tracking the different actions to perform on collision.
	/// </summary>
    public enum CollisionBehavior
    {
		/// <summary>
		/// Change colors on collision.
		/// </summary>
        ColorChange,

		/// <summary>
		/// Instantly kill object on collision.
		/// </summary>
        Die,

		/// <summary>
		/// Damage object on collision.
		/// </summary>
        Hurt,

		/// <summary>
		/// Do nothing on collision.
		/// </summary>
        Null
    }

    /// <summary>
	/// ScrollBehavior determines the type of action to undertake when scrolling.
	/// </summary>
    public enum ScrollBehavior
    {
		/// <summary>
		/// Kill objects that leave the screen.
		/// </summary>
        Die,

		/// <summary>
		/// Spawn a new object somewhere on the screen in this object's place.
		/// </summary>
        Spawn,

		/// <summary>
		/// Wrap the object's position on-screen.
		/// </summary>
        Wrap,

		/// <summary>
		/// Flip the object's velocity on collision.
		/// </summary>
		Bounce,

		/// <summary>
		/// The 'nothing' behavior. This should do the same thing as dying.
		/// </summary>
		Null
    }

	#endregion
	
	/// <summary>
	/// An <see cref="Entity"/> is any object with a texture,
	/// that has a position on screen, a tint color, 
	/// and boundaries that can deal with collision with other entities.
	/// </summary>
	public abstract class Entity
    {
		#region Fields. // Privately accessed data responsible for interacting with the Entity.
		
		#region Entity archival information.

		/// <summary>
		/// The Entity's tag.
		/// </summary>
		protected string tag;

		/// <summary>
		/// The type of Entity.
		/// </summary>
		protected EntityType type;

		#endregion

		#region Items used for drawing or entity management.

		/// <summary>
		/// The state this Entity is located within.
		/// </summary>
		protected State state;

		/// <summary>
		/// The image the entity is drawn with.
		/// </summary>
        protected Texture2D image;

		#endregion

		#region Items used for tracking location, rotation, and size.
		
		/// <summary>
		/// The Entity's on-screen position. (Centered).
		/// </summary>
		protected Vector2 position;

		/// <summary>
		/// The Entity's origin determines where to put the rotational point.
		/// </summary>
        protected Vector2 origin;

		/// <summary>
		/// The Entity's dimensions are the <see cref="originalDimensions"/> scaled by the <see cref="scale"/>. 
		/// </summary>
        protected Vector2 dimensions;

		/// <summary>
		/// The Entity's original dimensions are the values given before being modified by scale.
		/// </summary>
        protected Vector2 originalDimensions; // The "1.0" scale base size.

		/// <summary>
		/// The image dimensions correspond to the size of the <see cref="Texture2D"/> <see cref="image"/> in the Entity.  
		/// </summary>
		protected Vector2 imageDimensions; // The image dimensions. ("The source").

		/// <summary>
		/// Rotation of the entity in radians.
		/// </summary>
        protected float rotation; // Rotation of the entity.

		/// <summary>
		/// The scale given to draw an Entity.
		/// </summary>
        protected float scale = 1.0f; // The scale of an entity.

		#endregion

		#region Items used for tracking the drawn color.
		
		/// <summary>
		/// The current color the Entity is being drawn with.
		/// </summary>
		protected Color drawColor; // The current color to draw with.

		/// <summary>
		/// The whole set of colors the Entity can choose from.
		/// </summary>
        protected ColorSet colorSet; // The set of colors the entity is drawn with.

		#endregion

		#region Collision behaviors with other entities and with the screen.

		/// <summary>
		/// List of behaviors to perform when an Entity leaves the screen.
		/// </summary>
		protected List<ScrollBehavior> scrollModes; // What should the entity do when off-screen.

		/// <summary>
		/// List of behaviors to perform when an Entity has a collision.
		/// </summary>
        protected List<CollisionBehavior> collisionModes; // What should the entity do when hit.

		#endregion

		#region Dealing with control input.

		protected ControlScheme schema; // Control scheme used to affect entities in different ways.

		#endregion

		#endregion

		#region Flags. // Different flags noting the status of the Entity.

		/// <summary>
		/// Determines if an entity is enabled or disabled.
		/// </summary>
		protected bool enabled = false; // Enabled/Disabled flag.

		/// <summary>
		/// Determines if an entity should be drawn to the screen.
		/// </summary>
		protected bool visible = false; // Visibility flag.

		/// <summary>
		/// Determines if an entity is in debug mode or not.
		/// </summary>
		protected bool debug = false; // Debug flag.

		/// <summary>
		/// Determines if an entity has a GUI/HUD information to draw.
		/// </summary>
		protected bool drawGUI = false; // Draw GUI flag.

		/// <summary>
		/// Determines if an entity is currently in proximity with another entity that has a collision mode other than Null.
		/// </summary>
		protected bool isInProximity = false; // If something is close to this entity, the state will set this value to true.

		/// <summary>
		/// Determines if an entity is currently colliding with another entity that has a collision mode other than Null.
		/// </summary>
		protected bool isColliding = false; // If something is colliding with this entity, the state will set this value to true.

		#endregion

		#region Properties. // Publicly accessible data.

		/// <summary>
		/// Determines if the entity is currently enabled.
		/// </summary>
		public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

		/// <summary>
		/// Determines if the entity is currently disabled.
		/// </summary>
        public bool Disabled
        {
            get { return !this.enabled; }
            set { this.enabled = !value; }
        }

		/// <summary>
		/// Returns a list of all of the different behaviors that can be achieved when the entity leaves the screen.
		/// </summary>
        public List<ScrollBehavior> ScrollModes
        {
            get { return this.scrollModes; }
            set { this.scrollModes = value; }
        }

		/// <summary>
		/// Flag that determines if this entity can scroll at all.
		/// </summary>
        public bool IsScrollable
        {
            get
			{
				foreach (ScrollBehavior mode in scrollModes)
				{
					// Return true if the scroll behavior is currently set to wrap.
					if(mode == ScrollBehavior.Wrap) { return true; }
				}

				return false;
			}
        }

		/// <summary>
		/// Returns a list of all of the different behaviors that can be achieved when the entity leaves the screen.
		/// </summary>
		public List<CollisionBehavior> CollisionModes
        {
            get { return this.collisionModes; }
            set { this.collisionModes = value; }
        }

		/// <summary>
		/// Flag that determines if this entity can collide with anything at all.
		/// </summary>
        public bool IsCollidable
        {
			get
			{
				foreach (CollisionBehavior mode in collisionModes)
				{
					// Return true if the scroll behavior is currently set to wrap.
					if (mode != CollisionBehavior.Null) { return true; }
				}

				return false;
			}
		}

		/// <summary>
		/// Handles the visibility of an Entity.
		/// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

		/// <summary>
		/// Set the Debug flag for the entity.
		/// </summary>
        public bool Debug
        {
            get { return this.debug; }
            set { this.debug = value; }
        }

		/// <summary>
		/// Returns the type of EntityType given to this Entity.
		/// </summary>
        public EntityType Type
        {
            get { return this.type; }
            protected set { this.type = value; }
        }

		/// <summary>
		/// The tag value of a given entity.
		/// </summary>
        public string Tag
        {
            get { return this.tag; }
            set { this.tag = value; }
        }
        
		/// <summary>
		/// If this has an image, return true.
		/// </summary>
        public bool HasImage
        {
            get { return (image == null); }
        }

		/// <summary>
		/// Returns the image file used to draw this texture.
		/// </summary>
        public Texture2D Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /// <summary>
		/// The position of the entity.
		/// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
		/// The x-coordinate of the value.
		/// </summary>
        public float X
        {
            get { return this.position.X; }
            set { this.position = new Vector2(value, this.position.Y); }
        }

		/// <summary>
		/// The y-coordinate of the value.
		/// </summary>
		public float Y
        {
            get { return this.position.Y; }
            set { this.position = new Vector2(this.position.X, value); }
        }

        /// <summary>
		/// Dimensions returns the value of the dimensions after scaling.
		/// </summary>
        public Vector2 Dimensions
        {
            get { return this.dimensions; }
        }

		public Vector2 UnscaledDimensions
		{
			get { return this.originalDimensions; }
			set { this.SetDimensions(value); }
		}

		/// <summary>
		///  The center of the entity. (Since it is centered around the politician, this is pointless).
		/// </summary>
        public virtual Vector2 Center
        {
            get { return this.position; }
        }

        /// <summary>
		/// The radius.
		/// </summary>
        public float Radius
        {
            // The radius is the largest dimension.
            // Eg. If height < width, this will returnt the width.
            get { return this.dimensions.Length(); }
        }

        /// <summary>
		/// The width.
		/// </summary>
        public float Width
        {
            get { return this.dimensions.X; }
            set { this.dimensions = new Vector2(value, this.dimensions.Y); }
        }

        /// <summary>
		/// The height.
		/// </summary>
        public float Height
        {
            get { return this.dimensions.Y; }
            set { this.dimensions = new Vector2(this.dimensions.X, value); }
        }
		
		/// <summary>
		/// Rotation of the entity.
		/// </summary>
		public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }
		
		/// <summary>
		/// The boundaries of the entity. Also known as the display rectangle.
		/// </summary>
		public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - (int)(dimensions.X / 2), (int)position.Y - (int)(dimensions.Y / 2), (int)dimensions.X, (int)dimensions.Y); }
        }
		
		/// <summary>
		/// Rectangle storing the image dimensions.
		/// </summary>
        public Rectangle Source
        {
            get { return new Rectangle(0, 0, image.Width, image.Height); }
        }
		
		/// <summary>
		/// Color to draw the entity.
		/// </summary>
        public Color DrawColor
        {
            get { return this.drawColor; }
        }

		/// <summary>
		/// Returns this entity's color palatte.
		/// </summary>
		public ColorSet Colorset
		{
			get { return this.colorSet; }
		}

        #endregion

        #region Constructor

        /// An entity, at the minimum, needs a state object and a texture to be created.
        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, Vector2? _size = null,
            float _rotation = 0f, ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
			bool _drawGUI = false, ControlScheme _schema = null,
			Color? _drawColor = null,
			Color? _hoverColor = null, Color? _collisionColor = null,
			Color? _disableColor = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, new ColorSet(_drawColor, _hoverColor, _collisionColor, _disableColor), _scroll, _collision, _enabled, _visible, _drawGUI, _schema);
        }

        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, Vector2? _size = null,
            float _rotation = 0f, ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
			bool _drawGUI = false, ControlScheme _schema = null,
			ColorSet _set = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, _set, _scroll, _collision, _enabled, _visible, _drawGUI, _schema);
        }
        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, float _size = 0.0f,
            float _rotation = 0f, ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _drawGUI = false, ControlScheme _schema = null, Color? _drawColor = null,
			Color? _hoverColor = null, Color? _collisionColor = null,
			Color? _disableColor = null)

		{
            Initialize(_state, _image, _tag, _pos, _size, _rotation, new ColorSet(_drawColor, _hoverColor, _collisionColor, _disableColor), _scroll, _collision, _enabled, _visible, _drawGUI, _schema);
        }

        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, float _size = 0.0f,
            float _rotation = 0f, ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _drawGUI = false, ControlScheme _schema = null,
			ColorSet _set = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, _set, _scroll, _collision, _enabled, _visible, _drawGUI, _schema);
        }

        #endregion

        #region Methods

        #region Initialize Entity // Methods that set up default values OR set up custom values.

        // Initialize the Entity using values or optional parameters in its place.
        protected void Initialize(State _state,
            Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, Vector2? _size = null,
            float _rotation = 0f, ColorSet _set = null,
            ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
			bool _drawGUI = false, ControlScheme _schema = null)
        {
            SetState(_state); // Store the current state.
            SetImage(_image); // File the image to draw with.
            SetTag(_tag); // Add a tag for the element.
            SetColorSet(_set); // Set the colors we'd like to use.
            SetDimensions(_size); // By default, base the size off of the image's picture.
            SetRotation(_rotation); // By default this will be 0, but, we can customize on creation. Useful for spawning.
            SetScrollModes(_scroll); // Wrap entity by default.
            SetCollisionModes(_collision); // Do not collide by default.
            SetFlags(_enabled, _visible); // Set these flags up by default.
            CreateControlScheme(_schema);
        }

        protected void Initialize(State _state,
           Texture2D _image, string _tag = "Entity [Default]",
           Vector2? _pos = null, float _size = 0.0f,
           float _rotation = 0f, ColorSet _set = null,
           ScrollBehavior _scroll = ScrollBehavior.Wrap,
           CollisionBehavior _collision = CollisionBehavior.Null,
           bool _enabled = false, bool _visible = false,
			bool _drawGUI = false, ControlScheme _schema = null)
		{
            SetState(_state); // Store the current state.
            SetImage(_image); // File the image to draw with.
            SetTag(_tag); // Add a tag for the element.
            SetColorSet(_set); // Set the colors we'd like to use.
            SetDimensions(_size); // By default, base the size off of the image's picture.
            SetRotation(_rotation); // By default this will be 0, but, we can customize on creation. Useful for spawning.
            SetScrollModes(_scroll); // Wrap entity by default.
            SetCollisionModes(_collision); // Do not collide by default.
            SetFlags(_enabled, _visible); // Set these flags up by default.
            CreateControlScheme(_schema);
        }

        protected virtual void SetFlags(bool _enabled = false, bool _visible = false, bool _drawGUI = false)
        {
            // Set these flags.
            this.enabled = _enabled;
            this.visible = _visible;
            this.drawGUI = _drawGUI;

            // Initialize these flags.
            this.debug = false;
            this.isInProximity = false;
            this.isColliding = false;
        }

        protected void CreateControlScheme(ControlScheme _schema = null)
        {
            if (_schema == null)
            {
                // If a scheme doesn't exist, then create a schema and assign commands.
                this.schema = new ControlScheme();
                SetUpControlScheme();
            }
            else
            {
                this.schema = _schema;
            }
        }
        
        // This should be where the child classes specify what keys to add.
        protected abstract void SetUpControlScheme();
		
		/// <summary>
		/// Wrapper function. Bind a new keys to the schema.
		/// </summary>
		/// <param name="command">Command to activate on fire status.</param>
		/// <param name="key">Keys binded to the command.</param>
		protected void AssignControl(Commands command, List<Keys> keys)
        {
			foreach (Keys key in keys)
			{
				this.schema.Bind(command, key, ActionType.Released);
			}
        }
		
		/// <summary>
		/// Wrapper function. Bind a new key to the schema.
		/// </summary>
		/// <param name="command">Command to activate on fire status.</param>
		/// <param name="key">Key binded to the command.</param>
        protected void AssignControl(Commands command, Keys key)
		{
			this.schema.Bind(command, key, ActionType.Released);
		}

        #endregion
        
        #region Mutator Methods. // Set data using these. Get data, usually, through properties.

        // Sets the state the entity is in and gets the pen to draw with.
        private void SetState(State _state)
        {
            // Assigning the state to a field.
            this.state = _state;

            // Attributes that can be called from the state.
            this.position = state.GetScreenCenter();
            this.scale = state.GetScale();
        }

        // Set the drawing scale.
        public virtual void SetScale(float scale)
        {
            this.scale = scale;
            this.dimensions = this.originalDimensions * scale;
        }

        // Sets the texture.
        private void SetImage(Texture2D _image)
        {
            // Assigns the image.
            this.image = _image;

            // Attributes that can be set based off of the image itself.
            this.imageDimensions = new Vector2(this.image.Width, this.image.Height);
            this.origin = imageDimensions * 0.5f;
        }

        // Sets the tag.
        private void SetTag(string _tag)
        {
            this.tag = _tag;
        }

        // Set the colorset.
        private void SetColorSet(ColorSet _set)
        {
            if (_set == null)
            {
                this.colorSet = new ColorSet();
            }
            else
            {
                this.colorSet = _set;
            }

            this.drawColor = colorSet.DrawColor; // By default set the draw color to the color set's value.
        }

        // Set the dimensions.
        public void SetDimensions(float size)
        {
            // Sets to a square size.
            SetDimensions(size, size);
        }

        public void SetDimensions(float width, float height)
        {
            // Sets to vector.
            SetDimensions(new Vector2(ClampZero(width), ClampZero(height)));
        }

        private void SetDimensions(Vector2? _size = null)
        {
            if(_size == null) { SetDimensions(new Vector2(ClampZero(0), ClampZero(0))); }
            else { SetDimensions((Vector2)_size); }
        }

        private void SetDimensions(Vector2 unscaledDimensions)
        {
            this.originalDimensions = unscaledDimensions;
            this.dimensions = originalDimensions * scale; // Base the dimensions off of the original dimensions and multiply by the state's scale.
            // this.origin = dimensions * 0.5f; // Base the origin off of the scaled dimensions and divide by half.
        }

        // Set the rotation.
        public void SetRotation(float angle)
        {
            this.rotation = MathHelper.WrapAngle(angle); // Wrap the input float value to the rotation.
        }

		/// <summary>
		/// Set the scroll mode.
		/// </summary>
		/// <param name="_scroll">List of behaviors.</param>
		public void SetScrollModes(params ScrollBehavior[] _scroll)
        {
			foreach (ScrollBehavior scroll in _scroll)
			{
				AddScrollBehavior(scroll);
			}
		}
		
		/// <summary>
		/// Set the collision mode.
		/// </summary>
		/// <param name="_collide">List of behaviors.</param>
		public void SetCollisionModes(params CollisionBehavior[] _collide)
		{
			foreach (CollisionBehavior collide in _collide)
			{
				AddCollisionBehavior(collide);
			}
		}

		/// <summary>
		/// Set the entity's position, clamping it to the screen.
		/// </summary>
		/// <param name="_pos">Position to set to.</param>
        public void SetPosition(Vector2 _pos)
        {
            // Create a rectangle for keeping the entity within the screen's boundaries.
            this.position = Clamp(_pos, GetSafeArea());
        }

        #endregion

        #region Service Methods. // Assorted tools and methods.

		/// <summary>
		/// Clamp the input value to a very small float, since Zero cannot be handled by Vector2 objects.
		/// </summary>
		/// <param name="value">Value to clamp.</param>
		/// <returns>Returns validated value.</returns>
        public float ClampZero(float value)
        {
            if(value == 0.0f) { return 0.000000001f; }
            else { return value; }
        }

		/// <summary>
		/// Generate a new direction.
		/// </summary>
		/// <returns></returns>
        public Vector2 GenerateDirection()
        {
            Vector2 baseVector = GeneratePosition(); // We can utilize our GeneratePosition() code to our advantage here, giving us a sizeable vector that we can normalize.
            baseVector.Normalize(); // This gives us a direction length.
            return baseVector;
        }

        public Vector2 GenerateVelocity(float minSpeed, float maxSpeed)
        {
            // Generate a given direction.
            Vector2 dir = GenerateDirection();

            // Generate our magnitudes.
            float mag = InputManager.RNG.Next((int)minSpeed, (int)maxSpeed + 1);

            return dir * mag; // Return a velocity with 
        }

        public Vector2 GeneratePosition()
        {
            return GeneratePosition(GetSafeArea());
        }

        public Vector2 GeneratePosition(Rectangle _bounds)
        {
            // Generate a vector using the range provided. Ensures values are positive.
            float x = InputManager.RNG.Next(_bounds.X, _bounds.X + _bounds.Width);
            float y = InputManager.RNG.Next(_bounds.Y, _bounds.Y + _bounds.Width);

            // Clamp to the safe spawn area.
            return Clamp(new Vector2(x, y), GetSafeArea());
        }

        public Rectangle GetLocationBounds(Vector2 _center, float radius = 0.0f)
        {
            if (radius == 0.0f)
            {
                return new Rectangle((int)_center.X - 1, (int)_center.Y - 1, (int)1, (int)1); // If the rectangle has no boundaries, the bounds should represent the center.
            }

            // Location returns boundaries around a center point determined by a radius.
            return GetLocationBounds(_center, new Vector2(radius, radius));
        }

        public Rectangle GetLocationBounds(Vector2 _center, Vector2 _bounds)
        {
            float _radius = _bounds.Length() / 2;
            
            return new Rectangle((int)(_center.X - _radius), (int)(_center.Y - _radius), (int)(_radius * 2), (int)(_radius * 2)); // If the rectangle has no boundaries, the bounds should represent the center.
        }

        public Rectangle GetSafeArea()
        {
            return GetSafeArea(state.GetScreenBounds());
        }

        public Rectangle GetSafeArea(Vector2 _bounds)
        {
            return GetSafeArea(new Rectangle(0, 0, (int)_bounds.X, (int)_bounds.Y));
        }

        public Rectangle GetSafeArea(Vector2 _pos, Vector2 _bounds)
        {
            return GetSafeArea(new Rectangle((int)_pos.X, (int)_pos.Y, (int)_bounds.X, (int)_bounds.Y));
        }

        // Returns a rectangle that has added this entity's dimensions to its borders.
        public Rectangle GetSafeArea(Rectangle _bounds)
        {
            Point loc = new Point((int)(_bounds.X - this.dimensions.X), (int)(_bounds.Y - this.dimensions.Y));
            Point size = new Point((int)(_bounds.Width + this.dimensions.X), (int)(_bounds.Height + this.dimensions.Y));
            return new Rectangle(loc, size);
        }

        public Vector2 Clamp(Vector2 value, float min, float max)
        {
            return Clamp(value, min, min, max, max);
        }

        public Vector2 Clamp(Vector2 value, float minX, float minY, float maxX, float maxY)
        {
            float x = 0.0f;
            float y = 0.0f;

            if (minX == 0.0f) { minX = 0.00000000000001f; }
            if (minY == 0.0f) { minY = 0.00000000000001f; }
            if (maxX == 0.0f) { maxX = 0.00000000000001f; }
            if (maxY == 0.0f) { maxY = 0.00000000000001f; }

            if (!IsEmpty(value))
            {
                x = value.X;
                y = value.Y;
            }

            x = MathHelper.Clamp(x, minX, maxX);
            y = MathHelper.Clamp(y, minY, maxY);

            return new Vector2(x, y);
        }

        public Vector2 Clamp(Vector2 value, Vector2 minimum, Vector2 maximum)
        {
            return Clamp(value, minimum.X, minimum.Y, maximum.X, maximum.Y);
        }

        public Vector2 Clamp(Vector2 value, Rectangle area)
        {
            return Clamp(value, area.X, area.Y, area.X + area.Width, area.Y + area.Height);
        }

        public Point GetAsPoint(Vector2? _vector = null)
        {
            if (IsEmpty(_vector))
            {
                return new Point(0, 0);
            }
            else
            {
                return new Point((int)((Vector2)_vector).X, (int)((Vector2)_vector).Y);
            }
        }

        public Rectangle GetAsBounds(Vector2? _pos = null, Vector2? _size = null)
        {
            Point location = new Point(0, 0);
            Point size = new Point(0, 0);

            if (!IsEmpty(_pos))
            {
                location = GetAsPoint(_pos);
            }

            if (!IsEmpty(_size))
            {
                size = GetAsPoint(_size);
            }
            
            return new Rectangle(location, size);
        }
        
        protected bool IsEmpty(Rectangle? _bounds = null)
        {
            if(_bounds == null) { return true; }
            return IsEmpty((Rectangle)_bounds);
        }

        protected bool IsEmpty(Rectangle _bounds)
        {
            return (_bounds.X == 0 && _bounds.Y == 0 && _bounds.Width == 0 && _bounds.Height == 0);
        }

        protected bool IsEmpty(Vector2? _vec = null)
        {
            if (_vec == null)
            {
                return true;
            }
            else
            {
                return IsEmpty((Vector2)_vec);
            }
        }

        protected bool IsEmpty(Vector2 _vector)
        {
            return  (((int)_vector.Length() == int.MinValue) || ((int)_vector.X == int.MinValue) || ((int)_vector.Y == int.MinValue) || (float.IsNaN(_vector.Length()) || (float.IsNaN(_vector.X) || float.IsNaN(_vector.Y)) || (_vector.X == 0 && _vector.Y == 0) || (_vector == Vector2.Zero) || (_vector.LengthSquared() == 0) || (_vector.Length() == 0)));
        }

		#endregion

		#region Collision Methods. // These methods deal with the collision and proximity of the entities.

		// Called on collision with something else.
		public virtual void HandleCollisions()
		{
			if (IsCollidable)
			{
				foreach (CollisionBehavior behavior in collisionModes)
				{
					switch (behavior)
					{
						case CollisionBehavior.ColorChange:
							break;
						case CollisionBehavior.Die:
							Kill();
							break;
						case CollisionBehavior.Hurt:
							Hurt();
							break;
						case CollisionBehavior.Null:
							isColliding = false; // This cannot collide.
							break;
					}
				}
			}
		}

		/// <summary>
		/// Add a collision behavior.
		/// </summary>
		/// <param name="behavior">Behavior to add.</param>
		public void AddCollisionBehavior(CollisionBehavior behavior)
		{
			if (!HasCollisionBehavior(behavior))
			{
				collisionModes.Add(behavior);
			}
		}

		/// <summary>
		/// Determines if this entity has the specified collision behavior.
		/// </summary>
		/// <param name="behavior">Behavior to check for.</param>
		/// <returns>Returns true if behavior is included in collisionModes.</returns>
		public bool HasCollisionBehavior(CollisionBehavior behavior)
		{
			foreach (CollisionBehavior mode in collisionModes)
			{
				if (behavior == mode)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Handle collisions when another entity is involved. This allows the other entity to also handle collisions.
		/// </summary>
		/// <param name="e">Entity to check for collisions with.</param>
        public abstract void HandleCollisions(Entity e);
		
		/// <summary>
		/// Checks to see if this entity is within the radius of the other entity.
		/// </summary>
		/// <param name="other">Other entity in proximity.</param>
		/// <returns>Returns true if in proximity.</returns>
		public virtual bool Proximity(Entity other)
        {
            if (Enabled && Visible && IsCollidable)
            {
                // Calculate the distance.
                float distanceSquared = (other.Center - this.Center).LengthSquared();

                // Calculate the radius.
                float radiiSquared = (float)Math.Pow((double)other.Radius + this.Radius, (double)2);

				// If the dist between the points is less than the radii, return true. Else, return false.
				isInProximity = (distanceSquared < radiiSquared);
				return isInProximity;
            }

            // No collision, return false.
            return false;
        }
		
		/// <summary>
		/// Collision does a closer check, utilizing both a AABB and a circle collision check.
		/// </summary>
		/// <param name="other">Other entity colliding.</param>
		/// <returns>Returns true if colliding.</returns>
		public virtual bool Collision(Entity other)
        {
            if (Enabled && Visible && IsCollidable)
            {
                if (Proximity(other))
                {
                    isColliding = (this.Bounds.Intersects(other.Bounds));
					return isColliding;
                }
            }

            // No collision, return false.
            return false;
        }

        /// <summary>
		/// Check if the mouse is over an entity.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity.</returns>
        protected virtual bool MouseOver()
        {
            // Use the InputManager function.
            return InputManager.MouseCollision(this.Bounds);
        }
		
		/// <summary>
		/// Check if the mouse is hovering over an entity.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity.</returns>
		public bool OnHover()
        {
            return InputManager.OnHover(this.Bounds);
        }

		/// <summary>
		/// Check if the mouse just entered over an entity.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity.</returns>
		public bool OnEnter()
        {
            return InputManager.OnEnter(this.Bounds);
        }

		/// <summary>
		/// Check if the mouse just stopped hovering over an entity.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity.</returns>
		public bool OnExit()
        {
            return InputManager.OnExit(this.Bounds);
        }

		/// <summary>
		/// Check if the mouse is over an entity and the left mouse button was pressed.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity and button was clicked.</returns>
		public bool IsClicked()
        {
            // If the mouse is inside the button, and just the left button was just pressed.
            return (MouseOver() && InputManager.LeftButtonPressed);
        }

		/// <summary>
		/// Check if the mouse is over an entity and the left mouse button was held.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity and button was held.</returns>
		public bool IsHeld()
        {
            return (MouseOver() && InputManager.LeftButtonHeld);
        }

		/// <summary>
		/// Check if the mouse is over an entity and the left mouse button was released.
		/// </summary>
		/// <returns>Returns true if mouse is over an entity and button was released.</returns>
		public bool IsReleased()
        {
            if (!MouseOver())
            {
                return false; // If not inside the button, return false.
            }
            else
            {
                return (MouseOver() && InputManager.LeftButtonReleased);
            }
        }

		#endregion

		#region Update Methods. // Update the entity, GUI info, stop the entity, start, reset, or kill.

		// Update, stop, start, kill, and reset methods.
		
		/// <summary>
		/// Start the entity. Called when a state is asked to start.
		/// </summary>
		public abstract void Start();
		
		/// <summary>
		/// Stop the entity. Called when a state is asked to stop.
		/// </summary>
		public abstract void Stop();
		
		/// <summary>
		/// Reset the entity at the start of the game. Called when state needs to reset.
		/// </summary>
		public abstract void Reset();
		
		/// <summary>
		/// Kill the entity. Called when Entity needs to be removed from the state.
		/// </summary>
		public abstract void Kill();

		/// <summary>
		/// Spawn entity in a new, random place.
		/// </summary>
		public abstract void Spawn();

		/// <summary>
		/// Reverse the entity's velocities.
		/// </summary>
		public abstract void Bounce();

		/// <summary>
		/// Harm the entity's health, if possible.
		/// </summary>
		public abstract void Hurt();
		
		/// <summary>
		///  Wrap the entity's edges if it's off-screen.
		/// </summary>
		protected virtual void WrapEdges()
        {
            // Set up values.
            Vector2 screen = state.GetScreenBounds();
            float screenWidth = screen.X;
            float screenHeight = screen.Y;
            bool offScreen = false;

            if (!IsEmpty(position))
            {
                // Screen wrap.
                float x = this.position.X;
                float y = this.position.Y;

                float scale = 0.55f;

                float offsetX = this.dimensions.X * scale;
                float offsetY = this.dimensions.Y * scale;

                float minX = 0 - offsetX;
                float minY = 0 - offsetY;
                float maxX = screenWidth + offsetX;
                float maxY = screenHeight + offsetY;

                if (this.position.X > maxX)
				{
					if (HasScrollBehavior(ScrollBehavior.Null))
					{
						x = maxX;
					}
					else
					{
						x = minX;
					}

					offScreen = true;
				}
                else if (this.position.X < minX)
				{
					if (HasScrollBehavior(ScrollBehavior.Null))
					{
						x = minX;
					}
					else
					{
						x = maxX;
					}

					offScreen = true;
				}

                if (this.position.Y > maxY)
                {
					if (HasScrollBehavior(ScrollBehavior.Null))
					{
						y = maxY;
					}
					else
					{
						y = minY;
					}

					offScreen = true;
				}
                else if (this.position.Y < minY)
                {
					if (HasScrollBehavior(ScrollBehavior.Null))
					{
						y = minY;
					}
					else
					{
						y = maxY;
					}

					offScreen = true;
				}

                if (offScreen)
                {
					if (HasScrollBehavior(ScrollBehavior.Wrap))
					{
						this.position = new Vector2(x, y);
					}
					
					// Handle any additional screenwrap behaviors this entity might have.
					HandleScreenWrap();
                }
            }
            else
            {
                this.position = new Vector2(0, 0);
            }
        }

		/// <summary>
		/// Add a new behavior onto the entity to check for while scrolling.
		/// </summary>
		/// <param name="behavior">Behavior to add.</param>
		protected void AddScrollBehavior(ScrollBehavior behavior)
		{
			if (!scrollModes.Contains(behavior))
			{
				scrollModes.Add(behavior);
			}
		}

		/// <summary>
		/// This is called by child classes for handling actions on screen wrap when wrapping is not the default action.
		/// </summary>
		protected virtual void HandleScreenWrap()
		{
			foreach (ScrollBehavior behavior in scrollModes)
			{
				switch (behavior)
				{
					case ScrollBehavior.Bounce:
						Bounce();
						break;
					case ScrollBehavior.Die:
						Kill();
						break;
					case ScrollBehavior.Spawn:
						Spawn();
						break;
				}
			}
		}

		/// <summary>
		/// Determines if this contains a particular behavior.
		/// </summary>
		/// <param name="behavior">Behavior to check for.</param>
		/// <returns>Returns true if it is in the list.</returns>
		protected bool HasScrollBehavior(ScrollBehavior behavior)
		{
			foreach (ScrollBehavior mode in scrollModes)
			{
				if (behavior == mode)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Handle user input.
		/// </summary>
		protected abstract void HandleInput();


		/// <summary>
		/// Update the entity.
		/// </summary>
		/// <param name="gameTime">Snapshot of elapsed time since last frame.</param>
		public virtual void Update(GameTime gameTime)
		{
			if (Enabled)
			{
				HandleInput();
				Update((float)gameTime.ElapsedGameTime.TotalSeconds);
				WrapEdges();
			}
		}

		/// <summary>
		/// Update with delta.
		/// </summary>
		/// <param name="delta">Total elapsed time since last frame.</param>
		protected abstract void Update(float delta);
		
		/// <summary>
		/// Update GUI information from the entity, if any.
		/// </summary>
		/// <param name="gameTime">Snapshot of elapsed time since last frame.</param>
		public virtual void UpdateGUI(GameTime gameTime)
        {
            if (Enabled)
            {
                UpdateGUI((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
		
		/// <summary>
		/// Update GUI with delta.
		/// </summary>
		/// <param name="delta">Total elapsed time since last frame.</param>
		protected abstract void UpdateGUI(float delta);

        #endregion

        #region Draw Methods. // Draw methods and abstract signatures for child classes to define.
		
		/// <summary>
		/// Draw the entity to the screen, if enabled and visible, and then call <see cref="DrawOverlay"/>. 
		/// </summary>
        public void Draw()
        {
            if (Enabled && Visible)
            {
                GlobalManager.Pen.Pen.Draw(image, Bounds, Source, drawColor, rotation, origin, SpriteEffects.None, 0);
                DrawOverlay();
            }
        }
		
		/// <summary>
		/// DrawOverlay is called after the entity is drawn, just in case children need to draw anything else.
		/// </summary>
		public abstract void DrawOverlay();

        // DrawGUI draws any GUI/HUD information necessary.
        public void DrawGUI()
        {
            if (Enabled && Visible && drawGUI)
            {
                DrawHUD(); // This is drawn everytime.
                DrawDebug(); // This draws only if debug is true.
            }
        }
		
		/// <summary>
		/// DrawDebug is drawn the entity if debug is true
		/// </summary>
		protected virtual void DrawDebug()
        {
            if (Debug)
            {
                DebugLine.DrawLines();
                Vector2 position = new Vector2(10, 10);
                Padding padding = new Padding(0, GlobalManager.Pen.StringHeight("A"));

                StateManager.AddMessage(new Message("Object: [\"" + Tag + "\"]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN),
					new Message("[\"" + Tag + "\"] Bounds: " + Bounds.ToString(), position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN),
					new Message("[\"" + Tag + "\"] Position: " + new Point((int)(Position.X), (int)(Position.Y)) + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN));
            }
        }

        /// <summary>
		/// DrawHUD draws any GUI elements directly attached to the entity, to the screen.
		/// </summary>
        protected abstract void DrawHUD();
        
        #endregion

        #endregion

    }
}
