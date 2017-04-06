using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Monogame using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Asteroid using statments.
using Asteroids.Tools;
using Asteroids.Attributes;

namespace Asteroids.Entities
{
    public class Mover : Entity
    {
        // Constants.
        public const int DEFAULT_MAX_SPEED = 1000f;
        public const int DEFAULT_MAX_ACCEL = 10f;


        #region Fields. // Items that are unique to movers deal with motion and tracking it.

















        // Fields
        protected Extents exAcc = new Extents(-25f, 25f, 0.0f, 0.04f, 5f);
        protected Extents exSpeed = new Extents(-1000.0f, 1000.0f, 0.0f, 0.31f, 3f);
        protected Extents exAngSpeed = new Extents(-7.0f, 7.0f, 0.0f, 0.05f, 0.2f);

        protected float mew = 0.01f;

        protected Vector2 direction;
        protected Vector2 acceleration;
        protected Vector2 accelerationTrack;
        protected Vector2 velocity;
        
        protected float mass;
        protected float aVelocity;

        protected Gears gears;
        
        protected float delta = 0.0f;

        protected List<DebugLine> debugLine;

        protected bool friction;

        protected ControlScheme schema;

        // Properties.
        public float Mass
        {
            get { return this.mass; }
        }

        public float Speed
        {
            get
            {
                if (!IsEmpty(velocity))
                {
                    return this.velocity.Length();
                }
                else
                {
                    return 0;
                }
            }
        }

        public float AngularSpeed
        {
            get
            {
                return this.aVelocity;
            }
        }

        public Vector2 Direction
        {
            get {
                this.direction.Normalize();
                return this.direction;
            }
        }

        public Vector2 Velocity
        {
            get {
                if (!IsEmpty(velocity))
                {
                    return this.velocity;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public Vector2 Acceleration
        {
            get {
                if (!IsEmpty(acceleration))
                {
                    return this.acceleration;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public Vector2 Friction
        {
            get
            {
                if (!IsEmpty(velocity) && friction)
                {
                    Vector2 dir = Vector2.Normalize(velocity);
                    float mag = velocity.Length();
                    float normal = -1.0f * 0.86f * (mew * mass);

                    if (exSpeed.CloseToZero(mag))
                    {
                        return Vector2.Zero;
                    }
                    else
                    {
                        return Vector2.Multiply(dir, mag * normal);
                    }
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public float MaxSpeed
        {
            get
            {
                return GetExtentsMaximum(exSpeed);
            }

            set
            {
                SetExtentsMaximum(exSpeed, value);
            }
        }
        
        public float MaxAcceleration
        {
            get
            {
                return GetExtentsMaximum(exAcc);
            }

            set
            {
                SetExtentsMaximum(exAcc, value);
            }
        }

        public float MaxAngularSpeed
        {
            get
            {
                return GetExtentsMaximum(exAngSpeed);
            }

            set
            {
                SetExtentsMaximum(exAngSpeed, value);
            }
        }


        // Constructor
        public Mover(ShapeDrawer _pen, Texture2D _image, EntityType _type) : base(_pen, "Mover Entity", _image, null, new Vector2(100, 100), Color.White, false, true, true, _type)
        {
            Initialize(null, null);
        }

        public Mover(ShapeDrawer _pen, Texture2D _image, string _tag, EntityType _type = EntityType.Test) : base(_pen, _tag, _image, null, new Vector2(100, 100), Color.White, false, true, true, _type)
        {
            Initialize(null, null);
        }

        public Mover(ShapeDrawer _pen, Texture2D _image, string _tag, Color color, EntityType _type = EntityType.Test) : base(_pen, _tag, _image, null, new Vector2(100, 100), color, false, true, true, _type)
        {
            Initialize(null, null);
        }

        public Mover(ShapeDrawer _pen, Texture2D _image, string _tag, Color color, Vector2? _pos = null, Vector2? _size = null, EntityType _type = EntityType.Test) : base(_pen, _tag, _image, _pos, _size, color, false, true, true)
        {
            Initialize(null, null);
        }

        public Mover(ShapeDrawer _pen, Texture2D _image, string _tag, Color color, Vector2? _pos = null, Vector2? _size = null, float _mass = 1.0f, EntityType _type = EntityType.Test) : base(_pen, _tag, _image, _pos, _size, color, false, true, true)
        {
            Initialize(null, null, _mass);
        }

        public Mover(ShapeDrawer _pen, Texture2D _image, string _tag, Color color, Vector2 _pos, Vector2 _size, Vector2? _acc = null, Vector2? _vel = null, float _mass = 1.0f, bool _friction = true, EntityType _type = EntityType.Test) : base(_pen, _tag, _image, _pos, _size, color, false, true, true, _type)
        {
            Initialize(_acc, _vel, _mass, _friction);
        }

        protected void AddDebugLine(Vector2 vector, Color color, float magnitude = 10.0f, float thickness = 1f, float order = 1f)
        {
            if (Debug)
            {
                DebugLine.CreateDebugLine(this, vector, color, magnitude, thickness, (int)order);
            }
        }
        
        protected virtual void Initialize(Vector2? _acc = null, Vector2? _vel = null, float _mass = 1.0f, bool _friction = true)
        {
            // Set default values.
            this.direction = new Vector2(0, 1); // Right.
            this.acceleration = new Vector2(0, 1); // Not moving.
            this.velocity = new Vector2(0, 1); // Not moving.
            this.debugLine = new List<DebugLine>(); // Debug Lines.

            // Assign values.
            this.mass = Math.Abs(_mass);
            this.friction = _friction;
            this.gears = new Gears();

            if (_acc != null && ((Vector2)_acc) != Vector2.Zero)
            {
                this.acceleration = (Vector2)_acc;
            }

            if (_vel != null && ((Vector2)_vel) != Vector2.Zero)
            {
                this.velocity = (Vector2)_vel;
            }

            CreateControlScheme();
        }

        protected virtual void CreateControlScheme()
        {
            this.schema = new ControlScheme();
            // Add keys to schema.
        }
        

        // Helper method; determines if vector is empty.
        protected bool IsEmpty(Vector2 vec)
        {
            if (vec == null)
            {
                return true;
            }
            else if (vec == Vector2.Zero || vec.LengthSquared() == 0 || vec.Length() == 0 || vec.Length() == float.NaN || (int)vec.Length() == int.MinValue || vec.X == float.NaN || vec.Y == float.NaN)
            {
                return true;
            }

            return false;
        }

        // If controlled by the player, input is handled here.
        protected virtual void HandleInput()
        {
            // Input gets handled here if the entity requires it.
        }

        protected virtual void HandleAccelerationBehavior()
        {
            // Any miscellaneous functions that aren't user input.
        }

        protected virtual void HandleRotation()
        {
            // Acceleration affects rotation.
            // float aAcceleration = acceleration.X / (10.0f * Mass);
            // exAngSpeed.Value += aAcceleration;

            // Angular friction:

            if (friction)
            {
                float aFriction = mew * -1.0f * exAngSpeed.Value;
                exAngSpeed.Value += aFriction;
            }
                        
            if (exAngSpeed.InCenterRange())
            {
                exAngSpeed.Value = 0.0f;
            }

            aVelocity = exAngSpeed.Value;
            exAngSpeed.Value = exAngSpeed.Clamp(exAngSpeed.Value);
            aVelocity = exAngSpeed.Clamp(aVelocity);
            rotation += aVelocity * delta;

            rotation = MathHelper.WrapAngle(rotation);
        }

        // Set the heading.
        protected void UpdateDirection()
        {
            // Calculate direction based on velocity.
            if (!IsEmpty(velocity))
            {
                this.direction = Vector2.Normalize(this.velocity);
                AddDebugLine(this.direction, Color.Purple, 55f, 4f, 1);
            }
        }

        // Update the acceleration.
        protected void UpdateAcceleration()
        {
            if (IsEmpty(acceleration)) { this.acceleration = new Vector2(0f, 0f); }

            // Handle input here.
            this.HandleInput();

            // Handle any acceleration behaviors.
            this.HandleAccelerationBehavior();

            // Apply the forces here.
            this.ApplyForce(Friction); // Apply the frictional force. (Look at the Friction property for more information).

            // Calculate rotation.
            this.HandleRotation();

            // Add acceleration debug line.
            AddDebugLine(this.acceleration, Color.Red, 35.0f, 3, 2);

            // Clamp acceleration.
            this.ClampAcceleration();
        }

        // Clamp the acceleration.
        private void ClampAcceleration()
        {
            if (IsEmpty(acceleration)) { this.acceleration = new Vector2(0f, 0f); return; }

            // Clamp the vector.
            float mag = exAcc.Clamp(acceleration.Length()); // Auto-clamps value.
            accelerationTrack = Vector2.Multiply(Vector2.Normalize(acceleration), mag);

            if (exAcc.CloseToZero(mag))
            {
                acceleration = new Vector2(0, 0);
            }
            else
            {
                acceleration.Normalize();
                acceleration *= mag;
            }
        }

        // Update the velocity.
        private void UpdateVelocity()
        {
            if (IsEmpty(velocity))
            {
                this.velocity = new Vector2(0, 0);
            }

            if (!IsEmpty(acceleration))
            {
                this.velocity += acceleration;
                AddDebugLine(this.velocity, Color.Green, 35.0f, 5, 2);
                ClampVelocity();
            }
        }

        // Clamp the velocity.
        private void ClampVelocity()
        {
            if (IsEmpty(velocity)) { this.velocity = new Vector2(0f, 0f); return; }

            // Clamp the vector.
            float mag = exSpeed.Clamp(velocity.Length()); // Auto-clamps value.

            if (exSpeed.CloseToZero(mag))
            {
                velocity = new Vector2(0, 0);
            }
            else
            {
                velocity.Normalize();
                velocity *= mag;
            }
        }
        
        // Update the position.
        private void UpdatePosition()
        {
            if (IsEmpty(position))
            {
                this.position = new Vector2(0, 0);
            }

            if (!IsEmpty(velocity))
            {
                this.position += velocity * delta;
            }

            WrapEdges();
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                // Update the movement of the entity.
                // If a vector is zero, refresh it so it doesn't corrupt.
                if (IsEmpty(velocity)) { this.velocity = new Vector2(0f, 0f); }

                delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
                
                UpdateAcceleration();
                UpdateVelocity();
                UpdateDirection();
                UpdatePosition();

                // Clear the acceleration every time.
                this.acceleration = new Vector2(0, 0);
            }
        }

        public void ToggleFrictionMode()
        {
            SetFrictionMode(!friction);
        }

        private void SetFrictionMode(bool fMode)
        {
            this.friction = fMode;
        }

        // Force methods.
        public virtual void Throttle()
        {
            float gear = (gears.GetGear(Math.Abs(exSpeed.Clamp(velocity.Length()))) / gears.GearCount) * 100.0f;
            float throttle = gear * exAcc.Metric;
            
            Vector2 dir = new Vector2(this.direction.X, this.direction.Y);
            dir.Normalize();
            ApplyForce(dir * exAcc.Value);
            exAcc.SetValue(exAcc.Value + (exAcc.Metric + throttle));
        }

        public virtual void Brake()
        {
            if (!IsEmpty(velocity))
            {
                Vector2 dir = Vector2.Normalize(velocity);
                float mag = 0.0001f;

                if (Speed < 6f)
                {
                    velocity = Vector2.Normalize(velocity) * mag;
                }
                else
                {
                    // Get a negative magnitude.
                    mag = acceleration.Length();
                    mag *= -1.0f;

                    // Apply a force opposing the velocity, that is larger than the current acceleration forces.
                    ApplyForce(dir * mag, exAcc.Maximum);
                }
            }

            exAngSpeed.Value = exAngSpeed.Value * 0.68f;
        }

        public void TurnLeft()
        {
            if (!IsEmpty(velocity))
            {
                float weight = 4.0f / Mass;
                Vector2 dir = new Vector2(this.velocity.Y, -this.velocity.X);
                dir.Normalize();
                float mag = 0.0001f;

                if (Speed < 6f)
                {
                    ApplyForce(dir * mag, Mass);
                }
                else
                {
                    // Get a magnitude based on the current acceleration.
                    mag = exAcc.Clamp(acceleration.Length() * weight) + exAcc.Value;

                    // Apply a force that is either the maximum acceleration or the current acceleration plus the weight value in addition to the current acceleration.
                    ApplyForce(dir * mag, Mass);
                }
            }

            exAcc.SetValue(exAcc.Value + exAcc.Metric);
            exAngSpeed.Value += (-1 * exAngSpeed.Metric) / (1.5f * Mass) * (Speed / Mass);
        }

        public void TurnRight()
        {

            if (!IsEmpty(velocity))
            {
                float weight = 4.0f / Mass;
                Vector2 dir = new Vector2(-this.velocity.Y, this.velocity.X);
                dir.Normalize();
                float mag = 0.0001f;

                if (Speed < 6f)
                {
                    ApplyForce(dir * mag, Mass);
                }
                else
                {
                    // Get a magnitude based on the current acceleration.
                    mag = exAcc.Clamp(acceleration.Length() * weight) + exAcc.Value;

                    // Apply a force that is either the maximum acceleration or the current acceleration plus the weight value in addition to the current acceleration.
                    ApplyForce(dir * mag, Mass);
                }
            }

            exAcc.SetValue(exAcc.Value + exAcc.Metric);
            exAngSpeed.Value += (exAngSpeed.Metric) / (1.5f * Mass) * (Speed / Mass);
        }

        public void RotateCounterClockwise()
        {
            exAngSpeed.Value += -1 * exAngSpeed.Metric;
        }

        public void RotateClockwise()
        {
            exAngSpeed.Value += exAngSpeed.Metric;
        }

        public void Stop()
        {
            acceleration = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
        }

        public void ApplyForce(Vector2 f)
        {
            if (!IsEmpty(f))
            {
                Vector2 force = Vector2.Divide(f, Mass); // Copy the force and divide by the mass.
                if (force.Length() > 0.05f)
                {
                    if (IsEmpty(acceleration))
                    {
                        this.acceleration = new Vector2(0, 0);
                    }
                    
                    this.acceleration += force;
                    this.accelerationTrack += force;

                    // Add a debug line keeping track of the force applied.
                    AddDebugLine(force, Color.HotPink, 40.0f, 8, 3);
                }
            }
        }

        public void ApplyForce(Vector2 f, float weight)
        {
            // Apply a force with a weight.
            ApplyForce(f * weight);
        }

        public override void SetScale(float scale)
        {
            base.SetScale(scale);
            this.mass = this.Mass * scale;
        }

        protected virtual void DrawHeading()
        {
            // Draw the pointer
            // AddDebugLine(this.direction, Color.Purple, 55f, 4f, 1);
            int thick = (int)MathHelper.Clamp(Speed / 10f, 5, 15);
            Vector2 marker = this.position + (this.direction * (int)MathHelper.Clamp(Speed, this.Radius, this.Radius * 4.0f));
            pen.DrawRectOutlineAroundPoint((int)marker.X, (int)marker.Y, (int)(thick * scale), Color.Purple);

            int mark = (int)MathHelper.Clamp((thick - 2), 3, 14);
            pen.DrawRectAroundPoint((int)marker.X, (int)marker.Y, (int)(mark * scale), Color.Purple);
        }

        public float GetExtentsMaximum(Extents e)
        {
            return e.Maximum;
        }

        public void SetExtentsMaximum(Extents e, float value)
        {
            float positive = Math.Abs(value);
            float negative = -1 * positive;
            e = new Extents(negative, positive, e.Value, e.Midpoint, e.Metric);
        }
    }
}
