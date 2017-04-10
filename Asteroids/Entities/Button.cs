/*
    Button.cs
    Author: Ian Effendi
    Date: 2.13.2017
    Description: A standalone button class that can be used to add buttons to the game.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add the XNA using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Add the project using statements.
using Asteroids.Tools;

namespace Asteroids.Entities
{
	public enum Positions
	{
		TopLeft,
		TopCenter,
		TopRight,
		CenterLeft,
		Center,
		CenterRight,
		BottomLeft,
		BottomCenter,
		BottomRight,
		Absolute // Absolute means the button isn't relative to any screen position.
	}

	public class Button
    {
        // Enum.

        // Fields.
        // Use these fields to store important attributes.
        ShapeDrawer pen; // The shape drawer.
        Texture2D image; // Hold the button image.
        Actions action; // The action this button performs.
        Positions location;

        Vector2 offset; // Offset of the button.
        Vector2 position; // Final position of the button.
        Vector2 bounds; // Boundaries of the button.
        Vector2 screen; // Keeps track for changes.

        Rectangle source; // Source dimensions of the button.
        Color drawColor; // The color to draw the button.
        Color textColor; // The color to draw the text.

        string message; // Message.
        bool enabled; // Is the button enabled?
        
        // Properties.
        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public bool Disabled
        {
            get { return !this.enabled; }
            set { this.enabled = !value; }
        }

        public Actions Action
        {
            get { return this.action; }
        }

        // Used for drawing and checking user input.
        public Rectangle Dimensions
        {
            get { return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.bounds.X, (int)this.bounds.Y); }
        }

        public bool HasText
        {
            get { return (this.message != ""); }
        }

        public bool HasButtonTexture
        {
            // If the image isn't equal to null, it has a texture.
            get { return this.image != null; }
        }

        public Vector2 HalfSize
        {
            get { return new Vector2(bounds.X / 2, bounds.Y / 2); }
        }

        // Constructors.
        public Button(Actions _action, ShapeDrawer _pen,
            Positions _location = Positions.Center, Vector2? _offset = null, Vector2? _bounds = null, 
            Texture2D buttonTexture = null, string buttonText = null)
        {
            // Set action.
            this.action = _action;

            // Set the shape drawer.
            pen = _pen;

            // Set the message.
            if (buttonText == null)
            {
                this.message = "";
            }
            else
            {
                this.message = buttonText;
            }

            // When given no button texture, use the shapedrawer to draw the button.
            if (buttonTexture != null)
            {
                this.image = buttonTexture;
                this.source = new Rectangle(0, 0, image.Width, image.Height);
            }
            else
            {
                this.image = null;
                this.source = new Rectangle(0, 0, 1, 1);
            }

            // Set boundaries.
            if (_bounds == null || ((Vector2)_bounds) == Vector2.Zero || ((Vector2)_bounds).Length() == 0)
            {
                if (HasButtonTexture)
                {
                    // If a button exists.
                    this.bounds = new Vector2(this.image.Width, this.image.Height);
                }
                else
                {
                    // If a button doesn't exist.
                    this.bounds = new Vector2(135, 45);
                }
            }
            else
            {
                this.bounds = (Vector2) _bounds;
            }

            // Fit the text, if it exists.
            this.bounds = FitText(this.bounds);

            // Set the position.
            if (_offset == null || ((Vector2)_offset) == Vector2.Zero || ((Vector2)_offset).Length() == 0)
            {
                SetPosition(_location);
            }
            else
            {
                SetPosition(_location, (Vector2)_offset);
            }

        }

        private void SetPosition(Positions loc, Vector2 offset)
        {
            this.location = loc;
            this.offset = offset;
            SetPosition(offset);
        }

        private void SetPosition(Positions loc)
        {
            SetPosition(loc, new Vector2(0.0f, 0.0f));
        }

        private void SetPosition(Vector2 o)
        {
            Vector2 c = GlobalManager.ScreenCenter;
            Vector2 b = GlobalManager.ScreenBounds;
            Vector2 h = HalfSize;
            screen = b;
            
            switch (this.location)
            {
                case Positions.TopLeft:
                    this.position = new Vector2(h.X * 2, h.Y * 2) + o;
                    break;
                case Positions.TopCenter:
                    this.position = new Vector2(c.X - h.X, h.Y * 2) + o;
                    break;
                case Positions.TopRight:
                    this.position = new Vector2(b.X - (h.X * 2), h.Y * 2) + o;
                    break;
                case Positions.CenterLeft:
                    this.position = new Vector2((h.X * 2), c.Y - h.Y) + o;
                    break;
                case Positions.Center:
                    this.position = new Vector2(c.X - h.X, c.Y - h.Y) + o;
                    break;
                case Positions.CenterRight:
                    this.position = new Vector2(b.X - (h.X * 2), c.Y - h.Y) + o;
                    break;
                case Positions.BottomLeft:
                    this.position = new Vector2((h.X * 2), b.Y - (h.Y * 2)) + o;
                    break;
                case Positions.BottomCenter:
                    this.position = new Vector2(c.X - h.X, b.Y - (h.Y * 2)) + o;
                    break;
                case Positions.BottomRight:
                    this.position = new Vector2(b.X - (h.X * 2), b.Y - (h.Y * 2)) + o;
                    break;
                case Positions.Absolute:
                default:
                    this.position = o;
                    break;
            }
        }

        public void CalculatePosition()
        {
            // Updates the position.
            SetPosition(this.location, this.offset);
        }

        private Vector2 FitText(Vector2 _bounds)
        {
           return FitText(_bounds, 30f);
        }

        private Vector2 FitText(Vector2 _bounds, float padding)
        {
            // If it has text.
            if (HasText)
            {
                Vector2 strDim = pen.StringDimensions(message);

                float width = Math.Max(strDim.X + padding, _bounds.X);
                float height = Math.Max(strDim.Y + padding, _bounds.Y);

                return new Vector2(width, height);
            }

            // If there is no text, just return the parameter.
            return _bounds;
        }
        
        public bool IsMouseInsideButton()
        {
            // Use the InputManager function.
            return InputManager.MouseCollision(Dimensions);
        }

        // Is the button being hovered over?
        public bool OnHover()
        {
            return InputManager.OnHover(Dimensions);
        }

        // Did the mouse just enter over the button?
        public bool OnEnter()
        {
            return InputManager.OnEnter(Dimensions);
        }

        // Did the mouse just exit from over the button?
        public bool OnExit()
        {
            return InputManager.OnExit(Dimensions);
        }

        // Is the button just pressed?
        public bool IsClicked()
        {
            // If the mouse is inside the button, and just the left button was just pressed.
            return (IsMouseInsideButton() && InputManager.LeftButtonPressed);
        }

        // Is the button held?
        public bool IsHeld()
        {
            return (IsMouseInsideButton() && InputManager.LeftButtonHeld);
        }

        // Is the button just released?
        public bool IsReleased()
        {
            if (!IsMouseInsideButton())
            {
                return false; // If not inside the button, return false.
            }
            else
            {
                return (IsMouseInsideButton() && InputManager.LeftButtonReleased);
            }
        }

        // Update the button.
        public void Update(GameTime gameTime)
        {
            // Recalculate the position.
            if (screen != GlobalManager.ScreenBounds)
            {
                CalculatePosition();
            }

            // Default colors.
            drawColor = Color.DimGray;
            this.textColor = Color.Gold;

            if (Enabled)
            {
                if (OnEnter())
                {
                    drawColor = Color.Beige;
                    this.textColor = Color.Gray;
                }

                if (OnHover())
                {
                    drawColor = Color.White;
                    this.textColor = Color.Black;
                }

                if (OnExit())
                {
                    drawColor = Color.DimGray;
                    this.textColor = Color.Gold;
                }

                if (IsReleased())
                {
                    drawColor = Color.GhostWhite;
                    this.textColor = Color.Gold;
                }

                if (IsClicked() || IsHeld())
                {
                    drawColor = Color.Gold;
                    this.textColor = Color.White;
                }
            }
            else
            {
                drawColor = Color.Gray;
                this.textColor = Color.DimGray;
            }
        }

        public void Draw()
        {
            if (Enabled)
            {
                if (HasButtonTexture)
                {
                    pen.Pen.Draw(image, Dimensions, source, drawColor, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                }
                else
                {
                    pen.DrawRectFilled(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height, drawColor);
                    pen.DrawRectOutline(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height, textColor);
                }
                                
                if (HasText)
                {
                    Vector2 text = this.position + HalfSize + new Vector2(0, -(pen.StringHeight(message) / 2));
                    pen.DrawString((int)text.X, (int)text.Y, textColor, message, ShapeDrawer.CENTER_ALIGN);
                }
            }
        }

        public void DrawGUI()
        {
            if (Enabled)
            {
                int height = (int) pen.Font.MeasureString("A").Y;

                Color debugColor = StateManager.DrawColor;

                pen.DrawString(10, 10 + height, debugColor, "Test button information:");
                pen.DrawString(10, 10 + (height * 2), debugColor, "Button Color: ");
                int width = (int)pen.Font.MeasureString("Button Color: ").X;
                pen.DrawString(10 + width, 10 + (height * 2), drawColor, drawColor.ToString());
                pen.DrawString(10, 10 + (height * 3), debugColor, "Button Size: " + new Vector2(Dimensions.Width, Dimensions.Height).ToString());
                pen.DrawString(10, 10 + (height * 4), debugColor, "Button Location: " + new Vector2(Dimensions.X, Dimensions.Y).ToString());
                pen.DrawString(10, 10 + (height * 5), debugColor, "Button Message: ");
                width = pen.StringWidth("Button Message: ");
                pen.DrawString(10 + width, 10 + (height * 5), textColor, message);
            }
        }
    }
}
