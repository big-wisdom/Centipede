using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Centipede
{
    public class Scorpion : Entity
    {
        TimeSpan animationMilliseconds = TimeSpan.FromMilliseconds(100);
        TimeSpan timeTillAnimate = TimeSpan.FromMilliseconds(100);
        int frame = 0;

        public Scorpion(Vector2 position): base(position, new Vector2(-32, -32), 64, 300, GameView.CharachterEnum.Scorpion)
        {
        }
        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            return new Rectangle(frame*64, 192, 64, 64);
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
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
                if (frame >= 4) frame = 0;
            }
        }
    }
}
