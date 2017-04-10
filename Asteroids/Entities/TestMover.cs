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
    public class TestMover : Mover
    {
        public bool PlayerControl
        {
            get;
            set;
        }

        public TestMover(State _state, Mover _m) 
			:base(_state, _m.Image, _m.Tag, _m.Colorset, _m.Position, _m.UnscaledDimensions, _m.Mass, EntityType.Test)
        {
            this.position = _m.Position;
        }
		
		public TestMover(State _state, Texture2D _image, Color? draw = null, Color? hover = null, Color? collide = null, Color? disable = null, Vector2? _pos = null, Vector2? _size = null, float _mass = 1.0f)
			: base(_state, _image, "!Test", draw, hover, collide, disable, _pos, _size, null, null, _mass, true, EntityType.Test)
		{
			if (_pos == null)
			{
				Randomize();
			}
			else
			{
				this.position = Clamp((Vector2)_pos, GetSafeArea());
			}
		}

		public TestMover(State _state, Texture2D _image, ColorSet _col, Vector2? _pos = null, Vector2? _size = null, float _mass = 1.0f)
			:base(_state, _image, "!Test", _col, _pos, _size, _mass, EntityType.Test)
        {
            if (_pos == null)
            {
                Randomize();
            }
            else
            {
                this.position = Clamp((Vector2)_pos, GetSafeArea());
			}
        }

        public TestMover(State _state, Texture2D _image) 
			:base(_state, _image, "!Test", Color.LimeGreen, null, null, null, null, null, EntityType.Test)
        {
            this.Randomize();
        }
        
        protected override void Initialize(Vector2? _acc = default(Vector2?), Vector2? _vel = default(Vector2?), float _mass = 1, bool _friction = true)
        {
            base.Initialize(_acc, _vel, _mass, _friction);

			exAcc = new Extents(8f, 0.07f, false, 0.04f);
            exSpeed = new Extents(1000.0f, 3f, false, 0.31f);
            gears = new Gears(10, 0, 1000f);
        }

        protected override void CreateControlScheme()
        {
            base.CreateControlScheme();

                // Add to schema.
                // Forward
                AssignControl(Commands.Forward, Keys.Up);
                AssignControl(Commands.Forward, Keys.W);

                // Debug.
                AssignControl(Commands.Debug, Keys.I);

                // Toggle friction mode.
                AssignControl(Commands.FrictionMode, Keys.F);

                // Randomize
               AssignControl(Commands.Randomize, Keys.R);

                // Brake.
                AssignControl(Commands.Brake, Keys.Down);
                AssignControl(Commands.Brake, Keys.S);

                // Turn Left.
                AssignControl(Commands.RotateLeft, Keys.A);

                // Turn Right.
                AssignControl(Commands.RotateRight, Keys.D);

                // Rotate Left.
                AssignControl(Commands.TurnLeft, Keys.Left);

                // Rotate Right.
                AssignControl(Commands.TurnRight, Keys.Right);
            
        }

        protected override void DrawHUD()
        {
            base.DrawHUD();
            
            // Rotation.
            Vector2 rotationLine = new Vector2(-this.direction.Y, this.direction.X);
            AddDebugLine(rotationLine, Color.Blue, aVelocity * 7.0f, 7, 1);

            if (Debug)
            {
                DebugLine.DrawLines();
                Vector2 position = new Vector2(10, 10);
                Padding padding = new Padding(0, GlobalManager.Pen.StringHeight("A"));

                //StateManager.AddMessage("Object: [\"" + Tag + "\"]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                //StateManager.AddMessage("[\"" + Tag + "\"] Position: " + new Point((int)(Position.X), (int)(Position.Y)) + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN);
                StateManager.AddMessage(new Message("[\"" + Tag + "\"] Speed: [" + (int)Speed + " pixels/second]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN),
						new Message("[\"" + Tag + "\"] Velocity: " + new Point((int)(Velocity.X), (int)(Velocity.Y)) + "", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN),
						new Message("[\"" + Tag + "\"] Acceleration: " + new Point((int)(accelerationTrack.X), (int)(accelerationTrack.Y)) + " pixels/second^2", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN),
						new Message("[\"" + Tag + "\"] Angular Speed: [" + (int)MathHelper.ToDegrees(AngularSpeed) + " degrees/second] | [" + (int)AngularSpeed + " radians/second]", position, padding, drawColor, 0, ShapeDrawer.LEFT_ALIGN));
            }

            // Reset the acceleration magnitude tracking value.
            accelerationTrack = new Vector2(0.0f, 0.0f);

            // Draw the pointer
            this.DrawHeading();
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            if (this.Enabled && PlayerControl)
            {
                bool resetMetrics = true;

                if (InputManager.RightButtonHeld && MouseOver() && Debug)
                {
                    // Drag entity if in debug mode.
                    Point pos = InputManager.GetMousePosition();
                    this.Position = new Vector2(pos.X, pos.Y);
                }

                if (InputManager.IsKeyReleased(schema, Commands.FrictionMode))
                {
                    this.ToggleFrictionMode();
                }

                if (InputManager.IsKeyReleased(schema, Commands.Randomize))
                {
                    this.Randomize();
                }
                
                if (InputManager.IsKeyHeld(schema, Commands.TurnLeft) && !InputManager.IsKeyHeld(schema, Commands.TurnRight))
                {
                    if (Speed < 15f)
                    {
                        this.Throttle();
                    }

                    if (Speed > 1.1f)
                    {
                        this.TurnLeft();
                        resetMetrics = false;
                    }
                }

                if (InputManager.IsKeyHeld(schema, Commands.TurnRight) && !InputManager.IsKeyHeld(schema, Commands.TurnLeft))
                {
                    if (Speed < 15f)
                    {
                        this.Throttle();
                    }

                    if (Speed > 1.1f)
                    {
                        this.TurnRight();
                        resetMetrics = false;
                    }
                }

                if (InputManager.IsKeyHeld(schema, Commands.Forward))
                {
                    this.Throttle();
                    resetMetrics = false;
                }

                if (resetMetrics)
                {
                    this.exAcc.SetValue(exAcc.Metric);
                }

                if (InputManager.IsKeyHeld(schema, Commands.Brake))
                {
                    this.Brake();
                }


                if (InputManager.IsKeyHeld(schema, Commands.RotateLeft) && !InputManager.IsKeyHeld(schema, Commands.TurnLeft))
                {
                    this.RotateCounterClockwise();
                }

                
                if (InputManager.IsKeyHeld(schema, Commands.RotateRight) && !InputManager.IsKeyHeld(schema, Commands.TurnRight))
                {
                    this.RotateClockwise();
                }

                if (InputManager.IsKeyReleased(schema, Commands.Debug))
                {
                    this.debug = !debug;
                }
            }
        }

        // Reset.
        public override void Reset()
        {
            base.Reset();
            this.Stop();
            this.Randomize();
        }

        // Randomize; for testing.
        public void Randomize()
        {
            // Randomizes position, and adds a random initial acceleration.
            Vector2 screen = GlobalManager.ScreenBounds;
            screen = new Vector2(screen.X - 25, screen.Y - 25);
            Random rng = InputManager.RNG;

            int maxX = (int)(screen.X - dimensions.X) + 1;
            int maxY = (int)(screen.Y - dimensions.Y) + 1;
            int minX = (int)(dimensions.X);
            int minY = (int)(dimensions.Y);

            float x = rng.Next(minX, maxX);
            float y = rng.Next(minY, maxY);

            int signU = InputManager.GetSign();
            int signV = InputManager.GetSign();

            if (signU == 0) { signU = -1; }
            if (signV == 0) { signV = -1; }

            this.position = new Vector2(x, y);

            float u = rng.Next(0, (int)exAcc.Maximum);
            float v = rng.Next(0, (int)exAcc.Maximum);
            Vector2 randomForce = new Vector2(signU * u, signV * v);
            exAngSpeed.Value += (signU * exAngSpeed.Metric * u) / (1.5f * Mass);

            float mag = rng.Next(0, (int)exSpeed.Maximum);
            randomForce.Normalize();
            randomForce *= mag;

            this.velocity = new Vector2(0, 0);
            this.velocity = randomForce;

            if (!PlayerControl)
            {
                this.friction = false;
            }
            else
            {
                this.friction = true;
            }
            
        }
        
    }
}
