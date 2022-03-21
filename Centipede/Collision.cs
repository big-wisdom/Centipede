using System;
using static Centipede.GameView;

namespace Centipede
{
    public class Collision
    {
        public CharachterEnum entityType;
        public float distance;

        public Collision(CharachterEnum entityType, float distance)
        {
            this.distance = distance;
            this.entityType = entityType;
        }
    }
}
