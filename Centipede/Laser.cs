using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    public class Laser : Entity
    {
        public bool exists = true;
        public Laser(Vector2 position): base(position, new Vector2(-2, -12), 6, 2000, CharachterEnum.laser)
        {
            move(3* Math.PI / 2);
        }

        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            return new Rectangle(44, 320, 4, 24);
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
            position = getNextPosition(gameTime.ElapsedGameTime);
        }
    }
}
