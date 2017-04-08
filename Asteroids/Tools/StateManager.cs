using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Monogame using statements.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Asteroid using statments.
using Asteroids.Entities;
using Asteroids.Attributes;

namespace Asteroids.Tools
{
    // Actions.
    public enum Actions
    {
        Start,
        Quit,
        Back,
        Pause,
        Resume,
        Scores,
        Options,
        Dimensions
    }


    public enum States
    {
        Pause,
        Options,
        Main,
        Arena,
        Gameover,
        Quit
    }

    // Objects.
    public enum TextureIDs
    {
        Button,
        Player,
        Bullet,
        Asteroid1,
        Asteroid2,
        Asteroid3,
        Test
    }

    public class StateManager
    {
        // States.
        private static List<State> stateList;
        private static Dictionary<States, State> states;
        private static Dictionary<States, List<Entity>> stateEntities;
        private static Dictionary<States, List<Button>> stateButtons;
        private static List<Message> messages;

        
        private static States currState;
        private static States prevState;

        //

        // Methods.        
        // Initialize the state manager.
        public static void Initialize()
        {
            // Create the basic states.
            stateList = new List<State>();
            states = new Dictionary<States, State>();
            stateButtons = new Dictionary<States, List<Button>>();
            stateEntities = new Dictionary<States, List<Entity>>();
            messages = new List<Message>();

            // Set the initial state to the main state.
            currState = States.Main;
            prevState = currState;
        }

        public static void HandleInput(State cState)
        {
            // Handle input.
            switch (currState)
            {
                case States.Main:
                    // Main update code.
                    // Quit the game if these buttons are pressed in the main menu.
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.IsKeyReleased(Keys.Escape))
                    {
                        Quit();
                    }
                    break;
                case States.Options:
                    // Return to the main menu, if these buttons are pressed during the options screen.
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.IsKeyReleased(Keys.Escape))
                    {
                        ChangeState(States.Main);
                    }
                    break;
                case States.Arena:
                case States.Pause:
                    // During the main game.
                    if (currState == States.Pause)
                    {
                        // What to do during the pause functionality.
                        if (InputManager.IsKeyReleased(Keys.P))
                        {
                            TogglePause();
                        }

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.IsKeyReleased(Keys.Escape))
                        {
                            ChangeState(States.Main);
                        }

                        if (InputManager.IsKeyReleased(Keys.Q))
                        {
                            Quit();
                        }
                    }
                    else
                    {
                        // What to do when the game is not paused.
                        // If the pause button is pushed while playing the game.
                        if (InputManager.IsKeyReleased(Keys.P) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.IsKeyReleased(Keys.Escape))
                        {
                            TogglePause();
                        }
                    }
                    break;
                case States.Gameover:
                    // What to do during the victory/loss phase of the game.
                    // Return to the main menu, if these buttons are pressed during the victory screen.
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.IsKeyReleased(Keys.Escape))
                    {
                        ChangeState(States.Main);
                    }
                    break;
            }
        }

        public static void HandleGUIInput(State cState)
        {
            // Handle GUI input.
            switch (currState)
            {
                case States.Main:
                    // Deal with the state switcher.
                    if (cState.GetButton(Actions.Quit).IsReleased())
                    {
                        Quit();
                    }

                    if (cState.GetButton(Actions.Start).IsReleased())
                    {
                        StartGame();
                    }

                    if (cState.GetButton(Actions.Options).IsReleased())
                    {
                        ChangeState(States.Options);
                    }

                    if (cState.GetButton(Actions.Scores).IsReleased())
                    {
                        EndGame();
                    }
                    break;
                case States.Options:
                    if (cState.GetButton(Actions.Dimensions).IsReleased())
                    {
                        ChangeDimensions();
                    }
                    if (cState.GetButton(Actions.Back).IsReleased())
                    {
                        ChangeState(States.Main);
                    }
                    break;
                case States.Arena:
                case States.Pause:
                    // During the main game.
                    if (currState == States.Pause)
                    {
                        // What to do during the pause functionality.                   
                        // Handle the input.
                        if (cState.GetButton(Actions.Resume).IsReleased())
                        {
                            TogglePause();
                        }

                        if (cState.GetButton(Actions.Quit).IsReleased())
                        {
                            Quit();
                        }
                    }
                    else
                    {
                        // What to do when the game is not paused.
                        // Handle the input.
                        if (cState.GetButton(Actions.Pause).IsReleased())
                        {
                            TogglePause();
                        }
                    }
                    break;
                case States.Gameover:
                    // What to do during the victory/loss phase of the game.
                    if (cState.GetButton(Actions.Back).IsReleased())
                    {
                        ChangeState(States.Main);
                    }

                    break;
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (prevState != currState)
            {
                // Disable the previous state.
                states[prevState].Disable();
            }

            State cState = states[currState];
            cState.Enable();
            cState.Update(gameTime);
            HandleInput(cState);

            switch (currState)
            {
                case States.Main:
                    break;
                case States.Options:
                    break;
                case States.Arena:
                case States.Pause:
                    if (currState == States.Pause)
                    {

                    }
                    else
                    {
                        State current = states[currState];

                        if (current.Exists(EntityType.Test))
                        {
                            List<Entity> allTestObjects = current.GetEntities(EntityType.Test);

                            List<Entity> tests = new List<Entity>();
                            TestMover player = null;

                            foreach (Entity e in allTestObjects)
                            {
                                if (e is TestMover)
                                {
                                    if (((TestMover)e).PlayerControl)
                                    {
                                        player = (TestMover)e;
                                    }
                                    else
                                    {
                                        tests.Add(e);
                                    }
                                }
                            }

                            if (player != null)
                            {
                                foreach (Entity e in tests) {

                                    if (e is TestMover)
                                    {
                                        TestMover test = (TestMover)e;

                                        if (test.Collision(player))
                                        {
                                            test.DrawColor = Color.Red;
                                            test.Randomize();
                                        }
                                        else if (test.Proximity(player))
                                        {
                                            test.DrawColor = Color.LimeGreen;
                                        }
                                        else
                                        {
                                            test.DrawColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }

                        // GlobalParticleGenerator.Update(gameTime);
                    }
                    break;
                case States.Gameover:
                    break;
                case States.Quit:
                    Quit();
                    break;
            }
        }

        public static void UpdateGUI(GameTime gameTime)
        {
            if (prevState != currState)
            {
                // Disable the previous state's buttons.
                states[prevState].DisableGUI();
            }

            // Enable the buttons.
            State cState = states[currState];
            cState.EnableGUI();
            cState.UpdateGUI(gameTime);
            HandleGUIInput(cState);

            messages = new List<Message>();

            // Get measurement values.
            Vector2 bounds = Game1.ScreenBounds;
            Vector2 center = Game1.ScreenCenter;
            Padding padding;
            Vector2 position;
            Color color = cState.DrawColor;

            switch (currState)
            {
                case States.Main:
                    break;
                case States.Options:
                    position = center;
                    padding = new Padding(0, Game1.Pen.StringHeight("A"));
                    AddMessage(new Message("Press the ESC key to return to the main menu.", position, padding, color, 1, ShapeDrawer.CENTER_ALIGN));
                    AddMessage(new Message("Current screen dimensions: " + Game1.ScreenBounds, position, padding, color, 2, ShapeDrawer.CENTER_ALIGN));

                    string isFullScreen = "Fullscreen: ";

                    if (Game1.GraphicsDM.IsFullScreen)
                    {
                        isFullScreen += "On";
                    }
                    else
                    {
                        isFullScreen += "Off";
                    }

                    AddMessage(new Message(isFullScreen, position, padding, color, 3, ShapeDrawer.CENTER_ALIGN));
                    break;
                case States.Arena:
                case States.Pause:

                    if (currState == States.Pause)
                    {
                        // What to do during the pause functionality.  
                        // Print message: Escape to quit. P to resume the game.
                        position = center;
                        padding = new Padding(0, Game1.Pen.StringHeight("A"));
                        AddMessage(new Message("Press the ESC key to return to the main menu.", position, padding, color, 1, ShapeDrawer.CENTER_ALIGN));
                        AddMessage(new Message("Press the P key to resume the game.", position, padding, color, 2, ShapeDrawer.CENTER_ALIGN));
                        AddMessage(new Message("Press the Q key to quit to the desktop.", position, padding, color, 3, ShapeDrawer.CENTER_ALIGN));
                        AddMessage(new Message("Move with the WASD keys. Turn with the Left and Right arrow keys.", position, padding, color, 4, ShapeDrawer.CENTER_ALIGN));
                    }
                    else
                    {
                        // What to do when the game is not paused.


                    }

                    break;
                case States.Gameover:
                    // What to do during the victory/loss phase of the game.
                    position = center;
                    padding = new Padding(0, (Game1.Pen.StringHeight("A") * 2 + 55));
                    AddMessage(new Message("Scores are indicated here.", position, padding, color, 0, ShapeDrawer.CENTER_ALIGN));
                    break;
            }

        }

        public static void Draw()
        {
            State current = states[currState];
            current.Draw(); 

            switch (currState)
            {
                case States.Main:
                    break;
                case States.Options:
                    break;
                case States.Arena:
                case States.Pause:
                    if (currState == States.Pause)
                    {

                    }
                    else
                    {
                        // GlobalParticleGenerator.Draw();
                    }
                    break;
                case States.Gameover:
                    break;
            }
        }

        public static void DrawGUI(ShapeDrawer pen)
        {
            State cState = states[currState];
            cState.DrawGUI(); // Main draw code.
            
            Vector2 center = Game1.ScreenCenter;
            Padding padding = new Padding(0, pen.StringHeight("A"));

            string title = "Asteroids - The Game!";
            Vector2 offset = new Vector2(15, 5);
            int strHeight = pen.StringHeight("A");
            int strWidth = pen.StringWidth(title);            
            
            // Draw the title of the game at the top of the screen.
            pen.DrawRectFilled((int)center.X - (int)((strWidth + offset.X) / 2), strHeight, strWidth + (int)offset.X, strHeight + (int)offset.Y, cState.BackgroundColor); // Outlines "Asteroids, the Game!".

            // Draw the title of the game at the top of the screen.
            pen.DrawRectOutline((int)center.X - (int)((strWidth + offset.X) / 2), strHeight, strWidth + (int)offset.X, strHeight + (int)offset.Y, cState.DrawColor); // Outlines "Asteroids, the Game!".

            switch (currState)
            {
                case States.Main:

                    break;
                case States.Options:
                    break;
                case States.Arena:
                case States.Pause:
                    break;
                case States.Gameover:
                    break;
            }
            
            // Draw the mouse cursor.
            InputManager.Draw(pen);

            // Draw the final GUI pieces here.
            if (messages.Count() != 0) {
                for (int i = 0; i < messages.Count(); i++)
                {
                    if (messages[i].Order == 0)
                    {
                        messages[i].Order = (i + 1);
                    }

                    Message.DrawMessage(messages[i]);
                }
            }

            // Message.DrawMessages();

            cState.DrawMessage((int)center.X, strHeight, title, ShapeDrawer.CENTER_ALIGN); // Draw the title; don't queue it.

            // Debug messages.
            // InputManager.DrawGUI(shapeDrawer);
        }

        public static Color DrawColor
        {
            get
            {
                return states[currState].DrawColor;
            }
        }

        public static Color Background
        {
            get
            {
                return states[currState].BackgroundColor;
            }
        }

        public static void CreateStates(SpriteBatch sb, ShapeDrawer pen, Dictionary<TextureIDs, Texture2D> textures)
        {
            // Dimensions.
            Padding screenPadding = new Padding(10);
            Padding centerPadding = new Padding(60);

            Vector2 bounds = new Vector2(135, 45);
            Vector2 screen = Game1.ScreenBounds;
            Vector2 center = Game1.ScreenCenter;

            // Buttons.
            Texture2D button = textures[TextureIDs.Button];
            Button start = new Button(Actions.Start, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(-1)), bounds, button, "Start");
            Button options = new Button(Actions.Options, pen, Button.Positions.Center, null, bounds, button, "Options");
            Button scores = new Button(Actions.Scores, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(1)), bounds, button, "Scores");
            Button exit = new Button(Actions.Quit, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(2)), bounds, button, "Exit");

            Button dimensions = new Button(Actions.Dimensions, pen, Button.Positions.Center, null, null, button, "Change Dimensions");
            Button pause = new Button(Actions.Pause, pen, Button.Positions.BottomRight, screenPadding.Get(-1), bounds, button, "Pause");
            Button resume = new Button(Actions.Resume, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(-2)), bounds, button, "Resume");
            Button quit = new Button(Actions.Quit, pen, Button.Positions.Center, new Vector2(0, centerPadding.GetY(-1)), null, button, "Quit to Windows");

            Button back = new Button(Actions.Back, pen, Button.Positions.BottomRight, screenPadding.Get(-1), bounds, button, "Back"); // Reused.
           
            // Asteroid texture set-up.
            List<Texture2D> asteroidTextures = new List<Texture2D>();
            asteroidTextures.Add(textures[TextureIDs.Asteroid1]);
            asteroidTextures.Add(textures[TextureIDs.Asteroid2]);
            asteroidTextures.Add(textures[TextureIDs.Asteroid3]);

            // Entities
            Asteroid asteroid = new Asteroid(pen, asteroidTextures[0], "Asteroid");
            TestMover test = new TestMover(pen, textures[TextureIDs.Test], Color.LimeGreen, _size: new Vector2(15, 15));
            TestMover test2 = new TestMover(pen, textures[TextureIDs.Test], Color.LimeGreen, _size: new Vector2(15, 15));
            test2.Tag = "!Test2";

            // Initialize the global particle generator for testing.
            // GlobalParticleGenerator.Initialize(textures[TextureIDs.Bullet], null, 10);

            // Entities under player control.
            test.PlayerControl = true;
            test2.PlayerControl = false;
            
            // Main State.
            States state = States.Main;
            AddState(state, new State(pen, Color.Black, Color.AntiqueWhite));
            AddButtonToState(state, start); // Start button.          
            AddButtonToState(state, exit); // Exit button.      
            AddButtonToState(state, options); // Options button.
            AddButtonToState(state, scores); // Score button.

            // Options State.
            state = States.Options;
            AddState(state, new State(pen, Color.Black, Color.Beige));
            AddButtonToState(state, back); // Back button.  
            AddButtonToState(state, dimensions); // Dimensions 

            // Arena State.
            state = States.Arena;
            AddState(state, new State(pen, Color.LimeGreen, Color.Black));
            AddButtonToState(state, pause);
            AddEntityToState(state, test);
            AddEntityToState(state, test2);
            AddEntityToState(state, asteroid);

            // Pause State.
            state = States.Pause;
            AddState(state, new State(pen, Color.Black, Color.Beige));
            AddButtonToState(state, resume);
            AddButtonToState(state, quit);
            
            // Gameover State.
            state = States.Gameover;
            AddState(state, new State(pen, Color.Black, Color.LavenderBlush));
            AddButtonToState(state, back); // Back button.  

            // Quit State.
            state = States.Quit;
            AddState(state, new State(pen, Color.Black, Color.BlueViolet));            
        }

        private static void AddState(States key, State state)
        {
            if (states.ContainsKey(key))
            {
                states[key] = state;
            }
            else
            {
                states.Add(key, state);
            }

            stateList.Add(state);
            state.SetScale(1.0f);
        }

        private static void AddState(States key, ShapeDrawer pen, Color draw, Color bg)
        {
            AddState(key, new State(pen, draw, bg));
        }

        public static void AddEntityToState(States key, Entity entity)
        {
            states[key].AddEntity(entity);

            if (!stateEntities.ContainsKey(key))
            {
                stateEntities[key] = new List<Entity>();
            }

            stateEntities[key].Add(entity);
        }

        public static void AddButtonToState(States key, Button button)
        {
            states[key].AddButton(button);

            if (!stateButtons.ContainsKey(key))
            {
                stateButtons[key] = new List<Button>();
            }

            stateButtons[key].Add(button);
        }

        public static void ChangeState(States targetState)
        {
            // When changing states, set the previous state to the current state, 
            // Before setting target.

            prevState = currState;
            currState = targetState;

            states[prevState].Disable();
            states[currState].Enable();
        }

        public static void RevertState()
        {
            // Revert the states. The current state will be lost.

            states[currState].Disable();

            currState = prevState;
            states[currState].Enable();
        }

        public static void TogglePause()
        {
            if (currState != States.Pause)
            {
                ChangeState(States.Pause);              
            }
            else
            {
                RevertState();
            }
        }
        
        public static void StartGame()
        {
            ChangeState(States.Arena);
            // Perform reset functions here.
            states[currState].Reset();
            // GlobalParticleGenerator.Stop();
        }

        public static void EndGame()
        {
            ChangeState(States.Gameover);
            prevState = States.Main;
        }

        public static void Quit()
        {
            ChangeState(States.Quit);
            prevState = States.Main;
        }
        
        public static States GetState()
        {
            return currState;
        }

        private static void ChangeDimensions()
        {
            int height = 600;
            int width = 800;

            switch (Game1.GraphicsDM.PreferredBackBufferWidth)
            {
                case 1920:
                    height = 600;
                    width = 800;
                    break;
                case 1200:
                    height = 1020;
                    width = 1920;
                    break;
                case 800:
                default:
                    height = 720;
                    width = 1200;
                    break;
            }

            float scale = 1.0f * (width / 800);

            // GlobalParticleGenerator.SetScale(scale);

            foreach (State state in stateList)
            {
                state.SetScale(scale);
            }

            Game1.UpdateScreenDimensions(width, height);
        }
        
        public static void AddMessage(string msg, Vector2 pos, Padding pad, Color draw, int order, int alignment)
        {
            messages.Add(new Message(msg, pos, pad, draw, order, alignment));
        }

        public static void AddMessage(Message msg)
        {
            messages.Add(msg);
        }        
    }
}
