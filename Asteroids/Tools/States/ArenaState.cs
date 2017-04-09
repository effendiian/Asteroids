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

        #region Constructors. // Sets this state's States enum flag to States.Arena.

        public ArenaState(ColorSet set, float scale = 1.0f) : base(States.Arena, set, scale)
        {
            // Any special instructions for the arena should take place here.
        }

        public ArenaState(Color draw, Color bg, float scale = 1.0f) : base(States.Arena, draw, bg, scale)
        {
            // Any special instructions for the arena should take place here.
        }

        #endregion

        #region Methods. // Methods that have been overriden from the parent class.

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
