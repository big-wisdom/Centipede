using System;
using static Centipede.GameView;

namespace Centipede
{
    public class Collision
    {
        public CharachterEnum entityType;
        public int distance;

        public Collision(CharachterEnum entityType, int distance)
        {
            this.distance = distance;
            this.entityType = entityType;
        }
    }
}
