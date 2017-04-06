using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Monogame using statements.
using Microsoft.Xna.Framework;

// Asteroid using statments.
using Asteroids.Entities;

namespace Asteroids.Tools
{
    public class State
    {
        // Fields.
        private ShapeDrawer pen;
        private Color drawColor;
        private Color backgroundColor;

        private List<Entity> entities;
        private List<Button> buttons;

        // Properties.
        public List<Button> Buttons
        {
            get { return this.buttons; }
        }

        public Color DrawColor {
            get { return this.drawColor; }
        }

        public Color BackgroundColor {
            get { return this.backgroundColor; }
        }

        // Scale of the entities when drawn.
        public float Scale
        {
            get; set;
        }
        
        // Constructor.
        public State(ShapeDrawer _pen, Color _draw, Color _bg)
        {
            this.pen = _pen;
            this.drawColor = _draw;
            this.backgroundColor = _bg;

            this.entities = new List<Entity>();
            this.buttons = new List<Button>();
        }

        // Methods.     
        public bool Exists(string e)
        {
            return Exists(GetEntity(e));
        }

        public bool Exists(Entity e)
        {
            if (e != null)
            {
                return true;
            }

            return false;
        }
        public bool Exists(EntityType type)
        {
            return Exists(GetEntity(type));
        }

        public List<Entity> GetEntitiesFromList(List<Entity> source, string key)
        {
            List<Entity> results = new List<Entity>();

            foreach (Entity entity in source)
            {
                if (entity.Tag == key)
                {
                    results.Add(entity);
                }
            }

            return results;
        }

        public List<Entity> GetEntitiesFromList(List<Entity> source, EntityType key)
        {
            List<Entity> results = new List<Entity>();

            foreach (Entity entity in source)
            {
                if (entity.Type == key)
                {
                    results.Add(entity);
                }
            }

            return results;
        }

        public List<Entity> GetEntities()
        {
            return this.entities;
        }
        
        public List<Entity> GetEntities(string e)
        {
            // Get entities matching the tag.
            List<Entity> result = new List<Entity>();

            foreach (Entity entity in entities)
            {
                if (entity.Tag == e)
                {
                    result.Add(entity);
                }
            }

            // Return result.
            return result;
        }

        public List<Entity> GetEntities(EntityType type)
        {
            // Get entities matching the type.
            List<Entity> result = new List<Entity>();

            foreach (Entity entity in entities)
            {
                if (entity.Type == type)
                {
                    result.Add(entity);
                }
            }

            // Return result.
            return result;
        }

        public Entity GetEntity(string e)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Tag == e)
                {
                    return entity;
                }
            }

            return null;
        }
        
        public Entity GetEntity(EntityType type)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Type == type)
                {
                    return entity;
                }
            }

            return null;
        }

        public List<Button> GetButtons()
        {
            return this.buttons;
        }

        public Button GetButton(StateManager.Actions action)
        {
            foreach (Button button in buttons)
            {
                if (button.Action == action)
                {
                    return button;
                }
            }

            return null;
        }

        public void Enable()
        {
            foreach (Entity entity in entities)
            {
                entity.Enabled = true;
            }
        }

        public void EnableGUI()
        {
            foreach (Button button in buttons)
            {
                button.Enabled = true;
            }
        }

        public void Disable()
        {
            foreach (Entity entity in entities)
            {
                entity.Enabled = false;
            }
        }

        public void DisableGUI()
        {
            foreach (Button button in buttons)
            {
                button.Enabled = false;
            }
        }

        public void Reset()
        {
            Reset(this.entities);
        }

        public void Reset(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.Enabled = true;
                entity.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime, this.entities);
        }

        public void Update(GameTime gameTime, List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.Update(gameTime);
            }
        }

        public void UpdateGUI(GameTime gameTime)
        {
            UpdateGUI(gameTime, this.buttons);
        }
        public void UpdateGUI(GameTime gameTime, List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.UpdateGUI(gameTime);
            }
        }
        public void UpdateGUI(GameTime gameTime, List<Button> list)
        {
            foreach (Button button in list)
            {
                button.Update(gameTime);
            }
        }

        public void Draw()
        {
            Draw(this.entities);
        }
        public void Draw(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity.Enabled)
                {
                    entity.Draw();
                }
            }
        }

        public void DrawGUI()
        {
            DrawGUI(this.buttons);

            foreach (Entity asteroid in GetEntities(EntityType.Asteroid))
            {
                asteroid.DrawGUI();
            }

            foreach(Entity test in GetEntities("!Test"))
            {
                test.DrawGUI();
            }
        }

        public void DrawGUI(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity.Enabled)
                {
                    entity.DrawGUI();
                }
            }
        }

        public void DrawGUI(List<Button> list)
        {
            foreach (Button button in list)
            {
                if (button.Enabled)
                {
                    button.Draw();
                }
            }
        }

        public void DrawMessage(int x, int y, string s, int alignment)
        {
            // Print message.
            pen.DrawString(x, y, drawColor, s, alignment);            
        }

        public void AddEntity(Entity e)
        {
            this.entities.Add(e);
        }

        public void AddButton(Button b)
        {
            this.buttons.Add(b);
        }

        public void SetScale(float s)
        {
            this.Scale = s;

            foreach (Entity e in GetEntities())
            {
                e.SetScale(s);
            }
        }

        public float GetScale()
        {
            return this.Scale;
        }

        public ShapeDrawer GetPen()
        {
            return this.pen;
        }

        public Vector2 GetScreenCenter()
        {
            return Game1.ScreenCenter;
        }

        public Vector2 GetScreenBounds()
        {
            return Game1.ScreenBounds;
        }
    }
}
