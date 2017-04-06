/// InputManager.cs - Version 1
/// Author: Ian Effendi
/// Date: 3.26.2017
/// Last Updated: 4.4.2017
/// File Description: Contains reference to the class.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Import the Monogame packages.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Import the attributes.
using Asteroids.Attributes;

namespace Asteroids.Tools
{
    /// <summary>
    /// Handles the input values in the code.
    /// Deals with keyboard input, position of the mouse, and state of the buttons.
    /// </summary>
    public class InputManager
    {

        // Fields.
        // Codes.
        private const int LEFT = 0;
        private const int RIGHT = 1;
        private const int MIDDLE = 2;

        // Mouse color.
        private static Color mouseColor; // The mouse color.

        // Keyboard states.
        private static KeyboardState prevStateKB; // Previous Keyboard state.
        private static KeyboardState currStateKB; // Current Keyboard state.

        // Mouse states.
        private static MouseState prevStateMS; // Previous mouse state.
        private static MouseState currStateMS; // Current mouse state.

        // Position of the Mouse cursor.
        private static Point prevPosMS; // Previous mouse position.
        private static Point currPosMS; // Current mouse position.

        // Random number generator.
        private static Random rng; // Random number generator.

        private static bool initialized = false;

        // Properties.
        public static Random RNG {
            get { return rng; }
        }

        public static int NextInteger
        {
            get { return rng.Next(); }
        }

        public static double NextDouble
        {
            get { return rng.NextDouble(); }
        }

        // Methods.
        public static int Range(int inclusive, int inclusiveOut)
        {
            return rng.Next(inclusive, inclusiveOut + 1);
        }
        
        // Initialize the class.
        public static void Initialize()
        {
            currStateKB = Keyboard.GetState();
            prevStateKB = currStateKB;

            currStateMS = Mouse.GetState();
            prevStateMS = currStateMS;

            currPosMS = currStateMS.Position;
            prevPosMS = currPosMS;

            mouseColor = Color.Red;

            rng = new Random();

            initialized = true;
        }

        // Returns value of initialization flag.
        public static bool IsInitialized()
        {
            return InputManager.initialized;
        }

        // The update method.
        public static void Update(GameTime gameTime)
        {
            if (initialized)
            {
                UpdateKeyboard(gameTime); // Update Keyboard.
                UpdateMouse(gameTime); // Update Mouse.
            }
        }

        // Update the keyboard states.
        private static void UpdateKeyboard(GameTime gameTime)
        {
            prevStateKB = currStateKB;
            currStateKB = Keyboard.GetState();
        }

        // Update the mouse states.
        private static void UpdateMouse(GameTime gameTime)
        {
            prevStateMS = currStateMS;
            currStateMS = Mouse.GetState();

            prevPosMS = currPosMS;
            currPosMS = currStateMS.Position;

            mouseColor = Color.Red;

            if (LeftButtonDown)
            {
                mouseColor = Color.Blue;

                if (RightButtonDown)
                {
                    mouseColor = Color.Purple;
                }
            }

            if (RightButtonDown)
            {
                mouseColor = Color.Gold;
            }
        }

        // Draw the mouse cursor.
        public static void Draw(ShapeDrawer shapeDrawer)
        {
            if (initialized)
            {
                shapeDrawer.DrawMouseCursor(currPosMS.X, currPosMS.Y, mouseColor);
            }
        }

        // Draw information to the GUI.
        public static void DrawGUI(ShapeDrawer shapeDrawer)
        {
            if (initialized)
            {
                int h = (int)shapeDrawer.Font.MeasureString("A").Y;

                shapeDrawer.DrawString(10, 10, StateManager.DrawColor, "Mouse Position: " + currPosMS.ToString());
            }
        }

        #region Keyboard Key Functions
        // Return value to the control schema.
        public static bool IsFired(List<Keys> keys, ActionType action)
        {
            foreach (Keys key in keys)
            {
                if (IsFired(key, action))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFired(Keys key, ActionType action)
        {
            switch (action)
            {
                case ActionType.Up:
                    return IsKeyUp(key);
                case ActionType.Down:
                    return IsKeyDown(key);
                case ActionType.Pressed:
                    return IsKeyPressed(key);
                case ActionType.Released:
                    return IsKeyReleased(key);
                case ActionType.Held:
                    return IsKeyHeld(key);
                default:
                    // A key without an action type should not be true, ever.
                    return false;
            }
        }

        // Check the control schema.
        public static bool IsKeyUp(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyUp(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Check the control schema.
        public static bool IsKeyDown(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyDown(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Check the control schema.
        public static bool IsKeyHeld(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyHeld(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Check the control schema.
        public static bool IsKeyPressed(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyPressed(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        // Check the control schema.
        public static bool IsKeyReleased(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyReleased(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Check if key is up.
        public static bool IsKeyUp(Keys key)
        {
            // If key is up during current frame, return true.
            return (currStateKB.IsKeyUp(key));
        }

        // Check if key is down.
        public static bool IsKeyDown(Keys key)
        {
            // If key is down during the current frame, return true.
            return (currStateKB.IsKeyDown(key));
        }

        // Check if key is being held down.
        public static bool IsKeyHeld(Keys key)
        {
            // If the key is down during previous and current frames, return true.
            return (prevStateKB.IsKeyDown(key) && IsKeyDown(key));
        }

        // Check if key has just been pressed down.
        public static bool IsKeyPressed(Keys key)
        {
            // If the key is up during previous frame and down during current, return true.
            return (prevStateKB.IsKeyUp(key) && IsKeyDown(key));
        }

        // Check if the key has just been released.
        public static bool IsKeyReleased(Keys key)
        {
            // If the key is down during the previous frame and up during current, return true.
            return (prevStateKB.IsKeyDown(key) && IsKeyUp(key));
        }

        #endregion

        #region Mouse Button Functions

        // Mouse button states and functions:

        // Is the left button down?
        public static bool LeftButtonDown
        {
            get { return IsButtonDown(LEFT); }
        }

        // Is the left button up?
        public static bool LeftButtonUp
        {
            get { return IsButtonUp(LEFT); }
        }

        // Is the left button held?
        public static bool LeftButtonHeld
        {
           get { return IsButtonHeld(LEFT); }
        }

        // Is the left button just pressed?
        public static bool LeftButtonPressed
        {
            get { return IsButtonPressed(LEFT); }
        }

        // Is the left button just released?
        public static bool LeftButtonReleased
        {
            get { return IsButtonReleased(LEFT); }
        }

        // Is the right button down?
        public static bool RightButtonDown
        {
            get { return IsButtonDown(RIGHT); }
        }

        // Is the right button up?
        public static bool RightButtonUp
        {
            get { return IsButtonUp(RIGHT); }
        }

        // Is the right button held?
        public static bool RightButtonHeld
        {
            get { return IsButtonHeld(RIGHT); }
        }

        // Is the right button just pressed?
        public static bool RightButtonPressed
        {
            get { return IsButtonPressed(RIGHT); }
        }

        // Is the right button just released?
        public static bool RightButtonReleased
        {
            get { return IsButtonReleased(RIGHT); }
        }

        // Is the middle button down?
        public static bool MiddleButtonDown
        {
            get { return IsButtonDown(MIDDLE); }
        }

        // Is the middle button up?
        public static bool MiddleButtonUp
        {
            get { return IsButtonUp(MIDDLE); }
        }

        // Is the middle button held?
        public static bool MiddleuttonHeld
        {
            get { return IsButtonHeld(MIDDLE); }
        }

        // Is the middle button just pressed?
        public static bool MiddleButtonPressed
        {
            get { return IsButtonPressed(MIDDLE); }
        }

        // Is the middle button just released?
        public static bool MiddleButtonReleased
        {
            get { return IsButtonReleased(MIDDLE); }
        }

        // Is the button down?
        private static bool IsButtonDown(int code)
        {
            switch (code)
            {
                case LEFT:
                    return (currStateMS.LeftButton == ButtonState.Pressed);
                case RIGHT:
                    return (currStateMS.RightButton == ButtonState.Pressed);
                case MIDDLE:
                    return (currStateMS.MiddleButton == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        // Is the button up?
        private static bool IsButtonUp(int code)
        {
            switch (code)
            {
                case LEFT:
                    return (currStateMS.LeftButton == ButtonState.Released);
                case RIGHT:
                    return (currStateMS.RightButton == ButtonState.Released);
                case MIDDLE:
                    return (currStateMS.MiddleButton == ButtonState.Released);
                default:
                    return false;
            }
        }

        // Is the button being held?
        private static bool IsButtonHeld(int code)
        {
            switch (code)
            {
                case LEFT:
                    return (prevStateMS.LeftButton == ButtonState.Pressed) && IsButtonDown(code);
                case RIGHT:
                    return (prevStateMS.RightButton == ButtonState.Pressed) && IsButtonDown(code);
                case MIDDLE:
                    return (prevStateMS.MiddleButton == ButtonState.Pressed) && IsButtonDown(code);
                default:
                    return false;
            }
        }

        // Is the button just pressed?
        private static bool IsButtonPressed(int code)
        {
            switch (code)
            {
                case LEFT:
                    return (prevStateMS.LeftButton == ButtonState.Released) && IsButtonDown(code);
                case RIGHT:
                    return (prevStateMS.RightButton == ButtonState.Released) && IsButtonDown(code);
                case MIDDLE:
                    return (prevStateMS.MiddleButton == ButtonState.Released) && IsButtonDown(code);
                default:
                    return false;
            }
        }

        // Is the button just released?
        private static bool IsButtonReleased(int code)
        {
            switch (code)
            {
                case LEFT:
                    return (prevStateMS.LeftButton == ButtonState.Pressed) && IsButtonUp(code);
                case RIGHT:
                    return (prevStateMS.RightButton == ButtonState.Pressed) && IsButtonUp(code);
                case MIDDLE:
                    return (prevStateMS.MiddleButton == ButtonState.Pressed) && IsButtonUp(code);
                default:
                    return false;
            }
        }

        // Position of the mouse and cursor collision functions:

        // Get the current position of the mouse.
        public static Point GetMousePosition()
        {
            return currPosMS;
        }

        // Check for mouse collision.
        public static bool MouseCollision(Rectangle bounds)
        {
            // Is the mouse cursor colliding with the bounds?
            return (CurrentMouseCollision(bounds));
        }

        // Check if mouse entered bounds.
        public static bool OnEnter(Rectangle bounds)
        {
            // If the previous mouse position collision was false and current is true, return true.
            return (!PreviousMouseCollision(bounds) && CurrentMouseCollision(bounds));
        }

        // Check if mouse exited bounds.
        public static bool OnExit(Rectangle bounds)
        {
            // If the previous mouse position collision was true and current is false, return true.
            return (PreviousMouseCollision(bounds) && !CurrentMouseCollision(bounds));
        }

        // Check if mouse is hovering over bounds.
        public static bool OnHover(Rectangle bounds)
        {
            // If the previous mouse position collision is true and current is true, return true.
            return (PreviousMouseCollision(bounds) && CurrentMouseCollision(bounds));
        }

        // Check for mouse collision, of the current position.
        private static bool CurrentMouseCollision(Rectangle bounds)
        {
            // If the x and y of the mouse cursor is within the bounds, return true.
            return bounds.Contains(currPosMS);
        }
        
        // Check for mouse collision, of the previous position.
        private static bool PreviousMouseCollision(Rectangle bounds)
        {
            // If the x and y of the mouse cursor is within the bounds, return true.
            return bounds.Contains(prevPosMS);
        }

        #endregion

        // Helper functions.
        // Generates a sign.
        public static int GetSign()
        {
            int sign = rng.Next(0, 2);

            switch (sign)
            {
                case 0:
                    return 1;
                default:
                case 1:
                    return -1;
            }
        }
    }
}
