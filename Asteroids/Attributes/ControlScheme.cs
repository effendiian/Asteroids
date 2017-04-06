/// ControlScheme.cs - Version 2
/// Author: Ian Effendi
/// Last Updated: 4.4.2017
/// File Description: Contains reference to the class, as well as a reference to the Asteroids.Attributes.Controls enum.

#region Using statements.

// System using statements. 
using System.Collections.Generic; // For List.
using System.Linq; // For List.

// MonoGame/XNA using statements.
using Microsoft.Xna.Framework.Input; // For dealing with the Keys enum.

#endregion

// Attributes aren't tools
namespace Asteroids.Tools
{
    #region Enums. // Contains Commands and ActionType enums, used for finding keypress input.

    #region Commands enum. // List of commands players can call.

    /// <summary>
    /// Controls contains a list of all possible commands a player may call.
    /// </summary>
    public enum Commands
    {
        /// <summary>
        /// Push the entity forward.
        /// </summary>
        Forward,

        /// <summary>
        /// Stop the entity.
        /// </summary>
        Brake,

        /// <summary>
        /// Rotate the entity's image counter-clockwise, spinning it to the left.
        /// </summary>
        RotateLeft,

        /// <summary>
        /// Rotate the entity's image clockwise, spinning it to the right.
        /// </summary>
        RotateRight,

        /// <summary>
        /// Turn the entity's heading counter-clockwise, moving it toward the left.
        /// </summary>
        TurnLeft,

        /// <summary>
        /// Turn the entity's heading clockwise, moving it toward the right.
        /// </summary>
        TurnRight,

        /// <summary>
        /// Toggle an entity's debug mode, on or off.
        /// </summary>
        Debug,

        /// <summary>
        /// Toggle an entity's friction detection, on or off.
        /// </summary>
        FrictionMode,

        /// <summary>
        /// Randomize the entity's configuration in real-time.
        /// </summary>
        Randomize,

        /// <summary>
        /// Increment a property value of the entity.
        /// </summary>
        Increment,

        /// <summary>
        /// Decrement a property value of the entity.
        /// </summary>
        Decrement,

        /// <summary>
        /// Deal damage to the entity.
        /// </summary>
        Hurt,

        /// <summary>
        /// Heal the entity.
        /// </summary>
        Heal,

        /// <summary>
        /// Kill the entity, causing any action during death to occur.
        /// </summary>
        Kill,

        /// <summary>
        /// Spawn a new entity, somewhere on the screen.
        /// </summary>
        Spawn,

        /// <summary>
        /// Pause the game.
        /// </summary>
        Pause,

        /// <summary>
        /// Quit to windows/Exit the game.
        /// </summary>
        Quit,

        /// <summary>
        /// Return to a previous state.
        /// </summary>
        Back
    }

    #endregion

    #region ActionType enum. // Keypress response enum.

    /// <summary>
    /// Type of keypress response to look for, when associated with a given command.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Fire command if the key is up.
        /// </summary>
        Up,

        /// <summary>
        /// Fire command if the key is down.
        /// </summary>
        Down,

        /// <summary>
        /// Fire command if the key has been held down for more than 1 frame.
        /// </summary>
        Held,

        /// <summary>
        /// Fire command if the key has just been released.
        /// </summary>
        Released,

        /// <summary>
        /// Fire command if the key has just been pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// Null action. Commands with this action type will never fire.
        /// </summary>
        Null
    }

    #endregion

    #endregion

    /// <summary>
    /// ControlScheme takes a list of Commands and associates a Keys object and ActionType keypress response to listen for.
    /// The entity handles input and setup for the control scheme.
    /// </summary>
    public class ControlScheme
    {

        #region Constants. // Default values.

        /// <summary>
        /// A key press should fire when released, by default.
        /// </summary>
        private const ActionType DEFAULT_TYPE = ActionType.Released;

        #endregion

        #region Fields.

        /// <summary>
        /// Store of all the given commands in a particular control scheme.
        /// </summary>
        private List<Commands> commandLog;

        /// <summary>
        /// Returns the action type associated with a given command, for determining when a key is fired.
        /// </summary>
        private Dictionary<Commands, ActionType> actionTypes;

        /// <summary>
        /// Store of keys associated with a given command.
        /// </summary>
        private Dictionary<Commands, List<Keys>> commands;
        
        #endregion

        #region Properties. // Publicly accessible data.

        /// <summary>
        /// Returns a list of all commands associated with this scheme.
        /// </summary>
        public List<Commands> AllCommands
        {
            get { return this.commandLog; }
        }

        #endregion

        #region Constructor. // Sets up and initializes a control scheme.

        /// <summary>
        /// A control scheme is a list of commands,
        /// keys associated with those actions, and,
        /// the type of keypress to look for, before a key
        /// is considered as fired.
        /// </summary>
        public ControlScheme()
        {
            commandLog = new List<Commands>();
            commands = new Dictionary<Commands, List<Keys>>();
            actionTypes = new Dictionary<Commands, ActionType>();
        }

        #endregion

        #region Methods. // Service methods that aid in assigning keys to a given Control Scheme.

        /// <summary>
        /// Returns an array of flags for each command entered.
        /// </summary>
        /// <param name="commandArray">The list of commands being checked.</param>
        /// <returns>Returns a boolean array, parallel to the input list of commands, returning flags for each.</returns>
        public bool[] IsFired(List<Commands> commandList)
        {
            bool[] commandFlags = new bool[commandList.Count()];

            for (int i = 0; i < commandList.Count(); i++)
            {
                commandFlags[i] = IsFired(commandList[i]);
            }

            return commandFlags;
        }

        /// <summary>
        /// Returns an array of flags for each command entered.
        /// </summary>
        /// <param name="commandArray">The array of commands being checked.</param>
        /// <returns>Returns a boolean array, parallel to the input array of commands, returning flags for each.</returns>
        public bool[] IsFired(Commands[] commandArray)
        {
            bool[] commandFlags = new bool[commandArray.Length];

            for (int i = 0; i < commandArray.Length; i++)
            {
                commandFlags[i] = IsFired(commandArray[i]);
            }

            return commandFlags;
        }

        /// <summary>
        /// Determines if a command has been called,
        /// by checking its associated key with its associated actiontype.
        /// and using that to check its status from the InputManager.
        /// </summary>
        /// <param name="command">The command being checked.</param>
        /// <returns>Returns true if the key's action type matches the associated InputManager key status.</returns>
        public bool IsFired(Commands command)
        {
            if (CommandExists(command))
            {
                return InputManager.IsFired(GetCommandKeys(command), GetCommandActionType(command));
            }

            return false;
        }

        /// <summary>
        /// Checks if the input command is stored within this scheme.
        /// </summary>
        /// <param name="command">The command being checked.</param>
        /// <returns>Returns true if there is a key associated with this command.</returns>
        public bool CommandExists(Commands command)
        {
            // We use commands instead of commandLog,
            // because, this allows us to accurately
            // use commands.Add() and commands[key] in the future.
            return commands.ContainsKey(command);
        }

        /// <summary>
        /// Checks if a key is associated with a given command.
        /// </summary>
        /// <param name="command">The command being checked.</param>
        /// <param name="key">The key associated with the command.</param>
        /// <returns>Returns true if the key is associated with the given command.</returns>
        public bool KeyExists(Commands command, Keys key)
        {
            // If a command exists and if the key is associated with that command.
            return CommandExists(command) && commands[command].Contains(key);
        }

        /// <summary>
        /// Checks if a key is already used within this scheme.
        /// </summary>
        /// <param name="key">The key being checked.</param>
        /// <returns>Returns true if an association with the given key exists.</returns>
        public bool KeyExists(Keys key)
        {
            // We use command log to iterate.
            foreach (Commands command in commandLog)
            {
                // Call the other KeyExists() method
                // and check if an association exists for each command.
                if (KeyExists(command, key))
                {
                    // The key exists, somewhere. Return true.
                    return true;
                }
            }

            // No association exists.
            return false;
        }

        /// <summary>
        /// Returns all of the keys associated with this control scheme.
        /// </summary>
        /// <returns>Returns a Keys enum.</returns>
        public List<Keys> GetKeys()
        {
            List<Keys> keys = new List<Keys>();

            foreach (Commands command in commandLog)
            {
                foreach(Keys key in GetCommandKeys(command))
                {
                    keys.Add(key);
                }
            }

            return keys;
        }
        
        /// <summary>
        /// Bind a set of keys to a command with an action type to look for.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="keys">The list of keys being added.</param>
        /// <param name="action">The keypress type to look for once added.</param>
        public void Bind(Commands command, List<Keys> keys, ActionType action)
        {
            AssignAction(command, action);
            AssignKeys(command, keys);
        }

        /// <summary>
        /// Bind a set of keys to a command with an action type to look for.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="keys">The array of keys being added.</param>
        /// <param name="action">The keypress type to look for once added.</param>
        public void Bind(Commands command, Keys[] keys, ActionType action)
        {
            AssignAction(command, action);
            AssignKeys(command, keys);
        }

        /// <summary>
        /// Bind a key to a command with an action type to lookm for.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="key">The key being added.</param>
        /// <param name="action">The keypress type to look for once added.</param>
        public void Bind(Commands command, Keys key, ActionType action)
        {
            AssignAction(command, action);
            AssignKey(command, key);
        }

        /// <summary>
        /// Assign a key to a command.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="key">The key being added.</param>
        private void AssignKey(Commands command, Keys key)
        {
            if (CommandExists(command))
            {
                commands[command].Add(key);
            }
            else
            {
                List<Keys> keys = new List<Keys>();
                keys.Add(key);

                commands.Add(command, keys);
            }
        }

        /// <summary>
        /// Assign a list of keys to a command.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="keys">The keys being added.</param>
        private void AssignKeys(Commands command, List<Keys> keys)
        {
            foreach (Keys key in keys)
            {
                AssignKey(command, key);
            }
        }

        /// <summary>
        /// Assign an array of keys to a command.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="keys">The keys being added.</param>
        private void AssignKeys(Commands command, Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                AssignKey(command, key);
            }
        }

        /// <summary>
        /// Assign an action to a command.
        /// </summary>
        /// <param name="command">The command being affected.</param>
        /// <param name="action">The keypress type being tracked.</param>
        private void AssignAction(Commands command, ActionType action)
        {
            if (CommandExists(command))
            {
                actionTypes[command] = action;
            }
            else
            {
                actionTypes.Add(command, action);
            }
        }

        /// <summary>
        /// Determines if a command has any keys associated with it.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <returns>Returns false if command doesn't exist or if it has no keys. Returns true if it has keys associated with it.</returns>
        private bool CommandHasKeys(Commands command)
        {
            if (CommandExists(command))
            {
                if (commands[command] != null && commands[command].Count() != 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the keys associated with a given command and returns a validation boolean.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <param name="key">(Out) Keys associated with this command.</param>
        /// <returns>Returns true if there are keys. Returns false if no keys are sent back.</returns>
        public bool GetCommandKeys(Commands command, out List<Keys> keys)
        {
            keys = GetCommandKeys(command);
            return (keys != null) && (keys.Count() != 0);
        }

        /// <summary>
        /// Gets the keys associated with a given command.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <returns>Returns keys associtated with this command. Returns an empty list if command cannot be found or if none exist.</returns>
        private List<Keys> GetCommandKeys(Commands command)
        {
            if (CommandExists(command))
            {
                return commands[command];
            }
            else
            {
                return new List<Keys>();
            }
        }

        /// <summary>
        /// Gets the action type associated with a given command.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <param name="type">(Out) The keypress response associated with this command.</param>
        /// <returns>Returns the keypress response looked for in order to fire this command. Returns true if the action type isn't ActionType.Null.</returns>
        public bool GetCommandActionType(Commands command, out ActionType type)
        {
            type = GetCommandActionType(command);
            return (type != ActionType.Null);
        }

        /// <summary>
        /// Gets the action type associated with a given command.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <returns>Returns the keypress response associated with the given command.</returns>
        private ActionType GetCommandActionType(Commands command)
        {
            if (CommandExists(command))
            {
                return actionTypes[command];
            }
            else
            {
                return ActionType.Null;
            }
        }

        /// <summary>
        /// Determines if the control scheme has any commands.
        /// </summary>
        /// <returns>Returns true if there are commands. Returns false if there are none.</returns>
        public bool IsEmpty()
        {
            return ((commands == null) || (commands.Count() == 0));
        }

        #endregion

    }
}
