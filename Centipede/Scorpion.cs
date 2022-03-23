using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Centipede
{
    public class Scorpion : Entity
    {
        public bool hit = false;
        public bool dead = false;
        public bool showPoints = false;

        TimeSpan animationMilliseconds = TimeSpan.FromMilliseconds(100);
        TimeSpan timeTillAnimate = TimeSpan.FromMilliseconds(100);
        TimeSpan showPointsTime = TimeSpan.FromSeconds(2);
        int frame = 0;

        public Scorpion(Vector2 position): base(position, new Vector2(-32, -32), 64, 300, GameView.CharachterEnum.Scorpion)
        {
        }
        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            if (showPoints)
            {
                return new Rectangle(374, 72, 88, 20);
            }

            if (hit)
            {
                int x = 256;
                int y = 256;
                if (frame % 2 == 0)
                {
                    x += 32 * (frame / 2);
                }
                else
                {
                    y += 32;
                    x += ((frame - 1) / 2) * 32;
                }
                return new Rectangle(x, y, 32, 32);
            } else
            {
                return new Rectangle(frame * 64, 192, 64, 64);
            }
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
            if (showPoints)
            {
                showPointsTime -= gameTime.ElapsedGameTime;
                if (showPointsTime < TimeSpan.Zero)
                {
                    dead = true;
                }
            }

            foreach (Collision c in collisions)
            {
                if (c.entityType == GameView.CharachterEnum.laser)
                {
                    hit = true;
                    maxSpeed = 0;
                }
            }
            updateFrame(gameTime.ElapsedGameTime);
            position = getNextPosition(gameTime.ElapsedGameTime);
        }

        private void updateFrame(TimeSpan time)
        {
            timeTillAnimate -= time;
            if (timeTillAnimate.TotalMilliseconds < 0)
            {
                timeTillAnimate = animationMilliseconds + timeTillAnimate;
                frame++;
                int frames = hit ? 6 : 4;
                if (frame >= frames)
                {
                    frame = 0;
                    if (hit)
                    {
                        showPoints = true;
                    }
                }
            }
        }
    }
}
