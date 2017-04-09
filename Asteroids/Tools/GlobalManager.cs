/// GlobalManager.cs - Version 2
/// Author: Ian Effendi
/// Date: 4.8.2017
/// File Description: Contains reference to the class.

#region Using statements.

// Import system library.
using System;
using System.Collections.Generic;

// MonoGame calls.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Asteroid calls.
// using Asteroids.Attributes;
// using Asteroids.Entities;

#endregion

namespace Asteroids.Tools
{

    #region Enums. // Contains definition for the FontIDs and TextureIDs enums.

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


    /// <summary>
    /// TextureIDs are the type of IDs to be used when drawing images/sprites.
    /// </summary>
    public enum TextureIDs
    {
        /// <summary>
        /// The base of the button.
        /// </summary>
        Button,

        /// <summary>
        /// The player spaceship sprite.
        /// </summary>
        Player,

        /// <summary>
        /// Diamond bullet sprite. 
        /// </summary>
        Bullet,

        /// <summary>
        /// One of the asteroid sprites.
        /// </summary>
        Asteroid1,

        /// <summary>
        /// Alternative asteroid sprite.
        /// </summary>
        Asteroid2,

        /// <summary>
        /// Alternative asteroid sprite.
        /// </summary>
        Asteroid3,

        /// <summary>
        /// Test entity texture.
        /// </summary>
        Test
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
        private static ControlScheme keyListener; // ControlScheme objects can be used globally or attached to object instances! This is a great way to process keys because we can simply add the key reactions we want, instead of having to code the entire response chain for each key, everytime.

        /// <summary>
        /// Scale of the entities and buttons to be drawn.
        /// </summary>
        private static float scale; 

        #endregion

        #region Flags. // Contains flags for different states, such as initialization and debug status.

        private static bool _initialized = false;
        private static bool _debug = false;

        #endregion

        #region Properties. // Publicly accessible data for use through the other classes.
        
        /// <summary>
        /// The main game object. Useful for getting reference to things like the content directory, without declaring it more than once.
        /// </summary>
        public static Game1 Main
        {
            get;
            private set;
        }
        
        /// <summary>
        /// The ContentManager used for loading in Texture2D, Sound, and SpriteFont files.
        /// </summary>
        public static ContentManager Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the SpriteBatch object, used to draw strings and sprites to the screen.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the ShapeDrawer object, used to draw primitives and special images to the screen.
        /// </summary>
        public static ShapeDrawer Pen
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the GraphicsDeviceManager for this current game instance.
        /// </summary>
        public static GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }

        /// <summary>
        /// Current dimensions for the Window. For the current monitor's display dimensions, see <see cref="DisplayBounds"/>.
        /// </summary>
        public static Vector2 ScreenBounds
        {
            get;
            private set;
        }

        /// <summary>
        /// This game instance's Window center. For the center of the current monitor display dimensions see <see cref="DisplayCenter"/>. 
        /// </summary>
        public static Vector2 ScreenCenter
        {
            get { return ScreenBounds * 0.5f; }
        }
        
        /// <summary>
        /// The current monitor display dimensions. For this game instance's Window dimensions, see <see cref="ScreenBounds"/>. 
        /// </summary>
        public static Vector2 DisplayBounds
        {
            get { return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); }
        }

        /// <summary>
        /// The center of the current monitor display dimensions. For this game instance's Window center, see <see cref="ScreenCenter"/>. 
        /// </summary>
        public static Vector2 DisplayCenter
        {
            get { return DisplayBounds * 0.5f; }
        }

        /// <summary>
        /// Random object. This is just a wrapper call to the InputManager.RNG property.
        /// </summary>
        public static Random Random
        {
            get { return InputManager.RNG; }
        }

        /// <summary>
        /// The delta gets updated on every frame. It is the approximate number of seconds that have passed since the previous cycle.
        /// </summary>
        public static float Delta
        {
            get;
            private set;
        }

        // Flag properties. If needed to be accessed from outside the class.

        /// <summary>
        /// Determines if the class has already been initialized.
        /// </summary>
        public static bool Initialized
        {
            get { return _initialized; }
            private set { _initialized = value; }
        }

        /// <summary>
        /// Determines if the class is in debug mode or not.
        /// </summary>
        public static bool Debug
        {
            get { return _debug; }
            private set { _debug = value; }
        }

        #endregion

        #region Methods. // Initializes the class, loads resources and materials, updates entities and GUI components, draws pieces to the screen, and updates the screen dimensions.

        #region Initialization. // Method that initializes and instantiates class fields and data.

        /// <summary>
        /// Initialization method. The GraphicsDeviceManager is private in the main game object, so this is passed in through the initialization method.
        /// </summary>
        /// <param name="_main">The main instance of the game.</param>
        /// <param name="_graphics">The current instance of the GraphicsDeviceManager for the game.</param>
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
            
            keyListener = new ControlScheme();
            scale = 1.0f;

            _initialized = true;
        }

        private static void CreateControlScheme()
        {
            keyListener.Bind(Commands.Debug, Keys.OemTilde, ActionType.Released);
            keyListener.Bind(Commands.Quit, Keys.Delete, ActionType.Released);
        }

        #endregion

        #region Load methods. // Methods covering load functionality, for texture, image, font, and state information and initialization.

        /// <summary>
        /// Called at the end of the public Load overloads. Creates a ShapeDrawer object and sets public property references. Tells the StateManager to create its states.
        /// </summary>
        /// <param name="_sb">Main instance of this game's <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> object.</param>
        private static void Load(SpriteBatch _sb)
        {
            // Set up the shape drawer.
            SpriteBatch = _sb;
            Pen = new ShapeDrawer(_sb, fonts[FontIDs.Main], Main.GraphicsDevice);

            // Set up the shapes.
            StateManager.CreateStates(SpriteBatch, Pen, textures);
        }

        /// <summary>
        /// Loads in a series of textures and fonts that have already been loaded using the <see cref="ContentManager"/> pipeline.
        /// </summary>
        /// <param name="_sb">Main instance of this game's <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> object.</param>
        /// <param name="_textures">Associations of <see cref="TextureIDs"/> ids and <see cref="Texture2D"/> objects.</param>
        /// <param name="_textureIDs">List of <see cref="TextureIDs"/> ids with associations.</param>
        /// <param name="_fonts">Associations of <see cref="FontIDs"/> ids and <see cref="SpriteFont"/> objects.</param>
        /// <param name="_fontIDs">List of <see cref="FontIDs"/> ids with associations.</param>
        public static void Load(SpriteBatch _sb, Dictionary<TextureIDs, Texture2D> _textures, List<TextureIDs> _textureIDs, Dictionary<FontIDs, SpriteFont> _fonts, List<FontIDs> _fontIDs)
        {
            // Add the fonts.
            AddFonts(_fontIDs, _fonts);

            // Add the textures.
            AddTextures(_textureIDs, _textures);
            
            // Load the state and shape drawer.
            Load(_sb);
        }

        /// <summary>
        /// Loads in a series of textures and fonts that still need to be loaded in through the Content pipeline.
        /// </summary>
        /// <param name="_sb">Main instance of this game's <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> object.</param>
        /// <param name="_textures">Associations of <see cref="TextureIDs"/> ids and <see cref="string"/> paths to <see cref="Texture2D"/> objects.</param>
        /// <param name="_textureIDs">List of <see cref="TextureIDs"/> ids with associations.</param>
        /// <param name="_fonts">Associations of <see cref="FontIDs"/> ids and <see cref="string"/> paths to <see cref="SpriteFont"/> objects.</param>
        /// <param name="_fontIDs">List of <see cref="FontIDs"/> ids with associations.</param>
        public static void Load(SpriteBatch _sb, Dictionary<TextureIDs, string> _texturePaths, List<TextureIDs> _textureIDs, Dictionary<FontIDs, string> _fontPaths, List<FontIDs> _fontIDs)
        {
            // Add the fonts.
            AddFonts(_fontIDs, _fontPaths);

            // Add the textures.
            AddTextures(_textureIDs, _texturePaths);

            // Load the state and shape drawer.
            Load(_sb);
        }


        #region Add Texture methods. // Functions designed to add textures with associated ids.

        /// <summary>
        /// Adds in a new <see cref="Texture2D"/> object, through the <see cref="ContentManager"/> pipeline, for each corresponding key-value pair in the path dictionary.
        /// </summary>
        /// <param name="ids">List of <see cref="TextureIDs"/> ids with associations.</param>
        /// <param name="path">Associations of <see cref="TextureIDs"/> ids and <see cref="string"/> paths to <see cref="Texture2D"/> objects.</param>
        public static void AddTextures(List<TextureIDs> ids, Dictionary<TextureIDs, string> path)
        {
            foreach (TextureIDs id in ids)
            {
                AddTexture(id, path[id]);
            }
        }

        /// <summary>
        /// Adds in a <see cref="Texture2D"/> object for each corresponding key-value pair in the path dictionary.
        /// </summary>
        /// <param name="ids">List of <see cref="TextureIDs"/> ids with associations.</param>
        /// <param name="_textures">Associations of <see cref="TextureIDs"/> ids and <see cref="Texture2D"/> objects.</param>
        public static void AddTextures(List<TextureIDs> ids, Dictionary<TextureIDs, Texture2D> _textures)
        {
            foreach (TextureIDs id in ids)
            {
                AddTexture(id, _textures[id]);
            }
        }

        /// <summary>
        /// Adds a new <see cref="Texture2D"/> through the <see cref="ContentManager"/> pipeline.
        /// </summary>
        /// <param name="id"><see cref="TextureIDs"/> id being associated with the <see cref="Texture2D"/> object.</param>
        /// <param name="path">A <see cref="string"/> path to the <see cref="Texture2D"/> object.</param>
        public static void AddTexture(TextureIDs id, string path)
        {
            Texture2D texture = Content.Load<Texture2D>(path);
            AddTexture(id, texture);
        }

        /// <summary>
        /// Adds an existing <see cref="Texture2D"/> object.
        /// </summary>
        /// <param name="id"><see cref="TextureIDs"/> id being associated with the <see cref="Texture2D"/> object.</param>
        /// <param name="texture">A <see cref="Texture2D"/> object.</param>
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

        #endregion

        #region Add Fonts. // Functions designed to add fonts with associated ids.

        /// <summary>
        /// Adds in a new <see cref="SpriteFont"/> object, through the <see cref="ContentManager"/> pipeline, for each corresponding key-value pair in the path dictionary.
        /// </summary>
        /// <param name="ids">List of <see cref="FontIDs"/> ids with associations.</param>
        /// <param name="path">Associations of <see cref="FontIDs"/> ids and <see cref="string"/> paths to <see cref="SpriteFont"/> objects.</param>
        public static void AddFonts(List<FontIDs> ids, Dictionary<FontIDs, string> path)
        {
            foreach (FontIDs id in ids)
            {
                AddFont(id, path[id]);
            }
        }

        /// <summary>
        /// Adds in a <see cref="SpriteFont"/> object for each corresponding key-value pair in the path dictionary.
        /// </summary>
        /// <param name="ids">List of <see cref="FontIDs"/> ids with associations.</param>
        /// <param name="_fonts">Associations of <see cref="FontIDs"/> ids and <see cref="SpriteFont"/> objects.</param>
        public static void AddFonts(List<FontIDs> ids, Dictionary<FontIDs, SpriteFont> _fonts)
        {
            foreach (FontIDs id in ids)
            {
                AddFont(id, _fonts[id]);
            }
        }

        /// <summary>
        /// Adds a new <see cref="SpriteFont"/> through the <see cref="ContentManager"/> pipeline.
        /// </summary>
        /// <param name="id"><see cref="FontIDs"/> id being associated with the <see cref="SpriteFont"/> object.</param>
        /// <param name="path">A <see cref="string"/> path to the <see cref="SpriteFont"/> object.</param>
        public static void AddFont(FontIDs id, string path)
        {
            SpriteFont font = Content.Load<SpriteFont>(path);
            AddFont(id, font);
        }

        /// <summary>
        /// Adds an existing <see cref="SpriteFont"/> object.
        /// </summary>
        /// <param name="id"><see cref="FontIDs"/> id being associated with the <see cref="SpriteFont"/> object.</param>
        /// <param name="texture">A <see cref="SpriteFont"/> object.</param>
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

        #endregion

        #endregion

        #region Update methods. // Methods covering state, input, and GUI updates.

        /// <summary>
        /// Handles any global inputs that you'd like to track, given the commands were added to the Global inputs key already.
        /// </summary>
        private static void HandleGlobalInput()
        {
            Debug = keyListener.IsFired(Commands.Debug);
        }

        /// <summary>
        /// Update the main game loop. Will exit the program if the quit command is triggered. Updates the InputManager and StateManager.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current time span.</param>
        public static void Update(GameTime gameTime)
        {
            if (StateManager.GetState() == States.Quit || keyListener.IsFired(Commands.Quit))
            {
                Main.Exit();
            }

            // Update the elapsed seconds since the last cycle.
            UpdateDelta(gameTime);

            if (Initialized)
            {
                // Handle input.
                InputManager.Update(gameTime);
                HandleGlobalInput();

                // Handle state.
                StateManager.Update(gameTime);
            }
        }

        /// <summary>
        /// Update the main GUI. Updates the GUI through the StateManager.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current time span.</param>
        public static void UpdateGUI(GameTime gameTime)
        {
            if (Initialized)
            {
                StateManager.UpdateGUI(gameTime);
            }
        }

        /// <summary>
        /// Update the value for the total amount of seconds that passed since the last frame.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current time span.</param>
        private static void UpdateDelta(GameTime gameTime)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        #endregion

        #region Draw methods. // Methods covering statements that affect what graphics/textures/images/strings are drawn to the screen at a given moment in time.

        /// <summary>
        /// Draw the background based on the color file stored in the StateManager.
        /// </summary>
        public static void DrawBackground()
        {
            Graphics.GraphicsDevice.Clear(StateManager.Background);
        }

        /// <summary>
        /// Ask the StateManager to draw its entities and buttons to the screen.
        /// </summary>
        public static void Draw()
        {
            if (Initialized)
            {
                StateManager.Draw();
            }
        }

        /// <summary>
        /// Ask the StateManager to draw any GUI components to the screen.
        /// </summary>
        public static void DrawGUI()
        {
            if (Initialized)
            {
                StateManager.DrawGUI(Pen);
            }
        }

        #endregion

        #region Service methods. // Methods that aid the program or complete some limited, focused action within the game.

        /// <summary>
        /// Draws a test GUI component to the screen.
        /// </summary>
        private static void DrawTestGUI()
        {
            int height = (int)Pen.StringDimensions("A").Y;

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y - height, Color.White, "This is a left test.", ShapeDrawer.LEFT_ALIGN);

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y, Color.White, "This is a center test.", ShapeDrawer.CENTER_ALIGN);

            Pen.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y + height, Color.White, "This is a right test.", ShapeDrawer.RIGHT_ALIGN);

            Pen.DrawString(0, 0, Color.White, "This is a default test.");
        }

        /// <summary>
        /// Update the dimensions of the screen.
        /// </summary>
        /// <param name="screenWidth">The width to change the screen to.</param>
        /// <param name="screenHeight">The height to change the screen to.</param>
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

        /// <summary>
        /// Empty method for just applying changes. Calls UpdateScreen(w, h) using its current width and height values, anyway.
        /// </summary>
        public static void UpdateScreen()
        {
            UpdateScreen(Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Draw a message to the screen.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="s">String to print.</param>
        /// <param name="alignment">Alignment to print the message to.</param>
        public static void DrawMessage(int x, int y, string s, int alignment, Color textColor)
        {
            // Print message.
            Pen.DrawString(x, y, textColor, s, alignment);
        }


        /// <summary>
        /// Scale to draw entities.
        /// </summary>
        /// <param name="scale">Value. 1.0f means original size.</param>
        public static void SetScale(float _scale = 1.0f)
        {
            scale = _scale;

            if (scale != StateManager.Scale)
            {
                StateManager.Scale = scale;
            }
        }

        #endregion

        #endregion

    }
}
