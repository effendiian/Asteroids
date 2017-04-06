using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Asteroids.Tools;
using Asteroids.Attributes;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Entities
{
    public class Asteroid : Mover
    {
        // Maximums.
        public const int MAX_LEVEL = 3;
        public const int MAX_HEALTH = 50; // Can't have more than 50 total health points.
        public const int MAX_VALUE = 500; // Can't get more than 500 points from one asteroid.

        // Size limits.
        public const int MAX_SIZE = 500;

        // Minimums.
        public const int MIN_LEVEL = 1; // If less than this, clamp.
        public const int MIN_HEALTH = 1; // If less than this, clamp.

        // Base.
        public const int BASE_SIZE = 35; // Size for calculating dimensional limits.
        public const int BASE_HEALTH = 1; // Can't start with less than 1 health point.
        public const int BASE_VALUE = 5; // Can't be worth less than 5 points.

        // Fields.
        // Game attributes.
        private int level; // Also determines scale of the asteroid.
        private int points; // The number of points an asteroid is worth.
        private int health; // The health an asteroid has.
        private int currentHealth; // Current health an asteroid has.

        private bool dead; // Is it alive? Or is it dead?
        private bool scrollable; // Is it destroyed after exiting the screen?
        
        // Properties.
        public int Level
        {
            get { return this.level; }
        }

        public int Value
        {
            get { return this.points; }
        }

        public int Health
        {
            get { return this.health; }
        }

        public bool Scrollable
        {
            get { return this.scrollable; }
            set { this.scrollable = value; }
        }

        public bool Dead
        {
            get { return this.dead; }
        }
        
        // Constructors.
        public Asteroid(ShapeDrawer _pen, Texture2D _image, string _tag) : base(_pen, _image, _tag, Color.White, EntityType.Asteroid)
        {
            // An asteroid without a level should start at the highest level.
            Initialize();
        }

        public Asteroid(ShapeDrawer _pen, Texture2D _image, string _tag,
            Vector2? _pos = null, Vector2? _vel = null, float _radius = 0,
            int _level = MAX_LEVEL, int _health = MAX_HEALTH, int _value = BASE_VALUE, bool _scrollable = false) : base(_pen, _image, _tag, Color.White, EntityType.Asteroid)
        {
            Initialize(_pos, _vel, _radius, _level, _health, _value, _scrollable);
        }

        public Asteroid(Asteroid parent) : base(parent.pen, parent.image, parent.tag, parent.DrawColor, EntityType.Asteroid)
        {
            // Use the parent's values to initialize.
            Initialize(parent.position, parent.velocity, parent.Radius, parent.level - 1, parent.health, parent.points, parent.scrollable);
        }
        
        protected override void CreateControlScheme()
        {
            base.CreateControlScheme();

            // Add to schema.
            // For changing the speed.
            schema.AssignKey(Commands.Increment, Keys.Add);
            schema.AssignKey(Commands.Decrement, Keys.Subtract);

            // For changing the health.
            schema.AssignKey(Commands.Hurt, Keys.T);
            schema.AssignKey(Commands.Heal, Keys.Y);

        }
        
        protected override void HandleInput()
        {
            base.HandleInput();

            if (InputManager.IsKeyReleased(schema, Commands.Hurt))
            {
                Hurt();
            }

            if (InputManager.IsKeyReleased(schema, Commands.Heal))
            {
                Heal();
            }

            if (InputManager.IsKeyReleased(schema, Commands.Decrement))
            {
                DecreaseSpeedLimit();
            }

            if (InputManager.IsKeyReleased(schema, Commands.Increment))
            {
                IncreaseSpeedLimit();
            }
        }

        // Methods.
        protected void Initialize(Vector2? _pos = null, Vector2? _vel = null, float _radius = 0.0f, int _level = MAX_LEVEL, int _health = MAX_HEALTH, int _value = BASE_VALUE, bool _scrollable = false)
        {
            // Check if level is less than zero.
            if (_level < MIN_LEVEL)
            {
                Die();
                return;
            }

            if (_health < MIN_HEALTH)
            {
                Die();
                return;
            }
            
            SetLevel(_level);
            SetHealth(_health);
            SetValue(_value);

            this.friction = false;
            this.scrollable = _scrollable;
        }

        public void IncreaseSpeedLimit(float increment = 2.0f)
        {
            float currentLimit = exSpeed.Maximum + Math.Abs(increment);
            LimitSpeed(currentLimit);
        }
        public void DecreaseSpeedLimit(float decrement = 2.0f)
        {
            float currentLimit = exSpeed.Maximum - Math.Abs(decrement);
            LimitSpeed(currentLimit);
        }

        public void Hurt(float damage = 10.0f)
        {
            currentHealth -= (int)Math.Abs(damage);
        }

        public void Heal(float damage = 10.0f)
        {
            currentHealth += (int)Math.Abs(damage);
        }

        public void LimitSpeed(float x)
        {
            // Don't ever allow an asteroid to go over this value.
            float speed = MathHelper.Clamp(x, 0, 500);

            SetExtentsMaximum(exSpeed, x);
        }

        protected override void WrapEdges()
        {
            if (Scrollable)
            {
                base.WrapEdges();
            }
            else
            {
                // If not scrollable, spawn in a new position.

                // Set up values.
                Vector2 screen = Game1.ScreenBounds;
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
                        Spawn();
                        Size();
                        Push();
                        Spin();
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!dead)
            {
                base.Update(gameTime);

                if (currentHealth < MIN_HEALTH)
                {
                    Die();
                    return;
                }

                if (currentHealth > health)
                {
                    currentHealth = health;
                }
            }
        }

        public override void UpdateGUI(GameTime gameTime)
        {
            if (!dead)
            {
                base.UpdateGUI(gameTime);

                // Update health.
                if (currentHealth < 0)
                {
                    Die();
                }

                // Update level.

                // Update value.
            }
        }

        public override void DrawGUI()
        {
            if (!dead)
            {
                base.DrawGUI();

                Padding padding = new Padding(0, Game1.Small.MeasureString("A").Y);
                Vector2 pos = this.position + new Vector2(0, this.dimensions.Y / 2); // + new Vector2(0, padding.Y); //  new Vector2(0, this.dimensions.Y) 

                int a = ShapeDrawer.CENTER_ALIGN;

                Message.DrawMessage(new Message("Level: " + level, pos, padding, drawColor, 0, a), Game1.Small);
                Message.DrawMessage(new Message("Health: " + currentHealth + " / " + health + " hp.", pos, padding, drawColor, 1, a), Game1.Small);

                pos = this.position - new Vector2(this.dimensions.X / 2, this.dimensions.Y / 2) - new Vector2(0, padding.Y * 2);

                float numerator = (float)currentHealth;
                float denominator = (float)health;
                float portion = numerator / denominator;
                
                float width = portion * this.dimensions.X;

                pen.DrawRectOutline((int)pos.X, (int)pos.Y, (int)this.dimensions.X, (int)padding.Y, Color.Red);
                pen.DrawRectFilled((int)pos.X, (int)pos.Y, (int)width, (int)padding.Y, Color.Red);
            }
        }


        public void Spawn(Vector2? _pos = null, float _radius = 0.0f)
        {
            this.currentHealth = health;

            Random r = InputManager.RNG;
            Vector2 pos = Game1.ScreenCenter;

            if (IsEmpty(_pos))
            {
                // Generate new position.
                int c = r.Next(0, 4);

                // Get the width and height.
                int width = (int)this.Dimensions.X;
                int height = (int)this.Dimensions.Y;
                int screenWidth = (int)Game1.ScreenBounds.X;
                int screenHeight = (int)Game1.ScreenBounds.Y;

                Rectangle left = new Rectangle(0, 0, width, screenHeight);
                Rectangle top = new Rectangle(0, 0, screenWidth, height);
                Rectangle bottom = new Rectangle(0, screenHeight - height, screenWidth, screenHeight);
                Rectangle right = new Rectangle(screenWidth - width, 0, screenWidth, screenHeight);

                Rectangle spawn = left;

                switch (c)
                {
                    case 0:
                        spawn = left;
                        break;
                    case 1:
                        spawn = right;
                        break;
                    case 2:
                        spawn = top;
                        break;
                    default:
                    case 3:
                        spawn = bottom;
                        break;
                }

                float u = InputManager.GetSign() * _radius;
                float v = InputManager.GetSign() * _radius;

                float x = r.Next(spawn.X, spawn.X + spawn.Width) + u;
                float y = r.Next(spawn.Y, spawn.Y + spawn.Height) + v;

                pos = new Vector2(x, y);
            }
            else
            {
                // Assign this position.
                pos = (Vector2)_pos;
            }

            this.position = pos;
        }

        public void Spin()
        {
            Random r = InputManager.RNG;
            exAngSpeed.Value += (InputManager.GetSign() * exAngSpeed.Metric) / (1.5f * Mass);
        }

        public void Spin(Vector2? _vel = null)
        {
            if (IsEmpty(_vel))
            {
                Spin();
            }
            else
            {
                Vector2 vel = (Vector2)_vel;
                float mag = ((Vector2)_vel).X;
                exAngSpeed.Value += (exSpeed.Metric * mag) / (1.5f * Mass);
            }
        }

        public void Push(Vector2? _vel = null, float _mag = -1.0f)
        {
            Vector2 dir;

            Random r = InputManager.RNG;
            float x = r.Next(0, (int)Game1.ScreenBounds.X);
            float y = r.Next(0, (int)Game1.ScreenBounds.Y);
            
            if (IsEmpty(_vel))
            {
                dir = Vector2.Normalize(new Vector2(x, y));
            }
            else
            {
                dir = Vector2.Normalize(new Vector2(x, y));
                dir = Vector2.Normalize((Vector2)_vel + (dir * InputManager.GetSign()));
            }

            float mag = Math.Abs(_mag);

            if (_mag == -1.0f)
            {
                mag = r.Next(100, 500);
            }

            this.velocity = dir * mag;
        }

        public void Size(float _size = -1.0f)
        {
            float dim = MathHelper.Clamp(_size, 1, MAX_SIZE);

            if (_size == -1.0f)
            {
                int min = BASE_SIZE * level;
                int max = (min * 2 + points) + 1;

                dim = MathHelper.Clamp(InputManager.RNG.Next(min, max), min, max);
                mass = 10.0f * (dim / MAX_SIZE);
            }

            this.originalDimensions = new Vector2(dim / 2, dim / 2);
            this.dimensions = originalDimensions * scale;
        }

        public override void Reset()
        {
            base.Reset();

            this.currentHealth = health;
            this.dead = false;
            this.enabled = true;
            this.visible = true;

            Spawn();
            Size();
            Spin();
            Push();
        }

        private void Die()
        {
            this.currentHealth = 0;
            this.dead = true;
            this.enabled = false;
            this.visible = false;
            Stop();
        }

        private void SetLevel(int _level)
        {
            this.level = MathHelper.Clamp(_level, MIN_LEVEL, MAX_LEVEL);
        }

        private void SetValue(int _value)
        {
            this.points = MathHelper.Clamp(_value * this.level, BASE_VALUE, MAX_VALUE);
        }

        private void SetHealth(int _health)
        {
            this.health = MathHelper.Clamp(_health * this.level, BASE_HEALTH, MAX_HEALTH);
        }




    }
}
