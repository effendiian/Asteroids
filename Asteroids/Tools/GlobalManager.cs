/// GlobalManager.cs - Version 1
/// Author: Ian Effendi
/// Date: 4.8.2017
/// File Description: Contains reference to the class.

#region Using statements.

// Import system library.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MonoGame calls.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Asteroid calls.
using Asteroids.Attributes;
using Asteroids.Entities;

#endregion

namespace Asteroids.Tools
{

    #region Enums. // Contains definition for the FontIDs enum.

    /// <summary>
    /// FontIDs are the type of IDs to be used when drawing strings.
    /// </summary>
    public enum FontIDs
    {
        /// <summary>
        /// The Main font is the primary font used in must instances of drawing text.
        /// </summary>
        Main,

        /// <summary>
        /// The Small font is the same as the Main font, but, at a smaller size.
        /// </summary>
        Small
    }

    #endregion

    /// <summary>
    /// GlobalManager is used to access global values,
    /// and objects, like the random number generator, 
    /// and the sprite batch object.
    /// It also manages any textures that need to be
    /// shared across resources.
    /// </summary>
    public class GlobalManager
    {
        #region Fields. // Contains private fields for all useable textures, all useable fonts, and global control scheme.

        /// <summary>
        /// A collection of useable <see cref="Texture2D"/>  files, each associated with a given <see cref="TextureIDs"/> id. 
        /// </summary>
        private static Dictionary<TextureIDs, Texture2D> textures;

        /// <summary>
        /// A collection of useable <see cref="SpriteFont"/>  files, each associated with a given <see cref="FontIDs"/> id. 
        /// </summary>
        private static Dictionary<FontIDs, SpriteFont> fonts;

        /// <summary>
        /// A global control scheme that listens for general keystrokes that are consistently read throughout the entire program.
        /// </summary>
        private static ControlScheme keyListener;

        #endregion

        #region Flags. // Contains flags for different states, such as initialization and debug status.

        private static bool _initialized = false;
        private static bool _debug = false;

        #endregion

        // Properties.
        public static Game1 Main
        {
            get;
            private set;
        }
        
        public static ContentManager Content
        {
            get;
            private set;
        }

        public static SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        public static ShapeDrawer Pen
        {
            get;
            private set;
        }

        public static GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }

        public static Vector2 ScreenBounds
        {
            get;
            private set;
        }
        
        public static Vector2 ScreenCenter
        {
            get { return ScreenBounds * 0.5f; }
        }

        public static Vector2 DisplayBounds
        {
            get { return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); }
        }

        public static Random Random
        {
            get;
            private set;
        }

        public static bool Initialized
        {
            get { return _initialized; }
            private set { _initialized = value; }
        }

        public static bool Debug
        {
            get { return _debug; }
            private set { _debug = value; }
        }
        
        // Methods.

        // Initialization.
        public static void Initialize(Game1 _main, GraphicsDeviceManager _graphics)
        {
            Main = _main;
            Graphics = _graphics;
            Content = _main.Content;
            UpdateScreen(800, 600);

            fonts = new Dictionary<FontIDs, SpriteFont>();
            textures = new Dictionary<TextureIDs, Texture2D>();

            StateManager.Initialize();
            InputManager.Initialize();

            // This random is the same reference to the RNG in the InputManager.
            Random = InputManager.RNG;

            keyListener = new ControlScheme();

            _initialized = true;
        }

        private static void CreateControlScheme()
        {
            keyListener.Bind(Commands.Debug, Keys.OemTilde, ActionType.Released);
            keyListener.Bind(Commands.Quit, Keys.Delete, ActionType.Released);
        }

        // Load.
        private static void Load(SpriteBatch _sb)
        {
            // Set up the shape drawer.
            SpriteBatch = _sb;
            Pen = new ShapeDrawer(_sb, fonts[FontIDs.Main], Main.GraphicsDevice);

            // Set up the shapes.
            StateManager.CreateStates(SpriteBatch, Pen, textures);
        }

        public static void Load(SpriteBatch _sb, Dictionary<TextureIDs, Texture2D> _textures, List<TextureIDs> _textureIDs, Dictionary<FontIDs, SpriteFont> _fonts, List<FontIDs> _fontIDs)
        {
            // Add the fonts.
            AddFonts(_fontIDs, _fonts);

            // Add the textures.
            AddTextures(_textureIDs, _textures);
            
            // Load the state and shape drawer.
            Load(_sb);
        }

        public static void Load(SpriteBatch _sb, Dictionary<TextureIDs, string> _texturePaths, List<TextureIDs> _textureIDs, Dictionary<FontIDs, string> _fontPaths, List<FontIDs> _fontIDs)
        {
            // Add the fonts.
            AddFonts(_fontIDs, _fontPaths);

            // Add the textures.
            AddTextures(_textureIDs, _texturePaths);

            // Load the state and shape drawer.
            Load(_sb);
        }

        public static void AddTextures(List<TextureIDs> ids, Dictionary<TextureIDs, string> path)
        {
            foreach (TextureIDs id in ids)
            {
                AddTexture(id, path[id]);
            }
        }

        public static void AddTextures(List<TextureIDs> ids, Dictionary<TextureIDs, Texture2D> _textures)
        {
            foreach (TextureIDs id in ids)
            {
                AddTexture(id, _textures[id]);
            }
        }

        public static void AddFonts(List<FontIDs> ids, Dictionary<FontIDs, string> path)
        {
            foreach (FontIDs id in ids)
            {
                AddFont(id, path[id]);
            }
        }

        public static void AddFonts(List<FontIDs> ids, Dictionary<FontIDs, SpriteFont> _fonts)
        {
            foreach (FontIDs id in ids)
            {
                AddFont(id, _fonts[id]);
            }
        }

        public static void AddTexture(TextureIDs id, string path)
        {
            Texture2D texture = Content.Load<Texture2D>(path);
            AddTexture(id, texture);
        }

        public static void AddTexture(TextureIDs id, Texture2D texture)
        {
            if (textures.ContainsKey(id))
            {
                textures[id] = texture;
            }
            else
            {
                textures.Add(id, texture);
            }
        }

        public static void AddFont(FontIDs id, string path)
        {
            SpriteFont font = Content.Load<SpriteFont>(path);
            AddFont(id, font);
        }

        public static void AddFont(FontIDs id, SpriteFont font)
        {
            if (fonts.ContainsKey(id))
            {
                fonts[id] = font;
            }
            else
            {
                fonts.Add(id, font);
            }
        }

        private static void HandleGlobalInput()
        {
            Debug = keyListener.IsFired(Commands.Debug);
        }

        public static void Update(GameTime gameTime)
        {
            if (StateManager.GetState() == States.Quit || keyListener.IsFired(Commands.Quit))
            {
                Main.Exit();
            }

            if (Initialized)
            {
                // Handle input.
                InputManager.Update(gameTime);
                HandleGlobalInput();

                // Handle state.
                StateManager.Update(gameTime);
            }
        }

        public static void UpdateGUI(GameTime gameTime)
        {
            if (Initialized)
            {
                StateManager.UpdateGUI(gameTime);
            }
        }

        public static void DrawBackground()
        {
            Graphics.GraphicsDevice.Clear(StateManager.Background);
        }

        public static void Draw()
        {
            if (Initialized)
            {
                StateManager.Draw();
            }
        }

        public static void DrawGUI()
        {
            if (Initialized)
            {
                StateManager.DrawGUI(Pen);
            }
        }

        private static void DrawTestGUI()
        {
            int height = (int)Pen.StringDimensions("A").Y;

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y - height, Color.White, "This is a left test.", ShapeDrawer.LEFT_ALIGN);

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y, Color.White, "This is a center test.", ShapeDrawer.CENTER_ALIGN);

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y + height, Color.White, "This is a right test.", ShapeDrawer.RIGHT_ALIGN);

            Pen.DrawString(0, 0, Color.White, "This is a default test.");
        }


        // Update dimension variables.
        public static void UpdateScreen(float screenWidth, float screenHeight)
        {
            Graphics.PreferredBackBufferWidth = (int)Math.Abs(screenWidth);
            Graphics.PreferredBackBufferHeight = (int)Math.Abs(screenHeight);
            Graphics.ApplyChanges();

            ScreenBounds = new Vector2(screenWidth, screenHeight);

            // Reset window position.
            int w = Graphics.PreferredBackBufferWidth;
            int h = Graphics.PreferredBackBufferHeight;

            int dW = (int)DisplayBounds.X;
            int dH = (int)DisplayBounds.Y;

            if (w != dW)
            {
                Graphics.IsFullScreen = false;
                Main.Window.Position = new Point(((int)(dW / 2) - ((int)ScreenCenter.X)), ((int)(dH / 2) - ((int)ScreenCenter.Y)));
            }
            else
            {
                Graphics.IsFullScreen = true;
            }

            Graphics.ApplyChanges();
        }

        // Update dimension variables.
        public void UpdateScreen()
        {
            UpdateScreen(Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight);
        }


    }
}
