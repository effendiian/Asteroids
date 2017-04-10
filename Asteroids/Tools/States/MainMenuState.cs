/// MainMenuState.cs - Version 1.
/// Author: Ian Effendi
/// Date: 4.8.2017

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
using Asteroids.Tools;
using Asteroids.Entities;

#endregion

namespace Asteroids.Tools.States
{
    /// <summary>
    /// The MainMenuState stores the functionality for States.Menu enum.
    /// </summary>
    public class MainMenuState : State
    {

        #region Constructors. // Sets this state's States enum flag to States.Main.

        public MainMenuState(ColorSet set, float scale = 1.0f) : base(StateType.Main, set, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

        public MainMenuState(Color draw, Color bg, float scale = 1.0f) : base(StateType.Main, draw, bg, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

		#endregion

		#region Methods. // Methods that have been overriden from the parent class.

		#region Load methods. // Called to help faciliate the addition of entities to a given state.
		
		/// <summary>
		/// Facilitate the addition of buttons to a given state.
		/// </summary>
		/// <param name="button">Texture of the button.</param>
		public override void LoadButtons(Texture2D button, Padding screenPadding, Padding centerPadding, Vector2 bounds)
		{
			ShapeDrawer pen = GlobalManager.Pen;

			LoadButtons(new Button(Actions.Start, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(-1)), bounds, button, "Start"), 
				new Button(Actions.Options, pen, Button.Positions.Center, null, bounds, button, "Options"),
				new Button(Actions.Scores, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(1)), bounds, button, "Scores"),
				new Button(Actions.Quit, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(2)), bounds, button, "Exit"));
		}

		#endregion

		#region Update methods. // Update calls for the main menu.

		/// <summary>
		/// Bind the debug key and escape key for the Main menu.
		/// </summary>
		protected override void BindKeys()
        {
            Controls.Bind(Commands.Debug, Keys.D, ActionType.Released);
            Controls.Bind(Commands.Quit, Keys.Escape, ActionType.Released);
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

                if (Controls.IsFired(Commands.Quit) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    StateManager.Quit();
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
                if (IsActionFired(Actions.Quit))
                {
                    StateManager.Quit();
                }

                if (IsActionFired(Actions.Start))
                {
                    StateManager.StartGame();
                }

                if (IsActionFired(Actions.Options))
                {
                    StateManager.ChangeState(StateType.Options);
                }

                if (IsActionFired(Actions.Scores))
                {
                    StateManager.ChangeState(StateType.Gameover);
                }
            }
        }

        #endregion
        
        #endregion
        
    }
}
