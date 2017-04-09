/// ScoresState.cs - Version 1.
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
    /// The ScoresState stores the functionality for States.Gameover enum.
    /// </summary>
    public class ScoresState : State
    {

        #region Constructors. // Sets this state's States enum flag to StateType.Gameover.

        public ScoresState(ColorSet set, float scale = 1.0f) : base(StateType.Gameover, set, scale)
        {
            // Any special instructions for the scores menu should take place here.
        }

        public ScoresState(Color draw, Color bg, float scale = 1.0f) : base(StateType.Gameover, draw, bg, scale)
        {
            // Any special instructions for the scores menu should take place here.
        }

        #endregion

        #region Methods. // Methods that have been overriden from the parent class.
        
        #region Update methods. // Update calls for the Gameover.

        /// <summary>
        /// Bind the debug key and escape key for the Gameover.
        /// </summary>
        protected override void BindKeys()
        {
            Controls.Bind(Commands.Debug, Keys.D, ActionType.Released);
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

                if (Controls.IsFired(Commands.Back)
                    || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    StateManager.ChangeState(StateType.Main);
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
                if (IsActionFired(Actions.Back))
                {
                    StateManager.ChangeState(StateType.Main);
                }
            }
        }

        /// <summary>
        /// Queue messages to print to the score screen.
        /// </summary>
        protected override void QueueGUIMessages()
        {
            Vector2 position = this.GetScreenCenter();
            Padding padding = new Padding(0, (this.GetStringHeight("A") * 2) + 55);
            AddMessage("Scores are indicated here.", position, padding, this.DrawColor, 0, ShapeDrawer.CENTER_ALIGN);
        }

        #endregion

        #endregion

    }
}
