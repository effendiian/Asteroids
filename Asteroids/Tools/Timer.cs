using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Tools
{
    public class Timer
    {
        private bool running = false; // Doesn't run until started.
        private bool pause = false;
        private bool end = false;
        private bool loop = false;
        private bool cycle = false;

        private float delay = 15f; // Seconds by default.
        private float remainingDelay;

        public Timer(float _delay = 15f, bool _loop = true)
        {
            delay = _delay;
            remainingDelay = _delay;
            loop = _loop;
            cycle = false;
        }
        
        public bool End
        {
            get { return this.end; }
        }

        public bool Loop
        {
            get { return this.loop; }
        }
        
        public void Start()
        {
            if (!end || (loop && end))
            {
                running = true;
                end = false;
                pause = false;
                remainingDelay = delay;
                cycle = false;
            }
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }

        public void Stop()
        {
            running = false;
            end = true;
            remainingDelay = delay;
        }

        public bool IsFinished()
        {
            return this.cycle;
        }

        public void Update(GameTime gameTime)
        {
            if (running && !end)
            {
                if (!pause)
                {
                    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    remainingDelay -= delta;

                    if (remainingDelay <= 0)
                    {
                        Stop();
                        cycle = true;
                    }
                    else
                    {
                        cycle = false;
                    }
                }
            }
        }


    }
}
