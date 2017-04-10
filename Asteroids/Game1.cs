/// Game1.cs - Version 3
/// Author: Ian Effendi
/// Date: 3.12.2017

#region Using statements.

// System calls.
using System.Collections.Generic;
using System;

// MonoGame calls.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Asteroids calls.
using Asteroids.Entities;
using Asteroids.Tools;

#endregion

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region Fields. // Private obj/data that will be passed to the global manager.

        /// <summary>
        /// The GraphicsDeviceManager. This is used, mostly, for changing screen dimensions.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The SpriteBatch. This is used to draw our 2D images to the screen.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region Constructor.

        /// <summary>
        /// The only constructor for our game class. Sets up the Content and graphics objects.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #endregion

        #region Initialization. // This is called before loading the content.

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GlobalManager.Initialize(this, graphics);  

            base.Initialize();
        }

        #endregion

        #region Load Content. // This will load any resources that we need for the game.

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Set up font information collection.
            List<FontIDs> fontIDs = new List<FontIDs>();
            Dictionary<FontIDs, string> fontPaths = new Dictionary<FontIDs, string>();
            LoadFonts(out fontIDs, out fontPaths);
            
            // Set up texture information collection.
            List<TextureIDs> textureIDs = new List<TextureIDs>();
            Dictionary<TextureIDs, string> texturePaths = new Dictionary<TextureIDs, string>();
            LoadTextures(out textureIDs, out texturePaths);

            // Load the global resources using the global manager.
            GlobalManager.Load(spriteBatch, texturePaths, textureIDs, fontPaths, fontIDs); 
        }
        
        /// <summary>
        /// This will return a list of <see cref="FontIDs"/> and the paths of the <see cref="SpriteFont"/> objects associated with them.
        /// </summary>
        /// <param name="ids">List of <see cref="FontIDs"/> that resources have been added for.</param>
        /// <param name="paths">List of <see cref="string"/> paths to <see cref="SpriteFont"/> objects.</param>
        private void LoadFonts(out List<FontIDs> ids, out Dictionary<FontIDs, string> paths)
        {
            // Set up font information collection.
            ids = new List<FontIDs>();
            paths = new Dictionary<FontIDs, string>();

            // Add fonts here.
            LoadFont(FontIDs.Main, "assets/fonts/mainfont", ids, paths);
            LoadFont(FontIDs.Small, "assets/fonts/smallfont", ids, paths);

            // TODO: Add additional fonts here.
            // LoadFont(FontID, Path to Font, ids, paths);
        }

        /// <summary>
        /// This will add a font path to the dictionary under the provided <see cref="FontIDs"/> ID. 
        /// </summary>
        /// <param name="id"><see cref="FontIDs"/> to associate font with the <see cref="SpriteFont"/>.</param>
        /// <param name="path">Path to the <see cref="SpriteFont"/> object.</param>
        /// <param name="ids">List of <see cref="FontIDs"/> that resources have been added for.</param>
        /// <param name="paths">List of <see cref="string"/> paths to <see cref="SpriteFont"/> objects.</param>
        private void LoadFont(FontIDs id, string path, List<FontIDs> ids, Dictionary<FontIDs, string> paths)
        {
            ids.Add(id);
            paths.Add(id, path);
        }

        /// <summary>
        /// This will return a list of <see cref="TextureIDs"/> and the paths of the <see cref="Texture2D"/> objects associated with them.
        /// </summary>
        /// <param name="ids">List of <see cref="TextureIDs"/> that resources have been added for.</param>
        /// <param name="paths">List of <see cref="string"/> paths to <see cref="Texture2D"/> objects.</param>
        private void LoadTextures(out List<TextureIDs> ids, out Dictionary<TextureIDs, string> paths)
        {
            // Set up font information collection.
            ids = new List<TextureIDs>();
            paths = new Dictionary<TextureIDs, string>();

            // Add fonts here.
            LoadTexture(TextureIDs.Asteroid1, "assets/asteroids/asteroid_01", ids, paths);
            LoadTexture(TextureIDs.Asteroid2, "assets/asteroids/asteroid_02", ids, paths);
            LoadTexture(TextureIDs.Asteroid3, "assets/asteroids/asteroid_03", ids, paths);
            LoadTexture(TextureIDs.Player, "assets/player/spaceship", ids, paths);
            LoadTexture(TextureIDs.Bullet, "assets/player/bullet", ids, paths);
            LoadTexture(TextureIDs.Button, "assets/ui/button_base", ids, paths);
            LoadTexture(TextureIDs.Test, "assets/player/bullet", ids, paths);

            // TODO: Add additional textures here.
            // LoadTexture(TextureID, Path to Texture, ids, paths);
        }

        /// <summary>
        /// This will add a font path to the dictionary under the provided <see cref="TextureIDs"/> ID. 
        /// </summary>
        /// <param name="id"><see cref="TextureIDs"/> to associate font with the <see cref="Texture2D"/>.</param>
        /// <param name="path">Path to the <see cref="Texture2D"/> object.</param>
        /// <param name="ids">List of <see cref="TextureIDs"/> that resources have been added for.</param>
        /// <param name="paths">List of <see cref="string"/> paths to <see cref="Texture2D"/> objects.</param>
        private void LoadTexture(TextureIDs id, string path, List<TextureIDs> ids, Dictionary<TextureIDs, string> paths)
        {
            ids.Add(id);
            paths.Add(id, path);
        }

        #endregion

        #region Unload Content. // (Empty code section). This is called once per game, before exiting, and unloads any resources.

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update. // Calls the Update and UpdateGUI methods in the global manager.

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            GlobalManager.Update(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw. // Begins and Ends the spritebatch but also calls the DrawBackground, Draw, and DrawGUI methods in the global manager.

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GlobalManager.DrawBackground();

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            GlobalManager.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

    }
}
