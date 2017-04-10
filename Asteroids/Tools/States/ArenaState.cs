/// ArenaState.cs - Version 1.
/// Author: Ian Effendi
/// Date: 4.9.2017

#region Using statements.

// System using statements.
using System;
using System.Collections;
using System.Collections.Generic;

// MonoGame using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Asteroids using statements.
using Asteroids.Attributes;
using Asteroids.Entities;

#endregion

namespace Asteroids.Tools.States
{
    /// <summary>
    /// The ArenaState stores the functionality for States.Arena enum.
    /// </summary>
    public class ArenaState : State
    {

        #region Constructors. // Sets this state's States enum flag to StateType.Arena.

        public ArenaState(ColorSet set, float scale = 1.0f) : base(StateType.Arena, set, scale)
        {
            // Any special instructions for the arena should take place here.
        }

        public ArenaState(Color draw, Color bg, float scale = 1.0f) : base(StateType.Arena, draw, bg, scale)
        {
            // Any special instructions for the arena should take place here.
        }

		#endregion

		#region Methods. // Methods that have been overriden from the parent class.

		#region Load methods. // Called to help faciliate the addition of entities to a given state.

		/// <summary>
		/// Faciliate the addition of entities to a given state.
		/// </summary>
		/// <param name="textures">Texture2D hashtable.</param>
		public override void LoadEntities(Dictionary<TextureIDs, Texture2D> textures)
		{
			// Set up of anything needed of entities.
			// Asteroid textures set up.
			List<Texture2D> asteroidTextures = new List<Texture2D>();
			asteroidTextures.Add(textures[TextureIDs.Asteroid1]);
			asteroidTextures.Add(textures[TextureIDs.Asteroid2]);
			asteroidTextures.Add(textures[TextureIDs.Asteroid3]);

			TestMover test = new TestMover(GlobalManager.Pen, textures[TextureIDs.Test], Color.LimeGreen, _size: new Vector2(15, 15));
			test.PlayerControl = true;

			// TODO: Replace with manager code. Pass in list of asteroid textures.

			// Load the entities.
			LoadEntities(new Asteroid(GlobalManager.Pen, asteroidTextures[0], "Asteroid"),
					new TestMover(GlobalManager.Pen, textures[TextureIDs.Test], Color.LimeGreen, _size: new Vector2(15, 15)),
					test);
		}

		/// <summary>
		/// Facilitate the addition of buttons to a given state.
		/// </summary>
		/// <param name="button">Texture of the button.</param>
		public override void LoadButtons(Texture2D button, Padding screenPadding, Padding centerPadding, Vector2 bounds)
		{
			this.LoadButtons(new Button(Actions.Pause, GlobalManager.Pen, Button.Positions.BottomRight, screenPadding.Get(-1), bounds, button, "Pause"));
		}

		#endregion

		#region Reset/Start/Stop methods. // Called in various types of state changes.

		/// <summary>
		/// Reset all entities and then start the game.
		/// </summary>
		public override void Start()
        {
            Reset();
            base.Start();
        }

        #endregion

        #region Update methods. // Update calls for the Arena.

        /// <summary>
        /// Bind the debug key and escape key for the Arena.
        /// </summary>
        protected override void BindKeys()
        {
            Controls.Bind(Commands.Debug, Keys.D, ActionType.Released);
            Controls.Bind(Commands.Pause, Keys.P, ActionType.Released);
            Controls.Bind(Commands.Pause, Keys.Escape, ActionType.Released);
        }

        /// <summary>
        /// Handles all key press inputs for a state.
        /// </summary>
        protected override void HandleInput()
        {
            if (!Controls.IsEmpty())
            {
                if (Controls.IsFired(Commands.Debug))
                {
                    this.Debug = !Debug;
                }

                if (Controls.IsFired(Commands.Pause) 
                    || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed
                    || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    StateManager.TogglePause();
                }
            }
        }

        /// <summary>
        /// Handles all button press inputs for a state.
        /// </summary>
        protected override void HandleGUIInput()
        {
            if (HasButtons)
            {
                if (IsActionFired(Actions.Pause))
                {
                    StateManager.TogglePause();
                }
            }
        }

        /// <summary>
        /// Updates all entities and buttons.
        /// </summary>
        /// <param name="gameTime">Snapshot of time passed.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TestMoverCollision(); // The mover collision.
        }

        /// <summary>
        /// Test mover collisions.
        /// </summary>
        private void TestMoverCollision()
        {
            if (Exists(EntityType.Test))
            {
                List<Entity> tests = GetEntities(EntityType.Test);
                List<Entity> npcs = new List<Entity>();
                TestMover player = null;

                foreach (Entity e in tests)
                {
                    if (e is TestMover)
                    {
                        if (((TestMover)e).PlayerControl)
                        {
                            player = (TestMover)e;
                        }
                        else
                        {
                            npcs.Add(e);
                        }
                    }
                }

                if (player != null)
                {
                    foreach (Entity e in tests)
                    {
                        TestMover test = (TestMover)e;

                        if (test.Collision(player))
                        {
                            // test.DrawColor = Color.Red;
                            test.Randomize();
                        }
                        else if (test.Proximity(player))
                        {
                            // test.DrawColor = Color.LimeGreen;
                        }
                        else
                        {
                            // test.DrawColor = Color.White;
                        }
                    }
                }
            }
        }

        #endregion
        
        #endregion

    }
}
