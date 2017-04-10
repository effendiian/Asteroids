using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Tools
{

	// This class is obsolete. Keeping the text for future reference and refactors.

   /* using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;

    using Asteroids.Tools;
    using Asteroids.Entities;
    using Attributes;
    
    public class GlobalParticleGenerator
    {

        // Flags
        private static bool _initialized = false;
        private static bool _debug = false;
        private static bool _drawRectangle = false;
        private static bool _useRadius = false;

        // Fields.
        private static GenerationMode mode;

        private static Vector2 position; // Location of the generator.
        private static float radius; // How far away from the generator's location should particles be emitted?
        private static float scale; // The scale of the particle.

        private static Vector2 point1; // Gripped first point, when mouse is pressed down.
        private static Vector2 point2; // Gripped second point, when mouse is released.
        private static Vector2 point3; // For the boundaries.
        private static List<Particle> particles; // Particles are kept in the list until they are killed. Once dead, they are removed from the list.
        private static List<DebugLine> lines;
        private static Texture2D tex;
        
        // Properties.
        public static bool Initialized
        {
            get { return _initialized; }
            private set { _initialized = value; }
        }
        public static List<Particle> Particles
        {
            get
            {
                if (!Initialized)
                {
                    Initialize(null);
                }

                return particles;
            }            
        }
        
        public static void Initialize(Texture2D _texture, Vector2? _pos = null, float _rad = 1.0f, float _scale = 1.0f)
        {
            tex = _texture;
            scale = _scale;
            SetPosition(_pos, _rad);
            point1 = GlobalManager.ScreenCenter;
            point2 = point1 + new Vector2(1, 1);
            point3 = point1 - new Vector2(1, 1);
            particles = new List<Particle>();
            lines = new List<DebugLine>();
            _initialized = true;
        }

        private static void GenerateGlobal()
        {
            // Use points to get velocity.
            float mag = Math.Abs(Vector2.Distance(point1, point2));

            Particle p = new Particle(GlobalManager.Pen, tex, "Particle (Global)", Color.LightGoldenrodYellow, null, new Vector2(15, 15) * scale, null, 1, radius);
            p.Start(10);
            AddParticle(p);
        }

        private static void GenerateLocal()
        {
            // Use points as velocity and location.
            Vector2 location = point1;
            Vector2 velocity = Vector2.Normalize(point2 - point1);
            float mag = Math.Abs(Vector2.Distance(point1, point2) * scale * (radius + 1.0f) * 10.0f);

            Particle p;

            if (!_useRadius)
            {
                Rectangle bounds = new Rectangle(new Point((int)point1.X, (int)point1.Y), new Point((int)point3.X, (int)point3.Y));
                p = new Particle(this, tex, "Particle (Local)" + location.ToString(), Color.LightGoldenrodYellow, location, new Vector2(15, 15) * scale, velocity, _bounds: bounds, _speed: mag);
            }
            else
            {
                p = new Particle(this, tex, "Particle (Local)" + location.ToString(), Color.LightGoldenrodYellow, location, new Vector2(15, 15) * scale, velocity, radius, mag);
            }
            p.Start(10);
            AddParticle(p);
        }

        private static void ToggleDebug()
        {
            _debug = !_debug;
        }

        public static void HandleInput()
        {
            // Toggle debug information for the particles in the generator.
            if (InputManager.IsKeyReleased(Keys.L))
            {
                ToggleDebug();
            }

            _drawRectangle = false;
            bool keypress = false;

            // If the mouse buttons aren't currently down, do functionality concerning key press generation of particles.
            if (InputManager.LeftButtonUp && InputManager.RightButtonUp)
            {
                // Toggle generation mode for the generator by pressing B.
                if (InputManager.IsKeyReleased(Keys.B))
                {
                    ToggleMode();
                    point1 = GlobalManager.ScreenCenter;
                    point2 = point1 + new Vector2(1, 1);
                    keypress = true;
                }

                // If G is pressed, generate a particle globally.
                if (InputManager.IsKeyHeld(Keys.G))
                {
                    GenerateGlobal();
                    keypress = true;
                }

                // If H is pressed, generate a particle locally, if possible.
                if (InputManager.IsKeyHeld(Keys.H))
                {
                    GenerateLocal();
                    keypress = true;
                }

                // If the > button is pressed, increase the radius.
                if (InputManager.IsKeyHeld(Keys.LeftShift))
                {
                    if (InputManager.IsKeyHeld(Keys.OemPeriod))
                    {
                        radius = MathHelper.Clamp(radius + 0.1f, 0.1f, 1000f);
                        keypress = true;
                    }

                    if (InputManager.IsKeyHeld(Keys.OemComma))
                    {
                        radius = MathHelper.Clamp(radius - 0.1f, 0.1f, 1000f);
                        keypress = true;
                    }
                }
            }

            // If G, H, B, are not down, then do functionality concerning mouse generation of particles.
            if(!keypress)
            {
                bool left = false;

                // The distance between the two points creates the magnitude for the velocity.
                // If left button is pressed, get current mouse position for point a.
                if (InputManager.LeftButtonPressed)
                {
                    // Global Mode: Point 1 is start of velocity vector.
                    // Local Mode: Point 1 is start of velocity vector, Point 1 is origin of the particle.
                    Point a = InputManager.GetMousePosition();
                    point1 = new Vector2(a.X, a.Y);
                    left = true;
                }
                else if (InputManager.LeftButtonHeld && InputManager.IsKeyHeld(Keys.LeftShift))
                {
                    // While being held, keep track of the current mouse position for the debug line.
                    Point d = InputManager.GetMousePosition();
                    point3 = new Vector2(d.X, d.Y);

                    _useRadius = false;
                    _drawRectangle = true;                
                    left = true;
                }
                else if (InputManager.LeftButtonHeld && !InputManager.IsKeyHeld(Keys.LeftShift))
                {
                    // While being held, keep track of the current mouse position for the debug line.
                    Point b = InputManager.GetMousePosition();
                    point2 = new Vector2(b.X, b.Y);

                    Vector2 difference = point2 - point1;
                    lines.Add(new DebugLine(point1, difference, Color.White, difference.Length(), 3, 1));

                    _useRadius = true;
                    left = true;
                }
                else if ((InputManager.IsKeyHeld(Keys.LeftShift) && (InputManager.LeftButtonReleased)) || (InputManager.IsKeyReleased(Keys.LeftShift) && InputManager.LeftButtonReleased))
                {
                    Point d = InputManager.GetMousePosition();
                    point3 = new Vector2(d.X, d.Y);
                    left = true;
                }
                // On release of the left mouse button, get current mouse position for point b.
                else if (InputManager.LeftButtonReleased)
                {
                    // Global Mode: Point 2 is end of velocity vector.
                    // Local Mode: Point 2 is end of velocity vector.
                    Point b = InputManager.GetMousePosition();
                    point2 = new Vector2(b.X, b.Y);
                    left = true;
                }
                
                if (InputManager.RightButtonReleased && !left)
                {
                    // If the right mouse button is released, generate a point.
                    // Generate particle with right mouse click.
                    // Generate a point depending on the mode.
                    switch (mode)
                    {
                        case GenerationMode.Global:
                            GenerateGlobal();
                            break;
                        case GenerationMode.Local:
                            GenerateLocal();
                            break;
                    }
                }
            }            
        }

        public static void Update(GameTime gameTime)
        {
            if (Initialized)
            {
                // Handle input.
                HandleInput();

                if (!IsEmpty())
                {
                    List<Particle> toRemove = new List<Particle>();

                    foreach (Particle particle in Particles)
                    {
                        particle.Debug = _debug;
                        particle.Update(gameTime);

                        if (particle.IsDead())
                        {
                            toRemove.Add(particle);
                        }
                    }

                    foreach (Particle particle in toRemove)
                    {
                        RemoveParticle(particle);
                    }
                }
            }
        }

        public static void Draw()
        {
            if (Initialized)
            {
                if (!IsEmpty())
                {
                    foreach (Particle particle in Particles)
                    {
                        particle.Draw();
                    }
                }
            }
        }

        public static void DrawGUI()
        {
            if (Initialized)
            {
                if (_drawRectangle)
                {
                    int distX = (int)(point3.X - point1.X);
                    int distY = (int)(point3.Y - point1.Y);
					GlobalManager.Pen.DrawRectOutline((int)point1.X, (int)point1.Y, distX, distY, Color.White);
                }

                if (_debug)
                {

                    Padding padding = new Padding(0, GlobalManager.Pen.StringHeight("A"));
                    Vector2 position = new Vector2(10, 10 + padding.Y);

                    switch (mode)
                    {
                        case GenerationMode.Global:
                            StateManager.AddMessage("[Global Particle Manager] [Global Mode] [" + NumberOfParticles() + " particles]", position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                            break;
                        case GenerationMode.Local:
                            StateManager.AddMessage("[Global Particle Manager] [Local Mode] [" + NumberOfParticles() + " particles]", position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                            break;
                    }

                    float mag = Math.Abs(Vector2.Distance(point1, point2) * scale * (radius + 1.0f) * 10.0f);

                    // ABSOLUTE VALUE NEEDS TO BE FIXED

                    StateManager.AddMessage("[Points] <" + point1 + ", " + point2 + ">", position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                    StateManager.AddMessage("[Bounds] <" + new Rectangle((int)point1.X, (int)point1.Y, (int)(point3.X - point1.X), (int)(point3.Y - point1.Y)) + ">", position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                    StateManager.AddMessage("[Radius] " + radius, position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                    StateManager.AddMessage("[Scale] " + scale, position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                    StateManager.AddMessage("[Magnitude] " + mag, position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                    StateManager.AddMessage("", position, padding, Color.LimeGreen, 0, ShapeDrawer.LEFT_ALIGN);
                } 

                foreach (DebugLine line in lines)
                {
                    DebugLine.Draw(line, true);
                }

                lines = new List<DebugLine>();

                if (!IsEmpty())
                {
                    foreach (Particle particle in Particles)
                    {
                        particle.DrawGUI();
                    }
                }
            }
        }

        public static void Start()
        {
            foreach (Particle particle in Particles)
            {
                Birth(particle);
            }
        }

        public static void Start(float time)
        {
            foreach (Particle particle in Particles)
            {
                Birth(particle, time);
            }
        }

        public static void Stop()
        {
            foreach (Particle particle in Particles)
            {
                Kill(particle);
            }
        }

        public static void Birth(Particle particle)
        {
            particle.Start();
        }

        public static void Birth(Particle particle, float time)
        {
            particle.Start(time);
        }

        public static void Kill(Particle particle)
        {
            particle.Die();
        }

        public static void RemoveParticle(Particle particle)
        {
            if (!IsEmpty())
            {
                List<Particle> p = Particles;

                if (p.Contains(particle))
                {
                    particle.Die();
                    p.Remove(particle);
                }
            }

        }

        public static void AddParticle(Particle particle)
        {
            Particles.Add(particle);
        }

        public static void SetPosition(Vector2? _pos = null, float _rad = 1.0f)
        {
            if (_pos == null || ((Vector2)(_pos)) == Vector2.Zero)
            {
                // If null, then it's global.
                mode = GenerationMode.Global;
            }
            else
            {
                position = (Vector2)_pos;
                radius = _rad;
                mode = GenerationMode.Local;
            }
        }

        public static void ToggleMode()
        {
            switch (mode)
            {
                case GenerationMode.Global:
                    mode = GenerationMode.Local;
                    break;
                case GenerationMode.Local:
                    mode = GenerationMode.Global;
                    break;
            }
        }

        public static bool IsEmpty()
        {
            if (Initialized)
            {
                return (NumberOfParticles() == 0);
            }

            // If not initialized, it's empty.
            return true;
        }

        public static int NumberOfParticles()
        {
            if (Initialized)
            {
                return particles.Count();
            }
            else
            {
                return 0;
            }
        }

        public static void SetScale(float _scale)
        {
            scale = Math.Abs(_scale);

            foreach (Particle particle in Particles)
            {
                particle.SetScale(scale);
            }
        }

    }
	*/
}
