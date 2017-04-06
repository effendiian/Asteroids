/// InputManager.cs - Version 1
/// Author: Ian Effendi
/// Date: 3.26.2017
/// Last Updated: 4.6.2017
/// File Description: Contains reference to the class.

#region Using statements.

// Import the system packages.
using System;
using System.Collections.Generic;
using System.Linq;

// Import the Monogame packages.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Asteroids.Tools
{

    #region Enum. // Used for the mouse buttons.

    /// <summary>
    /// The mouse button enum is used to leverage mouse buttons.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Code for the Left Mouse Button.
        /// </summary>
        Left,

        /// <summary>
        /// Code for the Right Mouse Button.
        /// </summary>
        Right,

        /// <summary>
        /// Code for the Middle Mouse Button.
        /// </summary>
        Middle,

        /// <summary>
        /// Null code for error detection.
        /// </summary>
        Null
    }



    #endregion

    /// <summary>
    /// Handles the input values in the code.
    /// Deals with keyboard input, position of the mouse, and state of the buttons.
    /// </summary>
    public class InputManager
    {

        #region Fields. // These are static fields the InputManager class uses to leverage colors and states.

        /// <summary>
        /// The current color of the mouse used to draw the cursor to the screen.
        /// </summary>
        private static Color mouseColor; // The mouse color.

        // Keyboard states.

        /// <summary>
        /// The previous keyboard state.
        /// </summary>
        private static KeyboardState prevStateKB;

        /// <summary>
        /// The current keyboard state.
        /// </summary>
        private static KeyboardState currStateKB;

        // Mouse states.

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private static MouseState prevStateMS; // Previous mouse state.

        /// <summary>
        /// The current mouse state.
        /// </summary>
        private static MouseState currStateMS; // Current mouse state.

        // Position of the Mouse cursor.

        /// <summary>
        /// The previous mouse position.
        /// </summary>
        // private static Point prevPosMS; // Previous mouse position.

        /// <summary>
        /// The current mouse position.
        /// </summary>
        // private static Point currPosMS; // Current mouse position.

        /// <summary>
        /// The Random number generator used to give back values.
        /// </summary>
        private static Random rng; // Random number generator.

        #endregion

        #region Flags. // Flags used by the InputManager.

        /// <summary>
        /// Indicates whether or not this class has been initialized.
        /// </summary>
        private static bool initialized = false;

        #endregion

        #region Properties. // Provides public access to private data.

        /// <summary>
        /// Returns the random number generator.
        /// </summary>
        public static Random RNG
        {
            get { return rng; }
        }

        /// <summary>
        /// Return the previous mouse position.
        /// </summary>
        public static Point PreviousMousePosition
        {
            get { return prevStateMS.Position; }
        }

        /// <summary>
        /// Return the current mouse position.
        /// </summary>
        public static Point CurrentMousePosition
        {
            get { return currStateMS.Position; }
        }
        /// <summary>
        /// Determines if the InputManager class has been initialized already.
        /// </summary>
        public static bool Initialized
        {
            get { return initialized; }
        }
        
        /// <summary>
        /// Flag for whether or not the cursor is visible/drawn to the screen.
        /// </summary>
        public static bool IsCursorVisible
        {
            get; set;
        }

        /// <summary>
        /// Flag for whether or not debug information should be printed.
        /// </summary>
        public static bool Debug
        {
            get; set;
        }

        #endregion

        #region Methods. // A range of methods called by the InputManager class.

        #region Initialization methods. // These methods are called near the start of the program.

        /// <summary>
        /// Initialize the InputManager with default values and constructor calls.
        /// </summary>
        public static void Initialize(bool _cursor = true, bool _debug = false)
        {
            currStateKB = Keyboard.GetState();
            prevStateKB = currStateKB;

            currStateMS = Mouse.GetState();
            prevStateMS = currStateMS;

            mouseColor = Color.Red;

            rng = new Random();

            initialized = true;

            IsCursorVisible = _cursor;
            Debug = _debug;
        }

        #endregion

        #region Game Loop methods. // Calls the Update and Draw methods.

        #region Update methods. // Updates the keyboard and mouse states.

        /// <summary>
        /// Updates the keyboard and mouse if initialized.
        /// </summary>
        /// <param name="gameTime">Timespan information taken from the main game class.</param>
        public static void Update(GameTime gameTime)
        {
            if (initialized)
            {
                UpdateKeyboard(gameTime); // Update Keyboard.
                UpdateMouse(gameTime); // Update Mouse.
            }
        }

        /// <summary>
        /// Updates the keyboard states.
        /// </summary>
        /// <param name="gameTime">Timespan information taken from the main game class.</param>
        private static void UpdateKeyboard(GameTime gameTime)
        {
            prevStateKB = currStateKB;
            currStateKB = Keyboard.GetState();
        }

        /// <summary>
        /// Updates the mouse states.
        /// </summary>
        /// <param name="gameTime">Timespan information taken from the main game class.</param>
        private static void UpdateMouse(GameTime gameTime)
        {
            prevStateMS = currStateMS;
            currStateMS = Mouse.GetState();

            UpdateMouseColor();
        }

        /// <summary>
        /// Updates the mouse colors based on button press status.
        /// </summary>
        private static void UpdateMouseColor()
        {
            // Default mouse cursor color.
            mouseColor = Color.CornflowerBlue;

            if (LeftButtonDown && RightButtonDown && MiddleButtonDown)
            {
                mouseColor = Blend(Color.Red, Color.LimeGreen, Color.Blue);
            }
            else if (LeftButtonDown && RightButtonDown)
            {
                mouseColor = Blend(Color.Red, Color.Blue);
            }
            else if (LeftButtonDown && MiddleButtonDown)
            {
                mouseColor = Blend(Color.Red, Color.LimeGreen);
            }
            else if (RightButtonDown && MiddleButtonDown)
            {
                mouseColor = Blend(Color.Blue, Color.LimeGreen);
            }
            else if (LeftButtonDown)
            {
                mouseColor = Color.Red;
            }
            else if (MiddleButtonDown)
            {
                mouseColor = Color.LimeGreen;
            }
            else if (RightButtonDown)
            {
                mouseColor = Color.Blue;
            }
        }

        #endregion

        #region Draw methods. // Draw calls for the cursor and any debug information, if necessary.
        
        /// <summary>
        /// Draw the cursor.
        /// </summary>
        /// <param name="shapeDrawer">Uses the shapedrawer to draw the cursor.</param>
        public static void Draw(ShapeDrawer shapeDrawer)
        {
            if (initialized && IsCursorVisible)
            {
                shapeDrawer.DrawMouseCursor(CurrentMousePosition.X, CurrentMousePosition.Y, mouseColor);
            }
        }

        /// <summary>
        /// Draw the debug information.
        /// </summary>
        /// <param name="shapeDrawer">Uses the shapedrawer to draw the debug string.</param>
        public static void DrawGUI(ShapeDrawer shapeDrawer)
        {
            if (initialized && Debug)
            {
                int h = (int)shapeDrawer.Font.MeasureString("A").Y;
                shapeDrawer.DrawString(10, 10, StateManager.DrawColor, "Mouse Position: " + CurrentMousePosition.ToString());
            }
        }

        #endregion

        #endregion        

        #region Keyboard methods. // These methods use the keyboard states to determine requested key states.

        /// <summary>
        /// Checks if a key in the list matches the keypress response type.
        /// </summary>
        /// <param name="keys">List of keys to check.</param>
        /// <param name="action">The keypress type to look for.</param>
        /// <returns>Returns true if any one of the key matches.</returns>
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

        /// <summary>
        /// Checks if the key's current state matches the keypress response provided.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <param name="action">Keypress response to check for.</param>
        /// <returns>Returns true if the key matches.</returns>
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

        /// <summary>
        /// Using a control scheme and a command, this checks to see if all of the given keys associated with the command are up.
        /// </summary>
        /// <param name="schema">Schema to get information from.</param>
        /// <param name="command">Command to get key association for.</param>
        /// <returns>Returns true if all keys are up.</returns>
        public static bool IsKeyUp(ControlScheme schema, Commands command)
        {
            List<Keys> keys;

            if (schema.GetCommandKeys(command, out keys))
            {
                foreach (Keys key in keys)
                {
                    if (IsKeyDown(key))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Using a control scheme and a command, this checks to see if any of the given keys associated with the command are down.
        /// </summary>
        /// <param name="schema">Schema to get information from.</param>
        /// <param name="command">Command to get key association for.</param>
        /// <returns>Returns true if any key is down.</returns>
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

        /// <summary>
        /// Using a control scheme and a command, this checks to see if any of the given keys associated with the command are being held down.
        /// </summary>
        /// <param name="schema">Schema to get information from.</param>
        /// <param name="command">Command to get key association for.</param>
        /// <returns>Returns true if any key is being held down.</returns>
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

        /// <summary>
        /// Using a control scheme and a command, this checks to see if any of the given keys associated with the command have just been pressed.
        /// </summary>
        /// <param name="schema">Schema to get information from.</param>
        /// <param name="command">Command to get key association for.</param>
        /// <returns>Returns true if any key has just been pressed.</returns>
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

        /// <summary>
        /// Using a control scheme and a command, this checks to see if any of the given keys associated with the command has just been released.
        /// </summary>
        /// <param name="schema">Schema to get information from.</param>
        /// <param name="command">Command to get key association for.</param>
        /// <returns>Returns true if any key has just been released.</returns>
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

        /// <summary>
        /// Determines if a key is up during the current frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Return true if the key is up during the current keyboard state.</returns>
        public static bool IsKeyUp(Keys key)
        {
            // If key is up during current frame, return true.
            return (currStateKB.IsKeyUp(key));
        }

        /// <summary>
        /// Determines if a key is down during the current frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Return true if the key is down during the current keyboard state.</returns>
        public static bool IsKeyDown(Keys key)
        {
            // If key is down during the current frame, return true.
            return (currStateKB.IsKeyDown(key));
        }

        /// <summary>
        /// Determines if a key is down during the current frame and previous frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Return true if the key is down during the previous and current keyboard states.</returns>
        public static bool IsKeyHeld(Keys key)
        {
            // If the key is down during previous and current frames, return true.
            return (prevStateKB.IsKeyDown(key) && IsKeyDown(key));
        }

        /// <summary>
        /// Determines if a key is up during the previous frame and down during the current frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Return true if the key is up during the previous frame and down during the current frame.</returns>
        public static bool IsKeyPressed(Keys key)
        {
            // If the key is up during previous frame and down during current, return true.
            return (prevStateKB.IsKeyUp(key) && IsKeyDown(key));
        }

        /// <summary>
        /// Determines if a key is down during the previous frame and up during the current frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Return true if the key is down during the previous frame and up during the current frame.</returns>
        public static bool IsKeyReleased(Keys key)
        {
            // If the key is down during the previous frame and up during current, return true.
            return (prevStateKB.IsKeyDown(key) && IsKeyUp(key));
        }

        #endregion

        #region Mouse methods. // These methods use the mouse states to determine requested mouse button states.

        #region Mouse State properties. // Mouse button states and functions.

        /// <summary>
        /// Determines if the left button is down.
        /// </summary>
        /// <returns>Returns true if the left button is down.</returns>
        public static bool LeftButtonDown
        {
            get { return IsButtonDown(MouseButton.Left); }
        }

        /// <summary>
        /// Determines if the left button is up.
        /// </summary>
        /// <returns>Returns true if the left button is up.</returns>
        public static bool LeftButtonUp
        {
            get { return IsButtonUp(MouseButton.Left); }
        }
        
        /// <summary>
        /// Determines if the left button is being held.
        /// </summary>
        /// <returns>Returns true if the left button is being held.</returns>
        public static bool LeftButtonHeld
        {
            get { return IsButtonHeld(MouseButton.Left); }
        }

        /// <summary>
        /// Determines if the left button has just been pressed.
        /// </summary>
        /// <returns>Returns true if the left button has been pressed.</returns>
        public static bool LeftButtonPressed
        {
            get { return IsButtonPressed(MouseButton.Left); }
        }

        /// <summary>
        /// Determines if the left button has just been released.
        /// </summary>
        /// <returns>Returns true if the left button has been released.</returns>
        public static bool LeftButtonReleased
        {
            get { return IsButtonReleased(MouseButton.Left); }
        }

        /// <summary>
        /// Determines if the right button is down.
        /// </summary>
        /// <returns>Returns true if the right button is down.</returns>
        public static bool RightButtonDown
        {
            get { return IsButtonDown(MouseButton.Right); }
        }

        /// <summary>
        /// Determines if the right button is up.
        /// </summary>
        /// <returns>Returns true if the right button is up.</returns>
        public static bool RightButtonUp
        {
            get { return IsButtonUp(MouseButton.Right); }
        }

        /// <summary>
        /// Determines if the right button is being held.
        /// </summary>
        /// <returns>Returns true if the right button is being held.</returns>
        public static bool RightButtonHeld
        {
            get { return IsButtonHeld(MouseButton.Right); }
        }

        /// <summary>
        /// Determines if the right button has just been pressed.
        /// </summary>
        /// <returns>Returns true if the right button has been pressed.</returns>
        public static bool RightButtonPressed
        {
            get { return IsButtonPressed(MouseButton.Right); }
        }

        /// <summary>
        /// Determines if the right button has just been released.
        /// </summary>
        /// <returns>Returns true if the right button has been released.</returns>
        public static bool RightButtonReleased
        {
            get { return IsButtonReleased(MouseButton.Right); }
        }

        /// <summary>
        /// Determines if the middle button is down.
        /// </summary>
        /// <returns>Returns true if the middle button is down.</returns>
        public static bool MiddleButtonDown
        {
            get { return IsButtonDown(MouseButton.Middle); }
        }

        /// <summary>
        /// Determines if the middle button is up.
        /// </summary>
        /// <returns>Returns true if the middle button is up.</returns>
        public static bool MiddleButtonUp
        {
            get { return IsButtonUp(MouseButton.Middle); }
        }

        /// <summary>
        /// Determines if the middle button is being held.
        /// </summary>
        /// <returns>Returns true if the middle button is being held.</returns>
        public static bool MiddleuttonHeld
        {
            get { return IsButtonHeld(MouseButton.Middle); }
        }

        /// <summary>
        /// Determines if the middle button has just been pressed.
        /// </summary>
        /// <returns>Returns true if the middle button has been pressed.</returns>
        public static bool MiddleButtonPressed
        {
            get { return IsButtonPressed(MouseButton.Middle); }
        }

        /// <summary>
        /// Determines if the middle button has just been released.
        /// </summary>
        /// <returns>Returns true if the middle button has been released.</returns>
        public static bool MiddleButtonReleased
        {
            get { return IsButtonReleased(MouseButton.Middle); }
        }

        /// <summary>
        /// Determines if the button is down.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Returns true if the button is down.</returns>
        private static bool IsButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (currStateMS.LeftButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (currStateMS.RightButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (currStateMS.MiddleButton == ButtonState.Pressed);
                case MouseButton.Null:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the button is up.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Returns true if the button is up.</returns>
        private static bool IsButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (currStateMS.LeftButton == ButtonState.Released);
                case MouseButton.Right:
                    return (currStateMS.RightButton == ButtonState.Released);
                case MouseButton.Middle:
                    return (currStateMS.MiddleButton == ButtonState.Released);
                case MouseButton.Null:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the button is being held.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Returns true if the button is being held.</returns>
        private static bool IsButtonHeld(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (prevStateMS.LeftButton == ButtonState.Pressed) && IsButtonDown(button);
                case MouseButton.Right:
                    return (prevStateMS.RightButton == ButtonState.Pressed) && IsButtonDown(button);
                case MouseButton.Middle:
                    return (prevStateMS.MiddleButton == ButtonState.Pressed) && IsButtonDown(button);
                case MouseButton.Null:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the button has just been pressed.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Returns true if the button has been pressed.</returns>
        private static bool IsButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (prevStateMS.LeftButton == ButtonState.Released) && IsButtonDown(button);
                case MouseButton.Right:
                    return (prevStateMS.RightButton == ButtonState.Released) && IsButtonDown(button);
                case MouseButton.Middle:
                    return (prevStateMS.MiddleButton == ButtonState.Released) && IsButtonDown(button);
                case MouseButton.Null:
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Determines if the button has just been released.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Returns true if the button has been released.</returns>
        private static bool IsButtonReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (prevStateMS.LeftButton == ButtonState.Pressed) && IsButtonUp(button);
                case MouseButton.Right:
                    return (prevStateMS.RightButton == ButtonState.Pressed) && IsButtonUp(button);
                case MouseButton.Middle:
                    return (prevStateMS.MiddleButton == ButtonState.Pressed) && IsButtonUp(button);
                case MouseButton.Null:
                default:
                    return false;
            }
        }

        #endregion

        #region Mouse Collision and Position. // Position of the mouse and cursor collision functions.

        /// <summary>
        /// Get the current position of the mouse.
        /// </summary>
        /// <returns>Returns Point of current mouse position.</returns>
        public static Point GetMousePosition()
        {
            return CurrentMousePosition;
        }

        /// <summary>
        /// Determines if the mouse is currently colliding with the input boundaries.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the current mouse position is colliding with the bounds.</returns>
        public static bool MouseCollision(Rectangle bounds)
        {
            // Is the mouse cursor colliding with the bounds?
            return (CurrentMouseCollision(bounds));
        }

        /// <summary>
        /// Determines if the mouse just collided with the bounds.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the previous mouse position collision was false and current is true.</returns>
        public static bool OnEnter(Rectangle bounds)
        {
            // If the previous mouse position collision was false and current is true, return true.
            return (!PreviousMouseCollision(bounds) && CurrentMouseCollision(bounds));
        }

        /// <summary>
        /// Determines if the mouse just stopped colliding with the bounds.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the previous mouse position collision was true and current is false.</returns>
        public static bool OnExit(Rectangle bounds)
        {
            // If the previous mouse position collision was true and current is false, return true.
            return (PreviousMouseCollision(bounds) && !CurrentMouseCollision(bounds));
        }

        /// <summary>
        /// Determines if the mouse is still colliding with the bounds.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the previous mouse position collision was true and current is true.</returns>
        public static bool OnHover(Rectangle bounds)
        {
            // If the previous mouse position collision is true and current is true, return true.
            return (PreviousMouseCollision(bounds) && CurrentMouseCollision(bounds));
        }

        /// <summary>
        /// Check for mouse collision, of the current position.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the current mouse position is colliding with the bounds.</returns>
        private static bool CurrentMouseCollision(Rectangle bounds)
        {
            // If the x and y of the mouse cursor is within the bounds, return true.
            return bounds.Contains(CurrentMousePosition);
        }

        /// <summary>
        /// Check for mouse collision, of the previous position.
        /// </summary>
        /// <param name="bounds">Boundaries to check.</param>
        /// <returns>Returns true if the previous mouse position is colliding with the bounds.</returns>
        private static bool PreviousMouseCollision(Rectangle bounds)
        {
            // If the x and y of the mouse cursor is within the bounds, return true.
            return bounds.Contains(PreviousMousePosition);
        }

        #endregion

        #endregion

        #region Helper Functions. // These are service functions that perform different tasks.

        /// <summary>
        /// Generate a positive or negative one to multiply.
        /// </summary>
        /// <returns>Returns a positive or negative 1.</returns>
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

        /// <summary>
        /// Gives a value a positive or negative sign.
        /// </summary>
        /// <param name="value">Input value to give a sign to.</param>
        /// <returns>Returns a positive or negative value.</returns>
        public static int GetSign(int value)
        {
            return GetSign() * Math.Abs(value);
        }

        /// <summary>
        /// Gives a value a positive or negative sign.
        /// </summary>
        /// <param name="value">Input value to give a sign to.</param>
        /// <returns>Returns a positive or negative value.</returns>
        public static float GetSign(float value)
        {
            return GetSign() * Math.Abs(value);
        }

        /// <summary>
        /// Gives a value a positive or negative sign.
        /// </summary>
        /// <param name="value">Input value to give a sign to.</param>
        /// <returns>Returns a positive or negative value.</returns>
        public static double GetSign(double value)
        {
            return GetSign() * Math.Abs(value);
        }

        /// <summary>
        /// Gives a value a positive or negative sign.
        /// </summary>
        /// <param name="value">Input value to give a sign to.</param>
        /// <returns>Returns a positive or negative value.</returns>
        public static long GetSign(long value)
        {
            return GetSign() * Math.Abs(value);
        }

        /// <summary>
        /// Range returns a value between a minimum and maximum,
        /// with both values being inclusive.
        /// </summary>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="inclusiveMaximum">Maximum value.</param>
        /// <returns>Returns an integer between minimum and inclusiveMaximum.</returns>
        public static int Range(int minimum, int inclusiveMaximum)
        {
            return rng.Next(minimum, inclusiveMaximum + 1);
        }

        /// <summary>
        /// Returns the sum of values from a list of values.
        /// </summary>
        /// <param name="values">List of values to process.</param>
        /// <returns>Returns the sum of values.</returns>
        public static float Sum(List<float> values)
        {
            if (values == null || values.Count() == 0)
            {
                return 0;
            }

            float result = 0.0f;

            foreach (float value in values)
            {
                result += value;
            }

            return result;
        }

        /// <summary>
        /// Returns the sum of values from an array of values.
        /// </summary>
        /// <param name="values">Array of values to process.</param>
        /// <returns>Returns the sum of values.</returns>
        public static float Sum(params float[] values)
        {
            if (values == null || values.Length == 0)
            {
                return 0;
            }

            float result = 0.0f;

            foreach (float value in values)
            {
                result += value;
            }

            return result;
        }
        
        /// <summary>
        /// Returns the average from a list of values.
        /// </summary>
        /// <param name="values">List of values to process.</param>
        /// <returns>Returns the average value.</returns>
        public static float Average(List<float> values)
        {
            if (values == null || values.Count() == 0)
            {
                return 0;
            }

            return Sum(values) / values.Count();
        }

        /// <summary>
        /// Returns the average from an array of values.
        /// </summary>
        /// <param name="values">Array of values to process.</param>
        /// <returns>Returns the average value.</returns>
        public static float Average(params float[] values)
        {
            if (values == null || values.Length == 0)
            {
                return 0;
            }

            return Sum(values) / values.Length;
        }
        
        /// <summary>
        /// Returns the minimum from an array of values.
        /// </summary>
        /// <param name="values">Array of values to process.</param>
        /// <returns>Returns the minimum value.</returns>
        public static float Min(params float[] values)
        {
            float min = 0.0f;

            if (values == null || values.Count() == 0)
            {
                return min;
            }

            min = values[0];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                }
            }

            return min;
        }

        /// <summary>
        /// Returns the maximum from an array of values.
        /// </summary>
        /// <param name="values">Array of values to process.</param>
        /// <returns>Returns the maximum value.</returns>
        public static float Max(params float[] values)
        {
            float max = 0.0f;

            if (values == null || values.Count() == 0)
            {
                return max;
            }

            max = values[0];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                }
            }

            return max;
        }

        /// <summary>
        /// Blends two colors together, putting a limit on the values based on the individual color range.
        /// </summary>
        /// <param name="one">One of the colors being blended.</param>
        /// <param name="two">Second color being blended.</param>
        /// <returns>Returns a color between the first and second values.</returns>
        public static Color Blend(Color one, Color two)
        {
            float minR = Min(one.R, two.R);
            float maxR = Max(one.R, two.R);

            float minG = Min(one.G, two.G);
            float maxG = Max(one.G, two.G);

            float minB = Min(one.B, two.B);
            float maxB = Max(one.B, two.B);

            float min = MathHelper.Clamp(Min(minR, minG, minB), 0, 255);
            float max = MathHelper.Clamp(Max(maxR, maxG, maxB), 0, 255);

            float R = MathHelper.Clamp(Average(one.R, two.R), min, max);
            float G = MathHelper.Clamp(Average(one.G, two.G), min, max);
            float B = MathHelper.Clamp(Average(one.B, two.B), min, max);

            return new Color(R, G, B);
        }

        /// <summary>
        /// Blends a series of colors together.
        /// </summary>
        /// <param name="colors">Array of colors to blend.</param>
        /// <returns>Returns a color between the first and second values.</returns>
        public static Color Blend(params Color[] colors)
        {
            if (colors == null || colors.Length == 0)
            {
                return Color.White; // If null, return white.
            }

            Color blend = colors[0];

            if (colors.Length == 1)
            {
                return blend; // If only one color, return input color.
            }
            
            foreach (Color col in colors)
            {
                blend = Blend(blend, col);
            }

            return blend;
        }

        #endregion

        #endregion

    }
}
