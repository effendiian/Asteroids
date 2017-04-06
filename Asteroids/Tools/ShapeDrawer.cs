/*
    ShapeDrawer.cs
    Author: Ian Effendi
    Date: 2.7.2017
    Description: A helper class to simplify drawing several common shapes. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add the XNA using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Tools
{
    public class ShapeDrawer
    {
        // Fields.
        // Use these fields to store important attributes.
        SpriteBatch sb; // Field to hold thet sprite batch object.
        SpriteFont font; // Print messages to the GUI.
        Texture2D dot; // A 1x1 white Texture2D.

        // Properties.
        public Texture2D Dot
        {
            get { return this.dot; }
        }

        public SpriteFont Font
        {
            get { return this.font; }
        }
        
        public SpriteBatch Pen
        {
            get { return this.sb; }
        }

        // Constructor.
        public ShapeDrawer(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphics)
        {
            // Assign the sprite batch reference to the sb field.
            this.sb = spriteBatch;

            // Assign the spritefont.
            this.font = spriteFont;

            // Create the texture.
            this.dot = new Texture2D(graphics, 1, 1);

            // Assign the color.
            this.dot.SetData<Color>(new Color[1] { Color.White });
        }

        // Service Methods.
        /// <summary>
        /// Draw a single point of a specified color.
        /// </summary>
        /// <param name="x">Point, x.</param>
        /// <param name="y">Point, y.</param>
        /// <param name="color">Color to draw the point.</param>
        public void DrawPoint(int x, int y, Color color)
        {
            // Draw straight to the canvas with SpriteBatch.
            sb.Draw(dot, new Vector2(x, y), color);
        }

        public void DrawRectAroundPoint(int x, int y, int thickness, Color color)
        {
            // thickness is the size of the square.
            DrawRectFilled(x - (thickness / 2), y - (thickness / 2), thickness, thickness, color);
        }
        public void DrawRectOutlineAroundPoint(int x, int y, int thickness, Color color)
        {
            // thickness is the size of the square.
            DrawRectOutline(x - (thickness / 2), y - (thickness / 2), thickness, thickness, color);
        }

        /// <summary>
        /// Draw a line from one point to another.
        /// </summary>
        /// <param name="x0">Origin point, x.</param>
        /// <param name="y0">Origin point, y.</param>
        /// <param name="x1">End point, x.</param>
        /// <param name="y1">End point, y.</param>
        /// <param name="thickness">Thickness of a given line.</param>
        /// <param name="color">Color to draw the line.</param>
        public void DrawLine(int x0, int y0, int x1, int y1, int thickness, Color color)
        {
            // Calculate the length of the line.
            float length = Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1));

            // Calculate the angle between the desired line and the x axis.
            float angle = (float)Math.Atan2(y1 - y0, x1 - x0);

            // Create the rectangle that you'd like to draw.
            Rectangle rect = new Rectangle(x0, y0, (int)length, thickness);

            // Draw the rotated rectangle.
            sb.Draw(dot, rect, null, color, angle, new Vector2(0, 0.5f), SpriteEffects.None, 0.0f);   
        }

        /// <summary>
        /// Draw a filled rectangle using the specified parameters.
        /// </summary>
        /// <param name="x">Top left point, x.</param>
        /// <param name="y">Top left point, y.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to draw the rectangle.</param>
        public void DrawRectFilled(int x, int y, int width, int height, Color color)
        {
            // Get a rectangle of the specified dimensions.
            Rectangle rect = new Rectangle(x, y, width, height);

            // Draw the rectangle.
            sb.Draw(dot, rect, color);
        }

        /// <summary>
        /// Draw the outline of a rectangle, using the specified parameters.
        /// </summary>
        /// <param name="x">Top left point, x.</param>
        /// <param name="y">Top left point, y.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to draw the rectangle outline.</param>
        public void DrawRectOutline(int x, int y, int width, int height, Color color)
        {
            // Draw the first line. Top left to top right.
            DrawLine(x - 1, y - 1, x + width, y, 1, color);

            // Draw the second line. Top right to bottom right.
            DrawLine(x + width, y, x + width, y + height, 1, color);

            // Draw the third line. Bottom right to bottom left.
            DrawLine(x + width, y + height, x, y + height, 1, color);

            // Draw the fourth line. Bottom left to top left.
            DrawLine(x, y + height, x, y, 1, color);
        }

        /// <summary>
        /// Draw the mouse cursor.
        /// </summary>
        /// <param name="x">Mouse point, x.</param>
        /// <param name="y">Mouse point, y.</param>
        /// <param name="color">Color to draw the mouse cursor.</param>
        public void DrawMouseCursor(int x, int y, Color color)
        {
            DrawPoint(x, y, color);
            DrawPoint(x + 1, y, color);
            DrawPoint(x + 1, y + 1, color);
            DrawPoint(x, y + 1, color);
            DrawPoint(x - 1, y - 1, color);
            DrawPoint(x, y - 1, color);
            DrawPoint(x - 1, y, color);
            DrawPoint(x - 1, y + 1, color);
            DrawPoint(x + 1, y - 1, color);
        }

        // Consts.
        public const int LEFT_ALIGN = 0;
        public const int RIGHT_ALIGN = 1;
        public const int CENTER_ALIGN = 2;
        
        private void DrawStringLeftAligned(SpriteFont f, int x, int y, Color color, string message)
        {
            sb.DrawString(f, message, new Vector2(x, y), color);
        }

        private void DrawStringRightAligned(SpriteFont f, int x, int y, Color color, string message)
        {
            // Length of the string.
            Vector2 rightPos = new Vector2(x - (StringWidth(message, f)), y); // Don't change the height.

            sb.DrawString(f, message, rightPos, color);
        }

        private void DrawStringCentered(SpriteFont f, int x, int y, Color color, string message)
        {
            // Length of the string.
            Vector2 centerPos = new Vector2(x - (StringWidth(message, f) / 2), y); // Don't change the height.

            sb.DrawString(f, message, centerPos, color);
        }

        /// <summary>
        /// Draw to the GUI.
        /// </summary>
        /// <param name="x">Mouse point, x.</param>
        /// <param name="y">Mouse point, y.</param>
        /// <param name="color">Color to draw the mouse cursor.</param>
        /// <param name="message">Message to draw to the screen.</param>
        public void DrawString(int x, int y, Color color, string message, int alignment, SpriteFont _font = null)
        {
            SpriteFont f = this.font;

            if (_font != null)
            {
                f = _font;
            }

            switch (alignment)
            {
                case RIGHT_ALIGN:
                    DrawStringRightAligned(f, x, y, color, message);
                    break;
                case CENTER_ALIGN:
                    DrawStringCentered(f, x, y, color, message);
                    break;
                case LEFT_ALIGN:
                default:
                    DrawStringLeftAligned(f, x, y, color, message);
                    break;
            }
        }

        public void DrawString(int x, int y, Color color, string message, SpriteFont _font = null)
        {
            if (_font != null)
            {
                DrawStringLeftAligned(_font, x, y, color, message);
                return;
            }

            // When no alignment is provided, just default to left alignment.
            DrawStringLeftAligned(font, x, y, color, message);
        }

        public Vector2 StringDimensions(string message, SpriteFont _font = null)
        {
            if (_font != null)
            {
                return _font.MeasureString(message);
            }

            return Font.MeasureString(message);
        }

        public int StringWidth(string message, SpriteFont _font = null)
        {
            return (int) StringDimensions(message, _font).X;
        }

        public int StringHeight(string message, SpriteFont _font = null)
        {
            return (int) StringDimensions(message, _font).Y;
        }

    }
}
