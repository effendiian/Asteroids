/// OptionsState.cs - Version 1.
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
    /// The OptionsState stores the functionality for States.Options enum.
    /// </summary>
    public class OptionsState : State
    {

        #region Constructors. // Sets this state's States enum flag to States.Options.

        public OptionsState(ColorSet set, float scale = 1.0f) : base(StateType.Options, set, scale)
        {
            // Any special instructions for the options menu should take place here.
        }

        public OptionsState(Color draw, Color bg, float scale = 1.0f) : base(StateType.Options, draw, bg, scale)
        {
            // Any special instructions for the options menu should take place here.
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

			LoadButtons(new Button(Actions.Dimensions, pen, Button.Positions.Center, null, null, button, "Change Dimensions"),
				new Button(Actions.Back, pen, Button.Positions.BottomRight, screenPadding.Get(-1), bounds, button, "Back"));
		}

		#endregion

		#region Update methods. // Update calls for the Options.

		/// <summary>
		/// Bind the debug key and escape key for the Options.
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
                if (IsActionFired(Actions.Dimensions))
                {
                    this.ChangeDimensions();
                }

                if (IsActionFired(Actions.Back))
                {
                    StateManager.ChangeState(StateType.Main);
                }
            }
        }

        /// <summary>
        /// Queue options menu instructions to the Messages system.
        /// </summary>
        protected override void QueueGUIMessages()
        {
            string guiMessage = "";

            guiMessage += "Press the ESC key to return to the main menu." + "\n";
            guiMessage += "Current screen dimensions: " + this.GetScreenBounds() + "\n";
            guiMessage += "Fullscreen: ";

            if (GlobalManager.Graphics.IsFullScreen)
            {
                guiMessage += "On";
            }
            else
            {
                guiMessage += "Off";
            }

            guiMessage += "" + "\n";

            Vector2 position = this.GetScreenCenter();
            Padding padding = new Padding(0, this.GetStringHeight(guiMessage));
            AddMessage(guiMessage, position, padding, this.DrawColor, 1, ShapeDrawer.CENTER_ALIGN);
        }

        #endregion
        
        #region Service methods. // Methods that add class functionality.

        /// <summary>
        /// Change the dimensions of the game screen.
        /// </summary>
        private void ChangeDimensions()
        {
            int height = 600;
            int width = 800;

            switch (GlobalManager.Graphics.PreferredBackBufferWidth)
            {
                case 1920:
                    height = 600;
                    width = 800;
                    break;
                case 1200:
                    height = 1020;
                    width = 1920;
                    break;
                case 800:
                default:
                    height = 720;
                    width = 1200;
                    break;
            }

            float _scale = 1.0f * (width / 800);

            GlobalManager.UpdateScreen(width, height);
            GlobalManager.SetScale(_scale);
        }

        #endregion

        #endregion

    }
}
