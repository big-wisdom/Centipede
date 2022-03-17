using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    public class Ship : Entity
    {
        public Ship(Vector2 position, int radius, int maxSpeed, CharachterEnum type) : base(position, radius, maxSpeed, type)
        {
        }
    }
}
