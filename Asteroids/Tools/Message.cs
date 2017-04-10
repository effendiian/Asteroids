using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asteroids.Attributes;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Tools
{
    public class Message
    {

        // Add to the debug lines.
        public static List<Message> messagesToDraw;
        public static int highestOrder = 1;

        // Fields.
        string message;

        Vector2 position;
        Padding padding;
        Color drawColor;
        int alignment;
        float order;

        // Properties.
        public int Order
        {
            get { return (int)order; }
            set { this.order = value; }
        }

        public Padding Padding
        {
            get { return this.padding; }
            set { this.padding = value; }
        }

        // Constructor
        public static void QueueMessage(Message msg, int order)
        {
            msg.Order = order;
            QueueMessage(msg);
        }

        public static void QueueMessage(Message msg)
        {
            // If debugLine hasn't been made, create it.
            if (messagesToDraw == null || messagesToDraw.Count() == 0)
            {
                messagesToDraw = new List<Message>();
            }
            
            int index = Math.Abs(msg.Order - 1);

            if (index < 0 || index >= messagesToDraw.Count())
            {
                // Add to the end if the index doesn't exist yet.
                messagesToDraw.Add(msg);
            }
            else
            {
                // If the index already exists, insert it into the list at the given index.
                messagesToDraw.Insert(index, msg);
            }

            if (msg.Order > highestOrder)
            {
                highestOrder = msg.Order;
            }
        }

        public static void QueueMessage(string message, Vector2 basePosition, Padding padding, Color? color = null, int order = 1, int alignment = ShapeDrawer.LEFT_ALIGN)
        {
            // If the message is empty, do nothing.
            if(message == null || message == "") { return; }

            // If debugLine hasn't been made, create it.
            if (messagesToDraw == null || messagesToDraw.Count() == 0)
            {
                messagesToDraw = new List<Message>();
            }

            Message msg;

            if (color == null)
            {
                Color temp;
                temp = Color.White;
                msg = new Message(message, basePosition, padding, temp, order, alignment);
            }
            else
            {
                msg = new Message(message, basePosition, padding, (Color) color, order, alignment);
            }

            int index = Math.Abs(order - 1);

            if (index < 0 || index >= messagesToDraw.Count())
            {
                // Add to the end if the index doesn't exist yet.
                messagesToDraw.Add(msg);
            }
            else
            {
                // If the index already exists, insert it into the list at the given index.
                messagesToDraw.Insert(index, msg);
            }

            if (order > highestOrder)
            {
                highestOrder = order;
            }
        }

        public Message(string _str, Vector2 _pos, Padding _pad, Color _col, int _order = 1, int _alignment = ShapeDrawer.LEFT_ALIGN)
        {
            this.message = _str;
            this.drawColor = _col;
            this.order = _order;
            this.alignment = _alignment;
            this.position = _pos;
            this.padding = _pad;
        }

        // Draw
        public static void DrawMessage(Message msg)
        {
            msg.DrawMessage(msg.Order);
        }

        public static void DrawMessage(Message msg, SpriteFont font)
        {
            msg.DrawMessage(msg.Order, font);
        }

        public static void DrawMessages()
        {
            if(messagesToDraw == null || messagesToDraw.Count() == 0) { return; }

            for (int o = highestOrder; o > 0; o--)
            {
                List<Message> linesToRemove = new List<Message>();

                for (int i = 0; i < messagesToDraw.Count(); i++)
                {
                    if (messagesToDraw[i].Order == o)
                    {
                        messagesToDraw[i].DrawMessage(o);
                        messagesToDraw.Add(messagesToDraw[i]);
                    }
                }

                foreach (Message msg in linesToRemove)
                {
                    messagesToDraw.Remove(msg);
                }
            }

            // Empty the list, for new lines.
            messagesToDraw = new List<Message>();
        }
        public void DrawMessage(int order, SpriteFont font)
        {
            // Print message.
            GlobalManager.Pen.DrawString((int)position.X + (int)padding.GetX(order), (int)position.Y + (int)padding.GetY(order), drawColor, this.message, alignment, font);
        }

        public void DrawMessage(int order)
        {
			// Print message.
			GlobalManager.Pen.DrawString((int) position.X + (int) padding.GetX(order), (int) position.Y + (int) padding.GetY(order), drawColor, this.message, alignment);
        }
    }
}
