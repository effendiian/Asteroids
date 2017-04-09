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
using Microsoft.Xna.Framework.Graphics;

// Asteroid using statments.
using Asteroids.Entities;
using Asteroids.Attributes;

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

        /// <summary>
        /// Maximum amount of entities that can be inside a state.
        /// </summary>
        private int entityCap;

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
        /// Returns the number of entities inside the state.
        /// </summary>
        public int Count
        {
            get { return this.entities.Count(); }
        }

        /// <summary>
        /// Returns the maximum amount of entities allowed to exist within a state.
        /// </summary>
        public int Capacity
        {
            get { return this.entityCap; }
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
        /// Returns the <see cref="ControlScheme"/> object.
        /// </summary>
        protected ControlScheme Controls
        {
            get { return this.scheme; }
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
            this._debug = false;

            SetScale(_scale);
            BindKeys();
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
            this._debug = false;

            SetScale(_scale);
            BindKeys();
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
            if (this.Count < this.Capacity)
            {
                this.entities.Add(e);
            }
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

        /// <summary>
        /// Searches state for a matching button and then checks for its released state.
        /// </summary>
        /// <param name="action">Action to perform on button press.</param>
        /// <returns>Returns true if button exists and is fired.</returns>
        protected bool IsActionFired(Actions action)
        {
            Button button = GetButton(action); // May return null.
            return IsButtonFired(button, action);
        }

        /// <summary>
        /// Determines if a given button matching the action has been fired.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <param name="action">Action to perform on button press.</param>
        /// <returns>Returns true if button is not null, has a matching action, and has been released.</returns>
        protected bool IsButtonFired(Button button, Actions action)
        {
            if (button == null || button.Action != action)
            {
                return false;
            }
            else
            {
                return button.IsReleased();
            }
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
        /// Bind keys to the control scheme for the given state.
        /// </summary>
        protected abstract void BindKeys();

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
        /// Handles all button press inputs for a state.
        /// </summary>
        protected virtual void HandleGUIInput()
        {
            if (HasButtons)
            {
                // Button input goes here.
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

            UpdateGUI(gameTime);
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
            HandleGUIInput();
            QueueGUIMessages();
            UpdateGUI(gameTime, this.buttons);
            QueueDebugMessages();
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

        /// <summary>
        /// Text that gets drawn to the screen for the user to read.
        /// </summary>
        protected virtual void QueueGUIMessages()
        {
            // Stub. Child classes should override this method, if they need to use it.
        }

        /// <summary>
        /// Text that gets drawn to the screen for debugging purposes.
        /// </summary>
        protected virtual void QueueDebugMessages()
        {
            if (Debug)
            {
                string msg = this.ToString();
                Padding padding = new Padding(0, GetStringHeight(msg));
                Vector2 position = new Vector2(10, padding.Y);
                AddMessage(this.ToString(), position, padding, this.DrawColor, 1, ShapeDrawer.LEFT_ALIGN);
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
        /// Draw a series of entities.
        /// </summary>
        protected virtual void DrawEntityGUI()
        {
            DrawGUI(GetEntities(EntityType.Asteroid),
                GetEntities("test")); // Make DrawGUI calls for all entities that match the calls.
        }

        /// <summary>
        /// Draw the buttons.
        /// </summary>
        protected virtual void DrawButtons()
        {
            DrawGUI(this.buttons);
        }

        /// <summary>
        /// Draw the GUI of any entities that require it and draw all buttons.
        /// </summary>
        public virtual void DrawGUI()
        {
            DrawEntityGUI(); // Draw all entity GUI information.
            DrawButtons(); // Draw the buttons above the entity GUI.
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
        /// Draw the GUI for a concatenated amount of lists.
        /// </summary>
        /// <param name="lists">An array of lists.</param>
        public void DrawGUI(params List<Entity>[] lists)
        {
            foreach (List<Entity> list in lists)
            {
                DrawGUI(list);
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

        /// <summary>
        /// Return the string dimensions for a particular message.
        /// </summary>
        /// <param name="message">Message to check size of.</param>
        /// <param name="font">Optional font to choose a SpriteFont other than the main one.</param>
        /// <returns>Returns a vector containing the dimensions.</returns>
        public Vector2 GetStringDimensions(string message, SpriteFont font = null)
        {
            return GlobalManager.Pen.StringDimensions(message, font);
        }

        /// <summary>
        /// Return the string width for a particular message.
        /// </summary>
        /// <param name="message">Message to check size of.</param>
        /// <param name="font">Optional font to choose a SpriteFont other than the main one.</param>
        /// <returns>Returns the width as a float.</returns>
        public float GetStringWidth(string message, SpriteFont font = null)
        {
            return GetStringDimensions(message, font).X;
        }

        /// <summary>
        /// Return the string height for a particular message.
        /// </summary>
        /// <param name="message">Message to check size of.</param>
        /// <param name="font">Optional font to choose a SpriteFont other than the main one.</param>
        /// <returns>Returns the height as a float.</returns>
        public float GetStringHeight(string message, SpriteFont font = null)
        {
            return GetStringDimensions(message, font).Y;
        }

        /// <summary>
        /// Wrapper function that will add a message to the global state manager queue.
        /// </summary>
        /// <param name="msg">Message to print.</param>
        /// <param name="position">Position of the message.</param>
        /// <param name="padding">Padding to apply.</param>
        /// <param name="col">Color to draw the message in.</param>
        /// <param name="order">The order value.</param>
        /// <param name="alignment">Alignment type to print message with.</param>
        public void AddMessage(string msg, Vector2 position, Padding padding, Color col, int order = 1, int alignment = ShapeDrawer.LEFT_ALIGN)
        {
            StateManager.AddMessage(new Message(msg, position, padding, col, order, alignment));
        }
        
        /// <summary>
        /// Return the state's values 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string value = "";

            value += "Current State: " + StateManager.GetStateTypeAsString(this.stateType) + "\n";
            value += "Entities: [" + this.Count + "\\" + this.Capacity + "] | ";
            value += "Buttons: [" + this.buttons.Count() + "] | ";
            value += "Scale: [" + (this.Scale * 100) + "%]\n";

            return value;
        }

        #endregion

        #endregion

    }
}
