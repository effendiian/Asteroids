using Asteroids.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Tools
{
    public class DebugLine
    {
        // Add to the debug lines.
        public static List<DebugLine> linesToDraw;
        public static int highestOrder = 1;

        // Fields.
        public Vector2 position;

        public Color drawColor;
        public Vector2 vector;
        public float clamp;
        public float thickness;
        public float order;

        // Properties.
        public int Order
        {
            get { return (int)order; }
        }

        // Constructor.
        public static void CreateDebugLine(Entity _entity, Vector2 _vec, Color _col, float _mag = 10.0f, float _thick = 1f, int _order = 1)
        {
            // If debugLine hasn't been made, create it.
            if (linesToDraw == null || linesToDraw.Count() == 0)
            {
                linesToDraw = new List<DebugLine>();
            }

            DebugLine line = new DebugLine(_entity, _vec, _col, _mag, _thick, _order);

            int index = Math.Abs(_order - 1);

            if (index < 0 || index >= linesToDraw.Count())
            {
                // Add to the end if the index doesn't exist yet.
                linesToDraw.Add(line);
            }
            else
            {
                // If the index already exists, insert it into the list at the given index.
                linesToDraw.Insert(index, line);
            }

            if (_order > highestOrder)
            {
                highestOrder = _order;
            }

        }
        public static void CreateDebugLine(Vector2 _pos, Vector2 _vec, Color _col, float _mag = 10.0f, float _thick = 1f, int _order = 1)
        {
            // If debugLine hasn't been made, create it.
            if (linesToDraw == null || linesToDraw.Count() == 0)
            {
                linesToDraw = new List<DebugLine>();
            }

            DebugLine line = new DebugLine(_pos, _vec, _col, _mag, _thick, _order);

            int index = Math.Abs(_order - 1);

            if (index < 0 || index >= linesToDraw.Count())
            {
                // Add to the end if the index doesn't exist yet.
                linesToDraw.Add(line);
            }
            else
            {
                // If the index already exists, insert it into the list at the given index.
                linesToDraw.Insert(index, line);
            }

            if (_order > highestOrder)
            {
                highestOrder = _order;
            }

        }

        public DebugLine(Entity _entity, Vector2 _vec, Color _col, float _mag = 10.0f, float _thick = 1f, float _order = 1f)
        {            
            this.position = _entity.Position;
            this.drawColor = _col;
            this.vector = _vec;
            this.clamp = _mag;
            this.thickness = _thick;
            this.order = _order;
        }
        
        public DebugLine(Vector2 _pos, Vector2 _vec, Color _col, float _mag = 10.0f, float _thick = 1f, float _order = 1f)
        {
            this.position = _pos;
            this.drawColor = _col;
            this.vector = _vec;
            this.clamp = _mag;
            this.thickness = _thick;
            this.order = _order;
        }

        // Draw
        public static void DrawLines()
        {
            if (linesToDraw == null || linesToDraw.Count() == 0) { return; }

            for (int o = highestOrder; o > 0; o--)
            {
                List<DebugLine> linesToRemove = new List<DebugLine>();

                for (int i = 0; i < linesToDraw.Count(); i++)
                {
                    if (linesToDraw[i].Order == o)
                    {
                        linesToDraw[i].Draw();
                        linesToRemove.Add(linesToDraw[i]);
                    }
                }

                foreach (DebugLine line in linesToRemove)
                {
                    linesToDraw.Remove(line);
                }
            }
            // Empty the list, for new lines.
            linesToDraw = new List<DebugLine>();
        }

        private void Draw()
        {
            Vector2 line = position + (Vector2.Normalize(vector) * MathHelper.Clamp(vector.Length(), clamp / 2, clamp * 2));
            Game1.Pen.DrawLine((int)position.X, (int)position.Y, (int)line.X, (int)line.Y, (int)thickness, drawColor);
        }

        public static void Draw(DebugLine line, bool clamp = false)
        {
            Draw(line.position, line.vector, line.clamp, line.thickness, line.drawColor, clamp);
        }

        public static void Draw(Vector2 pos, Vector2 vec, float mag, float thick, Color col, bool clamp = false)
        {
            Vector2 line = pos;

            if (clamp)
            {
                line += (Vector2.Normalize(vec) * MathHelper.Clamp(vec.Length(), mag / 2, mag * 2));
            }
            else
            {
                line += (Vector2.Normalize(vec) * mag);
            }

            Game1.Pen.DrawLine((int)pos.X, (int)pos.Y, (int)line.X, (int)line.Y, (int)thick, col);
        }
        
    }
}
