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

namespace Asteroids.Entities
{

    // EntityType enum: Player, Asteroid, Test, Star, Particle.
    // CollisionBehavior enum: ColorChange, Die, Hurt, Null.
    // ScrollBehavior enum: Die, Spawn, Wrap

    #region Enums. // EntityType, CollisionBehavior, and ScrollBehavior are located here.

    // Enums for the types of entity.
    public enum EntityType
    {
        Player,
        Asteroid,
        Test,
        Star,
        Particle
    }

    // Collision behavior.
    public enum CollisionBehavior
    {
        ColorChange,
        Die,
        Hurt,
        Null
    }

    // Scroll behavior for the entity.
    public enum ScrollBehavior
    {
        Die,
        Spawn,
        Wrap
        // Bounce
    }

    #endregion

    // An entity is any object with a texture,
    // that has a position on screen, a tint color,
    // and boundaries that can deal with collision with
    // other entities.
    public abstract class Entity
    {
        #region Fields.

        // Entity archival information.
        protected string tag; // Name of the entity.
        protected EntityType type; // The entity type.

        // Items used for drawing or entity management.
        protected State state; // The state this entity resides in.
        protected ShapeDrawer pen;
        protected Texture2D image;
        
        // Items used for tracking location, rotation, and size.
        protected Vector2 position;
        protected Vector2 origin;
        protected Vector2 dimensions;
        protected Vector2 originalDimensions; // The "1.0" scale base size.
        protected Vector2 imageDimensions; // The image dimensions. ("The source").
        protected float rotation; // Rotation of the entity.
        protected float scale = 1.0f; // The scale of an entity.
        
        // Items used for tracking the drawn color.
        protected Color drawColor; // The current color to draw with.
        protected ColorSet colorSet; // The set of colors the entity is drawn with.
        
        // Collision behaviors with other entities and with the screen.
        protected ScrollBehavior scrollMode; // What should the entity do when off-screen.
        protected CollisionBehavior collideMode; // What should the entity do when hit.

        // Dealing with user key input.
        protected ControlScheme schema; // Control scheme used to affect entities in different ways.

        #region Flags.

        protected bool enabled = false; // Enabled/Disabled flag.
        protected bool visible = false; // Visibility flag.
        protected bool draggable = false; // Can it be dragged to begin with.
        protected bool dragged = false; // Is it being dragged?
        protected bool debug = false; // Debug flag.
        protected bool drawGUI = false; // Draw GUI flag.
        protected bool isInProximity = false; // If something is close to this entity, the state will set this value to true.
        protected bool isColliding = false; // If something is colliding with this entity, the state will set this value to true.
        
        #endregion

        #endregion

        #region Properties.

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public bool Disabled
        {
            get { return !this.enabled; }
            set { this.enabled = !value; }
        }

        public ScrollBehavior ScrollMode
        {
            get { return this.scrollMode; }
            set { this.scrollMode = value; }
        }

        public bool Scrollable
        {
            get { return (ScrollMode == ScrollBehavior.Wrap); }
        }

        public CollisionBehavior CollisionMode
        {
            get { return this.collideMode; }
            set { this.collideMode = value; }
        }

        public bool CollisionsOn
        {
            get { return (CollisionMode != CollisionBehavior.Null); }
        }

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public bool Debug
        {
            get { return this.debug; }
            set { this.debug = value; }
        }

        public EntityType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string Tag
        {
            get { return this.tag; }
            set { this.tag = value; }
        }
        
        public bool HasImage
        {
            get { return (image == null); }
        }

        public Texture2D Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        // Position.
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        // The x-coordinate.
        public float X
        {
            get { return this.position.X; }
            set { this.position = new Vector2(value, this.position.Y); }
        }

        // The y-coordinate.
        public float Y
        {
            get { return this.position.Y; }
            set { this.position = new Vector2(this.position.X, value); }
        }

        // Dimensions.
        public Vector2 Dimensions
        {
            get { return this.dimensions; }
            set { this.dimensions = value; }
        }

        // Readonly.
        public virtual Vector2 Center
        {
            get { return this.position; }
        }

        // The radius.
        public float Radius
        {
            // The radius is the largest dimension.
            // Eg. If height < width, this will returnt the width.
            get { return this.dimensions.Length(); }
        }

        // The width.
        public float Width
        {
            get { return this.dimensions.X; }
            set { this.dimensions = new Vector2(value, this.dimensions.Y); }
        }

        // The height.
        public float Height
        {
            get { return this.dimensions.Y; }
            set { this.dimensions = new Vector2(this.dimensions.X, value); }
        }

        // Rotation of the entity.
        public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        // The boundaries of the entity. Also known as the display rectangle.
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - (int)(dimensions.X / 2), (int)position.Y - (int)(dimensions.Y / 2), (int)dimensions.X, (int)dimensions.Y); }
        }

        // Source rectangle.
        public Rectangle Source
        {
            get { return new Rectangle(0, 0, image.Width, image.Height); }
        }

        // Color to draw the entity.
        public Color DrawColor
        {
            get { return this.drawColor; }
        }

        #endregion

        #region Constructor

        /// An entity, at the minimum, needs a state object and a texture to be created.
        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, Vector2? _size = null,
            float _rotation = 0f, Color? _drawColor = null,
            Color? _hoverColor = null, Color? _collisionColor = null,
            Color? _disableColor = null,
            ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _draggable = false, bool _drawGUI = false,
           ControlScheme _schema = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, new ColorSet(_drawColor, _hoverColor, _collisionColor, _disableColor), _scroll, _collision, _enabled, _visible, _draggable, _drawGUI, _schema);
        }

        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, Vector2? _size = null,
            float _rotation = 0f, ColorSet _set = null,
            ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _draggable = false, bool _drawGUI = false,
           ControlScheme _schema = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, _set, _scroll, _collision, _enabled, _visible, _draggable, _drawGUI, _schema);
        }
        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, float _size = 0.0f,
            float _rotation = 0f, Color? _drawColor = null,
            Color? _hoverColor = null, Color? _collisionColor = null,
            Color? _disableColor = null,
            ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _draggable = false, bool _drawGUI = false,
            ControlScheme _schema = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, new ColorSet(_drawColor, _hoverColor, _collisionColor, _disableColor), _scroll, _collision, _enabled, _visible, _draggable, _drawGUI, _schema);
        }

        public Entity(State _state, Texture2D _image, string _tag = "Entity [Default]",
            Vector2? _pos = null, float _size = 0.0f,
            float _rotation = 0f, ColorSet _set = null,
            ScrollBehavior _scroll = ScrollBehavior.Wrap,
            CollisionBehavior _collision = CollisionBehavior.Null,
            bool _enabled = false, bool _visible = false,
            bool _draggable = false, bool _drawGUI = false,
            ControlScheme _schema = null)
        {
            Initialize(_state, _image, _tag, _pos, _size, _rotation, _set, _scroll, _collision, _enabled, _visible, _draggable, _drawGUI, _schema);
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
            bool _draggable = false, bool _drawGUI = false,
           ControlScheme _schema = null)
        {
            SetState(_state); // Store the current state.
            SetImage(_image); // File the image to draw with.
            SetTag(_tag); // Add a tag for the element.
            SetColorSet(_set); // Set the colors we'd like to use.
            SetDimensions(_size); // By default, base the size off of the image's picture.
            SetRotation(_rotation); // By default this will be 0, but, we can customize on creation. Useful for spawning.
            SetScrollMode(_scroll); // Wrap entity by default.
            SetCollisionMode(_collision); // Do not collide by default.
            SetFlags(_enabled, _visible, _draggable, _drawGUI); // Set these flags up by default.
            CreateControlScheme(_schema);
        }

        protected void Initialize(State _state,
           Texture2D _image, string _tag = "Entity [Default]",
           Vector2? _pos = null, float _size = 0.0f,
           float _rotation = 0f, ColorSet _set = null,
           ScrollBehavior _scroll = ScrollBehavior.Wrap,
           CollisionBehavior _collision = CollisionBehavior.Null,
           bool _enabled = false, bool _visible = false,
           bool _draggable = false, bool _drawGUI = false,
           ControlScheme _schema = null)
        {
            SetState(_state); // Store the current state.
            SetImage(_image); // File the image to draw with.
            SetTag(_tag); // Add a tag for the element.
            SetColorSet(_set); // Set the colors we'd like to use.
            SetDimensions(_size); // By default, base the size off of the image's picture.
            SetRotation(_rotation); // By default this will be 0, but, we can customize on creation. Useful for spawning.
            SetScrollMode(_scroll); // Wrap entity by default.
            SetCollisionMode(_collision); // Do not collide by default.
            SetFlags(_enabled, _visible, _draggable, _drawGUI); // Set these flags up by default.
            CreateControlScheme(_schema);
        }

        protected virtual void SetFlags(bool _enabled = false, bool _visible = false, bool _draggable = false, bool _drawGUI = false)
        {
            // Set these flags.
            this.enabled = _enabled;
            this.visible = _visible;
            this.draggable = _draggable;
            this.drawGUI = _drawGUI;

            // Initialize these flags.
            this.debug = false;
            this.dragged = false;
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

        // Wrapper function. Add new keys to the schema.
        protected void AssignControl(Commands command, List<Keys> keys)
        {
            this.schema.AssignKeys(command, keys);
        }

        // Wrapper function. Add a new key to the schema.
        protected void AssignControl(Commands command, Keys key)
        {
            this.schema.AssignKey(command, key);
        }

        #endregion
        
        #region Mutator Methods. // Set data using these. Get data, usually, through properties.

        // Sets the state the entity is in and gets the pen to draw with.
        private void SetState(State _state)
        {
            // Assigning the state to a field.
            this.state = _state;

            // Attributes that can be called from the state.
            this.pen = state.GetPen();
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

        // Set the scroll mode.
        public void SetScrollMode(ScrollBehavior _scroll)
        {
            this.scrollMode = _scroll;
        }

        // Set the collision mode.
        public void SetCollisionMode(CollisionBehavior _collide)
        {
            this.collideMode = _collide;
        }

        public void SetPosition(Vector2 _pos)
        {
            // Create a rectangle for keeping the entity within the screen's boundaries.
            this.position = Clamp(_pos, GetSafeArea());
        }

        #endregion

        #region Service Methods. // Assorted tools and methods.

        public float ClampZero(float value)
        {
            if(value == 0.0f) { return 0.000000001f; }
            else { return value; }
        }

        public Vector2 GenerateDirection()
        {
            Vector2 baseVector = GeneratePosition(); // We can utilize our GeneratePosition() code to our advantage here, giving us a sizeable vector that we can normalize.
            baseVector.Normalize(); // This gives us a direction length.
            return baseVector;
        }

        public Vector2 GenerateVelocity()
        {
            return GenerateVelocity(exSpeed.Minimum, exSpeed.Maximum);
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
            return  (((int)_vector.Length() == int.MinValue) || ((int)_vector.X == int.MinValue) || ((int)_vector.Y == int.MinValue) || (float.IsNaN(_vector.Length()) || (float.IsNaN(_vector.X) || float.IsNaN(_vector.Y)) || (_vector.X == 0 && _vector.Y == 0) || (_vector == Vector2.Zero) || (_vector.LengthSquared() == 0) || (_vector.Length() == 0));
        }

        #endregion

        #region Collision Methods. // These methods deal with the collision and proximity of the entities.
        
        // Called on collision with something else.
        public abstract void HandleCollisions();
        public abstract void HandleCollisions(Entity e);

        // Checks to see if it's within the radius of the other entity.
        public virtual bool Proximity(Entity other)
        {
            if (Enabled && Visible && CollisionsOn)
            {
                // Calculate the distance.
                float distanceSquared = (other.Center - this.Center).LengthSquared();

                // Calculate the radius.
                float radiiSquared = (float)Math.Pow((double)other.Radius + this.Radius, (double)2);

                // If the dist between the points is less than the radii, return true. Else, return false.
                return (distanceSquared < radiiSquared);
            }

            // No collision, return false.
            return false;
        }

        // Collision does a closer check, utilizing both a AABB and a circle collision check.
        public virtual bool Collision(Entity other)
        {
            if (Enabled && Visible && CollisionsOn)
            {
                if (Proximity(other))
                {
                    return (this.Bounds.Intersects(other.Bounds));
                }
            }

            // No collision, return false.
            return false;
        }

        // Mouse cursor.
        protected virtual bool MouseOver()
        {
            // Use the InputManager function.
            return InputManager.MouseCollision(this.Bounds);
        }

        // Is the button being hovered over?
        public bool OnHover()
        {
            return InputManager.OnHover(this.Bounds);
        }
        // Did the mouse just enter over the button?
        public bool OnEnter()
        {
            return InputManager.OnEnter(this.Bounds);
        }

        // Did the mouse just exit from over the button?
        public bool OnExit()
        {
            return InputManager.OnExit(this.Bounds);
        }

        // Is the button just pressed?
        public bool IsClicked()
        {
            // If the mouse is inside the button, and just the left button was just pressed.
            return (MouseOver() && InputManager.LeftButtonPressed);
        }

        // Is the button held?
        public bool IsHeld()
        {
            return (MouseOver() && InputManager.LeftButtonHeld);
        }

        // Is the button just released?
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

        // Start the entity. Called when a state is asked to start.
        public abstract void Start();

        // Stop the entity. Called when a state is asked to stop.
        public abstract void Stop();

        // Reset the entity at the start of the game. Called when state needs to reset.
        public abstract void Reset();

        // Kill the entity. Called when Entity needs to be removed from the state.
        public abstract void Kill();

        // Update the entity.
        public virtual void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                HandleInput();
                Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                WrapEdges();
            }
        }

        // Wrap the entity's edges if it's off-screen.
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
                    x = minX;
                    offScreen = true;
                }
                else if (this.position.X < minX)
                {
                    x = maxX;
                    offScreen = true;
                }

                if (this.position.Y > maxY)
                {
                    y = minY;
                    offScreen = true;
                }
                else if (this.position.Y < minY)
                {
                    y = maxY;
                    offScreen = true;
                }

                if (offScreen)
                {
                    if (Scrollable)
                    {
                        this.position = new Vector2(x, y);
                    }
                    else
                    {
                        HandleScreenWrap();
                    }
                }
            }
            else
            {
                this.position = new Vector2(0, 0);
            }
        }

        // Handle any input.
        protected abstract void HandleInput();

        // This is called by child classes for handling actions on screen wrap when wrapping is not the default action.
        protected abstract void HandleScreenWrap();
        
        // Update with delta.
        protected abstract void Update(float delta);

        // Update GUI information from the entity, if any.
        public virtual void UpdateGUI(GameTime gameTime)
        {
            if (Enabled)
            {
                UpdateGUI((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        // Update GUI with delta.
        protected abstract void UpdateGUI(float delta);

        #endregion

        #region Draw Methods. // Draw methods and abstract signatures for child classes to define.

        // Draw the entity to the screen.
        public void Draw()
        {
            if (Enabled && Visible)
            {
                pen.Pen.Draw(image, Bounds, Source, drawColor, rotation, origin, SpriteEffects.None, 0);
                DrawOverlay();
            }
        }

        // DrawOverlay is called after the entity is drawn, just in case children need to draw anything else.
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

        // DrawDebug is drawn the entity if debug is true.
        protected virtual void DrawDebug()
        {
            if (Debug)
            {
                DebugLine.DrawLines();
                Vector2 position = new Vector2(10, 10);
                Padding padding = new Padding(0, pen.StringHeight("A"));

                StateManager.AddMessage("Object: [\"" + Tag + "\"]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("[\"" + Tag + "\"] Bounds: " + Bounds.ToString(), position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("[\"" + Tag + "\"] Position: " + new Point((int)(Position.X), (int)(Position.Y)) + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
            }
        }

        // DrawHUD is drawn by children classes.
        protected abstract void DrawHUD();
        
        #endregion

        #endregion

    }
}
