using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Centipede.GameView;

namespace Centipede
{
    public abstract class Entity
    {
        public Vector2 position;
        int radius;
        public CharachterEnum type;

        public Entity(Vector2 position, int radius, CharachterEnum type)
        {
            this.position = position;
            this.radius = radius;
        }
    }
}
