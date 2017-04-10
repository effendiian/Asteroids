using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Asteroids.Tools;
using Asteroids.Attributes;

namespace Asteroids.Entities
{
    public class Particle : Mover
    {

        // Fields
        private float lifetime; // Total lifespan, in seconds.
        private float ttl; // Life left, in seconds.

        private bool scrollable; // Scrollable.
        private bool start; // Has it started?
        private bool dead;
        private bool spin; // Does it spin?
        
        // Properties.
        public bool Scrollable
        {
            get { return this.scrollable; }
            set { this.scrollable = value; }
        }

        // Constructor.
		public Particle(State _state, Mover _m, float _lifetime, float _radius = 0, float _speed = 1, bool _spin = true)
			: base(_state, _m.Image, _m.Tag, _m.Colorset, _m.Position, _m.Dimensions, _m.Mass, EntityType.Particle)
		{
			this.Initialize(_lifetime, _radius, _speed, _m.Position, _m.Velocity, _spin);
		}

		public Particle(State _state, Texture2D _image, string _tag, ColorSet _col, Vector2? _pos = null, Vector2? _size = null, Vector2? _vel = null, float _mass = 1.0f, float _radius = 0, float _speed = 1, bool _spin = true)
			: base(_state, _image, _tag, _col, _pos, _size, null, _vel, _mass, false, EntityType.Particle)
		{
			this.Initialize(_radius: _radius, _speed: _speed, _pos: _pos, _vel: _vel, _spin: _spin);
		}

		public Particle(State _state, Texture2D _image, string _tag, Color _col, Vector2? _pos = null, Vector2? _size = null, Vector2? _vel = null, float _mass = 1.0f, float _radius = 0, float _speed = 1, bool _spin = true)
			: base(_state, _image, _tag, _col, null, null, null, _pos, _size, null, _vel, _mass, false, EntityType.Particle)
        {
            this.Initialize(_radius: _radius, _speed: _speed, _pos: _pos, _vel: _vel, _spin: _spin);
        }

		public Particle(State _state, Texture2D _image, string _tag, ColorSet _col, Vector2? _pos = null, Vector2? _size = null, Vector2? _vel = null, float _mass = 1.0f, Rectangle? _bounds = null, float _speed = 1, bool _spin = true)
			: base(_state, _image, _tag, _col, _pos, _size, null, _vel, _mass, false, EntityType.Particle)
		{
			this.Initialize(_bounds: _bounds, _speed: _speed, _pos: _pos, _vel: _vel, _spin: _spin);
		}

		public Particle(State _state, Texture2D _image, string _tag, Color _col, Vector2? _pos = null, Vector2? _size = null, Vector2? _vel = null, float _mass = 1.0f, Rectangle? _bounds = null, float _speed = 1, bool _spin = true)
			: base(_state, _image, _tag, _col, null, null, null, _pos, _size, null, _vel, _mass, false, EntityType.Particle)
		{
            this.Initialize(_bounds: _bounds, _speed: _speed, _pos: _pos, _vel: _vel, _spin: _spin);
        }

        public Particle(State _state, Texture2D _image, string _tag, float _radius = 0, float _speed = 1, bool _spin = true)
			: base(_state, _image, _tag, Color.Yellow, null, null, null, null, new Vector2(_radius, _radius), null, null, _radius, false, EntityType.Particle)
        {
            this.Initialize(_radius: _radius, _speed: _speed, _pos: null, _vel: null, _spin: _spin);
        }
        protected virtual void Initialize(float _life = 10, Rectangle? _bounds = null, float _speed = 1, Vector2? _pos = null, Vector2? _vel = null, bool _spin = true)
        {
            // Set flags.
            this.friction = false;
            this.scrollable = false;

            // Set up the lifetime.
            this.ttl = _life;
            this.lifetime = _life;
            this.dead = false;

            Spawn(_pos, _bounds, (int)_life);
            Spin(_spin);
            Push(_vel, _speed);
        }

        protected virtual void Initialize(float _life = 10, float _radius = 0, float _speed = 1, Vector2? _pos = null, Vector2? _vel = null, bool _spin = true)
        {
            // Set flags.
            this.friction = false;
            this.scrollable = false; 

            // Set up the lifetime.
            this.ttl = _life;
            this.lifetime = _life;
            this.dead = false;

            Spawn(_pos, _radius: _radius, _lifetime: (int)_life);
            Spin(_spin);
            Push(_vel, _speed);
        }

        public virtual void Push(Vector2? _vel = null, float _speed = 1)
        {
            Random r = InputManager.RNG;
            Vector2 dir = new Vector2(0, 0);
            float mag = 0.0f;

            // Is velocity null? If so, generate velocity.
            if (_vel == null || IsEmpty((Vector2)_vel))
            {
                mag = r.Next((int)(exSpeed.Maximum + 1));
                float x = InputManager.GetSign() * r.Next(0, (int)GlobalManager.ScreenBounds.X);
                float y = InputManager.GetSign() * r.Next(0, (int)GlobalManager.ScreenBounds.Y);
                dir = Vector2.Normalize(new Vector2(x, y));
            }
            else
            {
                Vector2 vel = (Vector2)_vel; // Store variable temporarily.

                mag = r.Next((int) vel.Length(), (int)(exSpeed.Maximum + 1)); // Get the magnitude. (Minimum speed).
                dir = Vector2.Normalize(vel); // Get the direction. (Get the heading).
            }

            float speed = MathHelper.Clamp(_speed, mag, exSpeed.Maximum); // The speed.
            this.velocity = dir * speed;
        }
        
        public virtual void Spawn(Vector2? _pos = null, Rectangle? _bounds = null, int _lifetime = 10)
        {
            // Set lifetime.
            this.lifetime = _lifetime;
            this.ttl = lifetime;

            // Rectangle: Radius X, Radius Y, Width, Height
            Rectangle empty = new Rectangle(0, 0, 0, 0);

            // If radius == 0, do nothing to modify the position.
            if (_bounds == null || _bounds == empty || _pos == null || IsEmpty((Vector2)_pos))
            {
                Spawn(_pos);
            }
            else
            {
                Rectangle bounds = ((Rectangle)_bounds);

                Random r = InputManager.RNG;
                Vector2 pos = ((Vector2)_pos);

                // Generate values between r, subtract/add based on random sign, and clamp.
                float x = MathHelper.Clamp(r.Next((int)(pos.X - bounds.X), (int)(pos.X + bounds.X) + 1), pos.X + this.Dimensions.X, bounds.Width - this.Dimensions.X);
                float y = MathHelper.Clamp(r.Next((int)(pos.Y - bounds.Y), (int)(pos.Y + bounds.Y) + 1), pos.Y + this.Dimensions.Y, bounds.Height - this.Dimensions.Y);

                this.position = new Vector2(x, y);
            }
        }

        public virtual void Spawn(Vector2? _pos = null, float _radius = 0.0f, int _lifetime = 10)
        {
            // Set lifetime.
            this.lifetime = _lifetime;
            this.ttl = lifetime;

            // If radius == 0, do nothing to modify the position.
            if (_radius == 0.0f || _pos == null || IsEmpty((Vector2)_pos))
            {
                Spawn(_pos);
            }
            else
            {
                Random r = InputManager.RNG;
                Vector2 screen = GlobalManager.ScreenBounds;
                Vector2 pos = (Vector2)_pos;

                // Generate values between r, subtract/add based on random sign, and clamp.
                float x = MathHelper.Clamp(r.Next((int)(pos.X - _radius), (int)(pos.X + _radius) + 1), this.Dimensions.X, screen.X - this.Dimensions.X);
                float y = MathHelper.Clamp(r.Next((int)(pos.Y - _radius), (int)(pos.Y + _radius) + 1), this.Dimensions.Y, screen.Y - this.Dimensions.Y);

                this.position = new Vector2(x, y);
            }
        }

        public virtual void Spawn(Vector2? _pos = null)
        {
            Vector2 screen = GlobalManager.ScreenBounds;
            float x = 0;
            float y = 0;

            // Set the position.
            if (_pos == null || IsEmpty((Vector2)_pos))
            {
                // Generate.
                Random r = InputManager.RNG;

                x = r.Next((int)(1), (int)(screen.X) + 1);
                y = r.Next((int)(1), (int)(screen.Y) + 1);
            }
            else
            {
                x = this.position.X;
                y = this.position.Y;
            }

            // Generate values between r, subtract/add based on random sign, and clamp.
            x = MathHelper.Clamp(x, this.Dimensions.X, screen.X - this.Dimensions.X);
            y = MathHelper.Clamp(y, this.Dimensions.Y, screen.Y - this.Dimensions.Y);

            this.position = new Vector2(x, y);
        }

        public virtual void Spin(bool _spin)
        {
            this.spin = _spin;

            // Initialize angular velocity if spin exists.
            if (spin)
            {
                Random r = InputManager.RNG;

                int signU = InputManager.GetSign();

                float mag = r.Next(0, (int)exAcc.Maximum);
                exAngSpeed.Value += (signU * exAngSpeed.Metric * mag) / (1.5f * Mass);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateGUI(gameTime);

            if (this.Enabled)
            {
                // Get the seconds past.
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (IsKillable())
                {
                    // Subtract delta from time to live.
                    this.ttl -= delta;

                    // Check if still alive.
                    if (ttl <= 0)
                    {
                        Die();
                    }
                }
            }

        }

        protected override void DrawHUD()
        {
            // Rotation.
            Vector2 rotationLine = new Vector2(-this.direction.Y, this.direction.X);
            AddDebugLine(rotationLine, Color.Blue, aVelocity * 7.0f, 7, 1);

            if (Debug)
            {
                DebugLine.DrawLines();
                Point life = new Point((int)ttl, (int)lifetime);
                Vector2 position = new Vector2(10, 10);
                Padding padding = new Padding(0, GlobalManager.Pen.StringHeight("A"));

                StateManager.AddMessage("Object: [\"" + Tag + "\"]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("[\"" + Tag + "\"] Position: " + new Point((int)(Position.X), (int)(Position.Y)) + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("[\"" + Tag + "\"] Speed: " + (int)Speed + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("[\"" + Tag + "\"] Time To Live: [" + life.X + " seconds out of " + life.Y + " seconds]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage("", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
            }

            // Reset the acceleration magnitude tracking value.
            accelerationTrack = new Vector2(0.0f, 0.0f);

            // Draw the heading.
            this.DrawHeading();
        }

        protected virtual bool IsKillable()
        {
            // If lifetime is less than or equal to zero,
            // Then the particle should never be able to die.
            // We treat it like an infinitely existing particle.
            return (this.lifetime > 0) && this.start;
        }

        public override void Start()
        {
            this.start = true;
            this.visible = true;
            this.enabled = true;
            this.dead = false;
            this.ttl = lifetime;
        }

        public virtual void Start(float _life)
        {
            this.lifetime = _life;
            Start();
        }

        public virtual void Die()
        {
            this.start = false;
            this.visible = false;
            this.enabled = false;
            this.dead = true;
            this.ttl = 0;
        }

        public bool IsDead()
        {
            return dead;
        }

        // If outside of edges, kill the particle.
        protected override void WrapEdges()
        {
            if (Scrollable)
            {
                base.WrapEdges();
            }
            else
            {
                // Set up values.
                Vector2 screen = GlobalManager.ScreenBounds;
                float screenWidth = screen.X;
                float screenHeight = screen.Y;

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

                    if ((this.position.X > maxX) || (this.position.X < minX) || (this.position.Y > maxY) || (this.position.Y < minY))
                    {
                        ttl = 0;
                        Die();
                    }
                }
            }
        }
    }
}
