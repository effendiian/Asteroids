/// ColorSet.cs - Version 3
/// Author: Ian Effendi
/// Date: 3.26.2017

#region Using statements.

// System using statements.
using System.Collections.Generic;
using System.Linq;

// Monogame using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Asteroid using statments.
using Asteroids.Entities;

#endregion

namespace Asteroids.Tools
{
    /// <summary>
    /// The 'State' of the game. Represents a 'situation' the game might find itself in and the entities/buttons involved with that context.
    /// </summary>
    public abstract class State
    {

        #region Fields. // Private data associated with a state.

        /// <summary>
        /// The state's type.
        /// </summary>
        private States stateType;

        /// <summary>
        /// Stores the scale of the entities inside the current state.
        /// </summary>
        private float scale;

        /// <summary>
        /// Stores the colors for the state's background and text.
        /// </summary>
        private ColorSet colorset;

        /// <summary>
        /// Stores a list of the entities in the state.
        /// </summary>
        private List<Entity> entities;

        /// <summary>
        /// Stores a list of the buttons in the state.
        /// </summary>
        private List<Button> buttons;

        /// <summary>
        /// ControlScheme for State-local input values.
        /// </summary>
        private ControlScheme scheme;

        #endregion

        #region Flags. // Flags that hold boolean status information.

        /// <summary>
        /// Determines if the state should print debug information.
        /// </summary>
        private bool _debug = false;

        #endregion

        #region Properties. // The properties that provide access to private data.

        /// <summary>
        /// The type of state this state is.
        /// </summary>
        public States StateType
        {
            get { return this.stateType; }
            private set { this.stateType = value; }
        }

        /// <summary>
        /// Returns a complete list of all the entities in the state.
        /// </summary>
        public List<Entity> AllEntities
        {
            get { return this.GetEntities(); }
        }
        
        /// <summary>
        /// Returns a complete list of all the buttons in the state.
        /// </summary>
        public List<Button> AllButtons
        {
            get { return this.GetButtons(); }
        }

        /// <summary>
        /// Determines if the state has any entities.
        /// </summary>
        public bool HasEntities
        {
            get { return AllEntities == null || AllEntities.Count() == 0; }
        }

        /// <summary>
        /// Determines if the state has any buttons.
        /// </summary>
        public bool HasButtons
        {
            get { return AllButtons == null || AllButtons.Count() == 0; }
        }

        /// <summary>
        /// Determines if the state has no buttons and no entities.
        /// </summary>
        public bool Empty
        {
            get { return !HasEntities && !HasButtons; }
        }

        /// <summary>
        /// Returns the color to draw the text for this state.
        /// </summary>
        public Color DrawColor
        {
            get { return this.colorset.GetColor(ColorType.Draw); }
        }

        /// <summary>
        /// Returns the color to draw the state. 
        /// </summary>
        public Color BackgroundColor
        {
            get { return this.colorset.GetColor(ColorType.Other); }
        }

        /// <summary>
        /// Scale to set the entities when drawing.
        /// </summary>
        public float Scale
        {
            get { return this.scale; }
            set { SetScale(value); }
        }

        /// <summary>
        /// Returns the debug flag value.
        /// </summary>
        public bool Debug
        {
            get { return this._debug; }
            set { this._debug = value; }
        }

        #endregion

        #region Constructor. // Two constructors; one using a colorset and one using the separate color statements.

        /// <summary>
        /// Construct a state, giving it its scale, UI colors, and background colors.
        /// </summary>
        /// <param name="set">The colors to to use for this state.</param>
        /// <param name="_scale">The scale to draw the items in this state.</param>
        public State(States type, ColorSet set, float _scale = 1.0f)
        {
            this.stateType = type;
            this.colorset = new ColorSet();
            this.colorset.AssignColors(set, ColorType.Draw, ColorType.Other); // Only get these colors from the pre-existing colorset.
            this.entities = new List<Entity>();
            this.buttons = new List<Button>();
            this.scheme = new ControlScheme();

            SetScale(_scale);
        }

        /// <summary>
        /// Construct a state, giving it its scale, UI colors, and background colors.
        /// </summary>
        /// <param name="_draw">The color to draw the UI.</param>
        /// <param name="_bg">The color to draw the background.</param>
        /// <param name="_scale">The scale to draw the items in this state.</param>
        public State(States type, Color _draw, Color _bg, float _scale = 1.0f)
        {
            this.stateType = type;
            this.colorset = new ColorSet(_draw, null, null, null, _bg);
            this.entities = new List<Entity>();
            this.buttons = new List<Button>();
            this.scheme = new ControlScheme();

            SetScale(_scale);
        }

        #endregion

        #region Methods. // Flags checking for the existence of the entities.

        #region Exist methods. // Methods that perform existence checks.

        /// <summary>
        /// Checks if an entity with the provided tag exists.
        /// </summary>
        /// <param name="e">The tag to search for.</param>
        /// <returns>Returns true if the entity exists.</returns>
        public bool Exists(string e)
        {
            return Exists(GetEntity(e));
        }

        /// <summary>
        /// Checks if the provided entity is null or not.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Returns if the provided entity is contained within the all entities list.</returns>
        public bool Exists(Entity e)
        {
            if (e != null)
            {
                return AllEntities.Contains(e);
            }

            return false;
        }

        /// <summary>
        /// Checks if an entity associated with the EntityType exists.
        /// </summary>
        /// <param name="type">EntityType to look for.</param>
        /// <returns>Returns true if the entity exists.</returns>
        public bool Exists(EntityType type)
        {
            return Exists(GetEntity(type));
        }

        #endregion

        #region Entity methods. // Get and add entities using various types of keys.

        /// <summary>
        /// Gets a list of entities that match a particular tag.
        /// </summary>
        /// <param name="source">Entities to search for matches inside of.</param>
        /// <param name="key">The tag to look for.</param>
        /// <returns>Returns a list of entities inside the provided source that have a tag matching the key.</returns>
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

        /// <summary>
        /// Gets a list of entities that match a particular <see cref="EntityType"/> type.
        /// </summary>
        /// <param name="source">Entities to search for matches inside of.</param>
        /// <param name="key">The <see cref="EntityType"/> type to look for.</param>
        /// <returns>Returns a list of entities inside the provided source that have a <see cref="EntityType"/> matching the key.</returns>
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

        /// <summary>
        /// Returns a list of all entities.
        /// </summary>
        /// <returns>Returns a list of all entities.</returns>
        public List<Entity> GetEntities()
        {
            return this.entities;
        }

        /// <summary>
        /// Returns a list of entities that match the tag.
        /// </summary>
        /// <param name="e">Tag to search for.</param>
        /// <returns>Returns a list of entities that match the tag, pulling from the pool of all entities in the state.</returns>
        public List<Entity> GetEntities(string e)
        {
            // Get entities matching the tag.
            return GetEntitiesFromList(AllEntities, e);
        }

        /// <summary>
        /// Returns the first entity that matches the tag.
        /// </summary>
        /// <param name="e">Tag to search for.</param>
        /// <returns>Returns the first entity that matches the given tags.</returns>
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

        /// <summary>
        /// Returns a list of entities that match the <see cref="EntityType"/> type. 
        /// </summary>
        /// <param name="type"><see cref="EntityType"/> type to search for.</param>
        /// <returns>Returns a list of entities that match the <see cref="EntityType"/> type, pulling from the pool of all entities in the state.</returns>
        public List<Entity> GetEntities(EntityType type)
        {
            // Get entities matching the type.
            return GetEntitiesFromList(AllEntities, type);
        }
        
        /// <summary>
        /// Returns the first entity that matches the <see cref="EntityType"/> type.
        /// </summary>
        /// <param name="type"><see cref="EntityType"/> type to search for.</param>
        /// <returns>Returns the first entity that matches the given <see cref="EntityType"/> type.</returns>
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

        /// <summary>
        /// Add an entity to the list of all entities.
        /// </summary>
        /// <param name="e">Entity to add.</param>
        public void AddEntity(Entity e)
        {
            this.entities.Add(e);
        }

        #endregion

        #region Button methods. // Get buttons in a various ways.

        /// <summary>
        /// Returns a list of all the buttons in the state.
        /// </summary>
        /// <returns>Returns a list of buttons.</returns>
        public List<Button> GetButtons()
        {
            return this.buttons;
        }

        /// <summary>
        /// Returns the first button in the list of all buttons that have the input action key.
        /// </summary>
        /// <param name="action">The action/response the button being searched for would have.</param>
        /// <returns>Returns a button with the same <see cref="Action"/> type.</returns>
        public Button GetButton(Actions action)
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

        /// <summary>
        /// Add an button to the list of all buttons.
        /// </summary>
        /// <param name="b">Button to add.</param>
        public void AddButton(Button b)
        {
            this.buttons.Add(b);
        }

        #endregion

        #region Enable/Disable methods. // Methods that enable or disable the entities in the state.

        /// <summary>
        /// Enable all of the entities in the state.
        /// </summary>
        public void Enable()
        {
            foreach (Entity entity in entities)
            {
                entity.Enabled = true;
            }
        }

        /// <summary>
        /// Enable all of the buttons in the state.
        /// </summary>
        public void EnableGUI()
        {
            foreach (Button button in buttons)
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// Disable all of the entities in the state.
        /// </summary>
        public void Disable()
        {
            foreach (Entity entity in entities)
            {
                entity.Enabled = false;
            }
        }

        /// <summary>
        /// Disable all of the buttons in the state.
        /// </summary>
        public void DisableGUI()
        {
            foreach (Button button in buttons)
            {
                button.Enabled = false;
            }
        }

        #endregion

        #region Reset/Start/Stop methods. // Reset functionality.

        /// <summary>
        /// Reset the state. (Calls reset for all entities).
        /// </summary>
        public virtual void Reset()
        {
            Reset(this.entities);
        }

        /// <summary>
        /// Reset and enable entities within a specified list.
        /// </summary>
        /// <param name="list">List of entities to enable and reset.</param>
        public virtual void Reset(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.Enabled = true;
                entity.Reset();
            }
        }

        /// <summary>
        /// Method called at the start of the state change. Starts all entities, by default.
        /// </summary>
        public virtual void Start()
        {
            foreach (Entity entity in AllEntities)
            {
                entity.Start();
            }
        }

        /// <summary>
        /// Stops all entities, by default.
        /// </summary>
        public virtual void Stop()
        {
            foreach (Entity entity in AllEntities)
            {
                entity.Stop();
            }
        }

        #endregion

        #region Update methods. // Input handling methods and update functionality.

        /// <summary>
        /// Create the control scheme to listen for the debug key press specified.
        /// </summary>
        /// <param name="key"></param>
        private void BindDebugKey(Keys key)
        {
            scheme.Bind(Commands.Debug, key, ActionType.Released);
        }

        /// <summary>
        /// Handle any input that needs to be checked on a State-level scope.
        /// </summary>
        protected virtual void HandleInput()
        {
            if (!scheme.IsEmpty())
            {
                if (scheme.IsFired(Commands.Debug))
                {
                    this._debug = !_debug;
                }
            }
        }

        /// <summary>
        /// Update the entities and check for input.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current elapsed time.</param>
        public virtual void Update(GameTime gameTime)
        {
            HandleInput();
            Update(gameTime, this.entities);
        }

        /// <summary>
        /// Update the entities for a specified list.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current elapsed time.</param>
        /// <param name="list">List of entities to update.</param>
        public void Update(GameTime gameTime, List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.Update(gameTime);
            }
        }

        /// <summary>
        /// Update the buttons.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current elapsed time.</param>
        public virtual void UpdateGUI(GameTime gameTime)
        {
            UpdateGUI(gameTime, this.buttons);
        }

        /// <summary>
        /// Update the entity GUI for a specified list.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current elapsed time.</param>
        /// <param name="list">List of entities to update the gui for.</param>
        private void UpdateGUI(GameTime gameTime, List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                entity.UpdateGUI(gameTime);
            }
        }

        /// <summary>
        /// Update the buttons for a specified list.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current elapsed time.</param>
        /// <param name="list">List of buttons to update.</param>
        private void UpdateGUI(GameTime gameTime, List<Button> list)
        {
            foreach (Button button in list)
            {
                button.Update(gameTime);
            }
        }

        #endregion

        #region Draw methods. // Draw the items in the state.
        
        /// <summary>
        /// Draw all entities in a given state.
        /// </summary>
        public virtual void Draw()
        {
            Draw(this.entities);
        }

        /// <summary>
        /// Draw the entities contained within a list.
        /// </summary>
        /// <param name="list">List of entities to draw.</param>
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

        /// <summary>
        /// Draw the GUI of any entities that require it and draw all buttons.
        /// </summary>
        public virtual void DrawGUI()
        {
            DrawGUI(this.buttons);

            foreach (Entity asteroid in GetEntities(EntityType.Asteroid))
            {
                asteroid.DrawGUI();
            }

            foreach (Entity test in GetEntities("!Test"))
            {
                test.DrawGUI();
            }
        }

        /// <summary>
        /// Draw the GUI for a list of entities.
        /// </summary>
        /// <param name="list">List of entities to draw information for.</param>
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

        /// <summary>
        /// Draw the GUI for a list of buttons.
        /// </summary>
        /// <param name="list">List of buttons to draw information for.</param>
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

        /// <summary>
        /// Draw a string in the color of the state's colorset.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="s">String to draw.</param>
        /// <param name="alignment">Alignment of the string to draw.</param>
        public void DrawMessage(int x, int y, string s, int alignment)
        {
            // Print message.
            GlobalManager.DrawMessage(x, y, s, alignment, colorset.DrawColor);
        }

        #endregion

        #region Service methods. // Contains service and wrapper functions.

        /// <summary>
        /// Set the scale of the entities.
        /// </summary>
        /// <param name="s">Scale to be set to.</param>
        public void SetScale(float s = 1.0f)
        {
            this.Scale = s;

            foreach (Entity e in GetEntities())
            {
                e.SetScale(s);
            }
        }

        /// <summary>
        /// Return the scale of the state.
        /// </summary>
        /// <returns>Returns the scale value as a float.</returns>
        public float GetScale()
        {
            return this.scale;
        }

        /// <summary>
        /// Returns the ShapeDrawer the pen.
        /// </summary>
        /// <returns>Returns reference to the GlobalManager ShapeDrawer object.</returns>
        public ShapeDrawer GetPen()
        {
            return GlobalManager.Pen;
        }

        /// <summary>
        /// Returns the vector for the screen center.
        /// </summary>
        /// <returns>Returns reference to the GlobalManager ScreenCenter vector.</returns>
        public Vector2 GetScreenCenter()
        {
            return GlobalManager.ScreenCenter;
        }

        /// <summary>
        /// Returns the vector for the screen boundaries.
        /// </summary>
        /// <returns>Returns reference to the GlobalManager ScreenBounds vector.</returns>
        public Vector2 GetScreenBounds()
        {
            return GlobalManager.ScreenBounds;
        }

        #endregion

        #endregion

    }
}
