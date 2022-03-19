using System;
using static Centipede.GameView;

namespace Centipede
{
    public class Collision
    {
        public CharachterEnum entityType;

        public Collision(CharachterEnum entityType)
        {
            this.entityType = entityType;
        }
    }
}
