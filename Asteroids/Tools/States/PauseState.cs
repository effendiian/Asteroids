/// PauseState.cs - Version 1.
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
    /// The PauseState stores the functionality for States.Pause enum.
    /// </summary>
    public class PauseState : State
    {

        #region Constructors. // Sets this state's States enum flag to States.Pause.

        public PauseState(ColorSet set, float scale = 1.0f) : base(States.Pause, set, scale)
        {
            // Any special instructions for the pause menu should take place here.
        }

        public PauseState(Color draw, Color bg, float scale = 1.0f) : base(States.Pause, draw, bg, scale)
        {
            // Any special instructions for the pause menu should take place here.
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

        #region Update methods. // Update calls for the Pause.

        /// <summary>
        /// Bind the debug key and escape key for the Pause.
        /// </summary>
        protected override void BindKeys()
        {
            Controls.Bind(Commands.Debug, Keys.D, ActionType.Released);
            Controls.Bind(Commands.Pause, Keys.P, ActionType.Released);
            Controls.Bind(Commands.Back, Keys.Escape, ActionType.Released);
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

                if (Controls.IsFired(Commands.Back))
                {
                    StateManager.ChangeState(States.Main);
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
                if (IsActionFired(Actions.Resume))
                {
                    StateManager.TogglePause();
                }

                if (IsActionFired(Actions.Quit))
                {
                    StateManager.Quit();
                }
            }
        }

        /// <summary>
        /// Updates all entities and buttons.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        /// <summary>
        /// Queue all the GUI messages that need to be printed.
        /// </summary>
        protected override void QueueGUIMessages()
        {
            string guiMessage = "";

            guiMessage += "Press the ESC key to return to the main menu." + "\n";
            guiMessage += "Press the P key to resume the game." + "\n";
            guiMessage += "Press the Q key to quit to the desktop." + "\n";
            guiMessage += "Throttle and Brake with W and S. Rotate with A and D." + "\n";
            guiMessage += "Turn with the Left and Right arrow keys.";

            // What to do during the pause functionality.  
            // Print message: Escape to quit. P to resume the game.
            Vector2 position = this.GetScreenCenter();
            Padding padding = new Padding(0, this.GetStringHeight(guiMessage));
            AddMessage(guiMessage, position, padding, this.DrawColor, 1, ShapeDrawer.CENTER_ALIGN);
        }

        #endregion

        #region Draw methods. // Draw calls for the Pause.

        #endregion

        #endregion

    }
}
