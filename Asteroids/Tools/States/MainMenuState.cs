/// MainMenuState.cs - Version 1.
/// Author: Ian Effendi
/// Date: 4.8.2017

#region Using statements.

// System using statements.

// MonoGame using statements.

// Asteroids using statements.


#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Tools.States
{
    public class MainMenuState : State
    {

        public MainMenuState(ColorSet set, float scale = 1.0f) : base(States.Main, set, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

        public MainMenuState(Color draw, Color bg, float scale = 1.0f) : base(States.Main, set, scale)
        {
            // Any special instructions for the main menu should take place here.
        }

    }
}
