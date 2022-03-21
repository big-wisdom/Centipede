using Microsoft.Xna.Framework;
using System;
using static Centipede.GameView;

namespace Centipede
{
    public class Collision
    {
        public CharachterEnum entityType;
        public Vector2 displacement;

        public Collision(CharachterEnum entityType, Vector2 displacement)
        {
            this.displacement = displacement;
            this.entityType = entityType;
        }
    }
}
