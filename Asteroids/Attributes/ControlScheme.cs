using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Attributes
{
    public class ControlScheme
    {
        private Dictionary<Controls, List<Keys>> commands;

        public ControlScheme()
        {
            this.commands = new Dictionary<Controls, List<Keys>>();
        }

        public void AssignKey(Controls command, Keys key)
        {
            if (this.commands.ContainsKey(command))
                this.commands[command].Add(key);
            else
                this.commands.Add(command, new List<Keys>() { key });
        }

        public void AssignKeys(Controls command, List<Keys> keys)
        {
            foreach (Keys key in keys)
                this.AssignKey(command, key);
        }

        public bool HasCommand(Controls command)
        {
            return this.commands.ContainsKey(command) && (this.commands[command] != null && (uint)this.commands[command].Count<Keys>() > 0U);
        }

        public bool GetCommandKeys(Controls command, out List<Keys> key)
        {
            if (this.HasCommand(command))
            {
                key = this.commands[command];
                return true;
            }
            key = new List<Keys>();
            return false;
        }
    }
}
