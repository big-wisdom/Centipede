using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    public class Mushroom : Entity
    {
        int frame = 0;
        bool poison = false;
        public bool alive = true;

        public Mushroom(int x, int y): base(new Vector2((x * 32) + 16, (y * 32) + 16), new Vector2(-16, -16), 16, 0, GameView.CharachterEnum.Mushroom)
        {
        }

        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            int x = 256 + (32 * frame);
            int y = poison ? 32 : 0;
            return new Rectangle(x, y, 32, 32);
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
            foreach (Collision c in collisions)
            {
                switch (c.entityType)
                {
                    case CharachterEnum.laser:
                        updateFrame();
                        break;
                    case CharachterEnum.Scorpion:
                        poison = true;
                        break;
                }
            }
        }

        private void updateFrame()
        {
            frame++;
            if (frame > 3) alive = false;
        }
    }
}
