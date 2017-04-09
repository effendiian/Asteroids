/// MainMenuState.cs - Version 1.
/// Author: Ian Effendi
/// Date: 4.8.2017

#region Using statements.

// MonoGame using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Asteroids using statements.
using Asteroids.Attributes;

#endregion

namespace Asteroids.Tools.States
{
    public class MainMenuState : State
    {

        #region Constructors. // Sets this state's States enum flag to States.Main.

        public MainMenuState(ColorSet set, float scale = 1.0f) : base(States.Main, set, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

        public MainMenuState(Color draw, Color bg, float scale = 1.0f) : base(States.Main, draw, bg, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

        #endregion

        #region Child methods. // Methods that have been overriden from the parent class.

        #region Reset/Start/Stop methods. // Called in various types of state changes.

        // Reset

        // Start

        // Stop

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
                    StateManager.ChangeState(States.Arena);
                }

                if (IsActionFired(Actions.Options))
                {
                    StateManager.ChangeState(States.Options);
                }

                if (IsActionFired(Actions.Scores))
                {
                    StateManager.ChangeState(States.Scores);
                }
            }
        }

        #endregion

        #region Draw methods. // Draw calls for the main menu.
        
        /// <summary>
        /// Draw the GUI for the main menu.
        /// </summary>
        public override void DrawGUI()
        {
            base.DrawGUI();

            if (Debug)
            {
                string msg = this.ToString();
                Padding padding = new Padding(0, GetStringHeight(msg));
                Vector2 position = new Vector2(10, padding.Y);
                AddMessage(this.ToString(), position, padding, this.DrawColor, 1, ShapeDrawer.LEFT_ALIGN);
            }
        }

        #endregion


        #endregion

        #region Service methods.

        #endregion

    }
}
